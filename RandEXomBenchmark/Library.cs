using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;

namespace RandEXomBenchmark;

public sealed class Library
{
    public Assembly Assembly { get; }
    public string Path { get; }
    public Type[] Types { get; }
    public int ItemCount { get; set; } = 256;
    public string? RandomType { get; set; }
    public string? SeedType { get; set; }
    public Dictionary<string, JsonElement> Arguments { get; set; } = new();
    public List<string> Skipped { get; } = new();

    public Library(string path)
    {
        Path = System.IO.Path.GetFullPath(path);
        AssemblyLoadContext.Default.Resolving += (_, name) =>
        {
            string candidate = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path)!, name.Name + ".dll");
            return File.Exists(candidate) ? AssemblyLoadContext.Default.LoadFromAssemblyPath(candidate) : null;
        };
        Assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path);
        Types = Assembly.GetExportedTypes().Where(x => x.IsClass && !x.IsAbstract && !x.IsNested)
            .OrderBy(x => x.FullName).ToArray();
    }

    public static bool IsRandom(Type type) => type.GetMethod("NextInt", new[] { typeof(int), typeof(int) })?.ReturnType == typeof(int)
        && type.GetMethod("NextLong", new[] { typeof(long), typeof(long) })?.ReturnType == typeof(long)
        && type.GetMethod("NextBytes", new[] { typeof(byte[]) }) != null;

    public Type Find(string name) => Types.SingleOrDefault(x => x.FullName == name || x.Name == name)
        ?? throw new ArgumentException("Unknown or ambiguous type: " + name);

    public Type Close(Type type) => type.IsGenericTypeDefinition && type.GetGenericArguments().Length == 1
        ? type.MakeGenericType(typeof(int)) : type;

    public Func<object> Factory(Type original, long seed = 12345, bool remove = false, HashSet<Type>? chain = null)
    {
        Type type = Close(original);
        chain = chain == null ? new() : new(chain);
        if (!chain.Add(type)) throw new NotSupportedException("Circular constructor dependency: " + type.Name);
        if (type.ContainsGenericParameters) throw new NotSupportedException("Only one generic item parameter is supported.");
        List<string> errors = new();
        foreach (var ctor in type.GetConstructors().OrderByDescending(x =>
            x.GetParameters().Count(p => Arguments.ContainsKey(p.Name!)) * 1000 +
            x.GetParameters().Count(p => p.ParameterType.IsInterface) * 100 +
            x.GetParameters().Count(p => p.Name == "seed") * 10).ThenBy(x => x.GetParameters().Length))
        {
            try
            {
                var args = ctor.GetParameters().Select(p => Argument(p, seed, remove, chain)).ToArray();
                return Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(ctor, args), typeof(object))).Compile();
            }
            catch (Exception ex) when (ex is NotSupportedException or ArgumentException or JsonException)
            { errors.Add(ex.Message); }
        }
        throw new NotSupportedException("No supported constructor for " + type.FullName + ": " + string.Join("; ", errors.Distinct()));
    }

    private Expression Argument(ParameterInfo p, long seed, bool remove, HashSet<Type> chain)
    {
        Type type = p.ParameterType;
        object? value;
        if (p.Name == "remove_on_pull") return Expression.Constant(remove);
        if (Arguments.TryGetValue(p.Name!, out var setting))
        {
            value = type.IsEnum ? Enum.Parse(type, setting.GetString()!) : setting.Deserialize(type);
            return Expression.Constant(value, type);
        }
        if (p.Name == "seed" && (type == typeof(long) || type == typeof(long?) || type == typeof(int)))
            return Expression.Convert(Expression.Constant(seed), type);
        if (p.Name == "reset_on_empty") return Expression.Constant(false);
        if (type.IsAssignableFrom(typeof(int[])) && type != typeof(object))
            return Expression.Convert(Expression.Constant(Enumerable.Range(0, ItemCount).ToArray()), type);
        if (type.IsInterface)
        {
            bool random = type.GetMethod("NextInt", new[] { typeof(int), typeof(int) }) != null;
            string? selected = random ? RandomType : SeedType;
            //without a seed injection, prefer the numeric seed constructor
            if (!random && selected == null) throw new NotSupportedException("Use a numeric seed or --seed-type for " + type.Name);
            Type dependency = selected != null ? Find(selected) : Types.FirstOrDefault(x => type.IsAssignableFrom(x) && !chain.Contains(x))
                ?? throw new NotSupportedException("No implementation for " + type.Name);
            if (!type.IsAssignableFrom(dependency)) throw new NotSupportedException(dependency.Name + " does not implement " + type.Name);
            return Expression.Convert(Expression.Invoke(Expression.Constant(Factory(dependency, seed, remove, chain))), type);
        }
        if (p.HasDefaultValue) return Expression.Constant(p.DefaultValue, type);
        value = p.Name switch
        {
            "percentage" when type == typeof(float) => 25f,
            _ => throw new NotSupportedException("Provide a JSON constructor argument for " + p.Name + " (" + type.Name + ")")
        };
        return Expression.Constant(value, type);
    }

    public List<Workload> Discover(string filter, bool includeObsolete)
    {
        List<Workload> cases = new();
        foreach (Type original in Types)
        {
            if (!(original.FullName ?? original.Name).Contains(filter, StringComparison.OrdinalIgnoreCase)) continue;
            if (!includeObsolete && original.IsDefined(typeof(ObsoleteAttribute)))
            { Skipped.Add(original.FullName + ": obsolete (use --include-obsolete to opt in)"); continue; }
            try
            {
                Type type = Close(original);
                bool hasRemoval = type.GetConstructors().Any(c => c.GetParameters().Any(p => p.Name == "remove_on_pull"));
                foreach (bool remove in hasRemoval ? new[] { false, true } : new[] { false })
                {
                    var factory = Factory(type, remove: remove);
                    string label = original.FullName + (hasRemoval ? (remove ? "/remove" : "/keep") : "");
                    cases.Add(new Workload(label + ".ctor", () => () => { GC.KeepAlive(factory()); return 0; }, 64,
                        "construction including injected dependencies"));
                    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => !m.IsSpecialName && m.DeclaringType != typeof(object)))
                    {
                        try
                        {
                            MethodInfo callable = method;
                            if (method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 1)
                                callable = method.MakeGenericMethod(typeof(int));
                            MethodArguments(callable);
                            bool pull = callable.Name == "Pull";
                            string id = label + "." + callable.Name + "(" + string.Join(",", callable.GetParameters().Select(p => p.ParameterType.Name)) + ")";
                            cases.Add(new Workload(id, () =>
                            {
                                object instance = factory();
                                //fill bags outside the timed region
                                var add = type.GetMethod("AddItem", new[] { typeof(int), typeof(int) });
                                if (add != null)
                                    for (int i = 0; i < ItemCount; i++) add.Invoke(instance, new object[] { i, 1 });
                                return Bind(instance, callable, MethodArguments(callable));
                            }, pull ? ItemCount : 4096, pull ? "one pull; fresh full pool per sample" : "one call; setup excluded"));
                        }
                        catch (NotSupportedException) { Skipped.Add(original.FullName + "." + method.Name + ": unsupported method shape"); }
                    }
                }
            }
            catch (Exception ex) { Skipped.Add(original.FullName + ": " + ex.Message); }
        }
        return cases;
    }

    private object?[] MethodArguments(MethodInfo method)
    {
        Type[] p = method.GetParameters().Select(x => x.ParameterType).ToArray();
        if ((method.Name is "Next" or "NextInt" or "NextLong") && p.SequenceEqual(new[] { typeof(int), typeof(int) })) return new object[] { 0, 1000000 };
        if ((method.Name is "Next" or "NextInt" or "NextLong") && p.SequenceEqual(new[] { typeof(long), typeof(long) })) return new object[] { 0L, 1000000L };
        if ((method.Name is "Next" or "NextInt" or "NextLong" or "Pull" or "Count" or "CountType" or "ToList" or "GetSeed" or "GetSeedSTR") && p.Length == 0) return Array.Empty<object>();
        if (method.Name == "NextBytes" && p.SequenceEqual(new[] { typeof(byte[]) })) return new object[] { new byte[1024] };
        if (method.Name == "Shuffle" && p.SequenceEqual(new[] { typeof(List<int>) })) return new object[] { Enumerable.Range(0, ItemCount).ToList() };
        throw new NotSupportedException();
    }

    private static Func<long> Bind(object instance, MethodInfo method, object?[] args)
    {
        var call = Expression.Call(Expression.Constant(instance), method,
            args.Select((x, i) => Expression.Constant(x, method.GetParameters()[i].ParameterType)));
        Expression result = method.ReturnType == typeof(int) || method.ReturnType == typeof(long)
            ? Expression.Convert(call, typeof(long))
            : method.ReturnType == typeof(bool) ? Expression.Condition(call, Expression.Constant(1L), Expression.Constant(0L))
            : method.ReturnType == typeof(void) ? Expression.Block(call, Expression.Constant(0L))
            : Expression.Block(Expression.Call(typeof(GC).GetMethod(nameof(GC.KeepAlive))!, Expression.Convert(call, typeof(object))), Expression.Constant(0L));
        //compile once so the measurement excludes reflection invocation and boxing
        return Expression.Lambda<Func<long>>(result).Compile();
    }
}

public sealed record Workload(string Name, Func<Func<long>> Setup, int CallsPerSample, string Unit);
