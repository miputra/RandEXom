using RandEXom.SeedLib;
using RandEXom.RandomLib;
using RandEXom.Framework.Item;
using RandEXom.Framework.Number;
using RandEXom.Framework.Boolean;
using RandEXom.Interface;
using RandEXom.Utility;
using System.Net;
using System.Net.Http;

static void Check(bool ok, string name)
{
    if (!ok) throw new Exception(name);
}

Check(new LCGSeedR(0).now == 0, "integer zero LCG seed");
Check(new SSRNGRandom(0).GetSeed().init == 0, "integer zero SSRNG seed");
Check(new ModuloRandom(0).GetSeed().init == 0, "integer zero modulo seed");
Check(new GachaR<string>().Pull() == null, "empty gacha");
var invalidRange = false;
try { new ModuloRandom(1).NextInt(5, 5); }
catch (ArgumentOutOfRangeException) { invalidRange = true; }
Check(invalidRange, "invalid bounded range");

var lcg = new LCGSeedR(seed: 0);
long[] reference = { 0, 0x3C6EF35F, 0x47502932, 0xD1CCF6E9, 0xAAF95334 };
foreach (long expected in reference)
{
    Check(lcg.now == expected, "Numerical Recipes LCG sequence");
    lcg.Next();
}
var apple = new SSRNGRandom(SSRNGRandom.ParameterTemplate.Apple_CarbonLib, 1);
apple.GetSeed().Next();
Check(apple.GetSeed().now == 16807, "Apple preset multiplier and modulus");

var stuck = new XORShift64Seed(long.MinValue);
stuck.Next();
Check(stuck.now != long.MinValue, "xorshift minimum seed advances");
var star = new XORShift64Seed(XORShift64Seed.Type.Xorshift64_star, 1);
star.Next();
Check(star.now == 33554433, "xorshift64* transition");
var starRandom = new ModuloRandom(ModuloRandom.Multiplier.XORShift64_Star, 1);
byte[] starByte = new byte[1];
starRandom.NextBytes(starByte);
Check(starByte[0] == (byte)(unchecked((ulong)33554433 * 2685821657736338717UL) >> 56), "xorshift64* multiplier");

// Reference sequence from Vigna's Figure 10, starting from state 1.
long[] starStates = { 1126174793148417, 3659449627584515, 2306758490171379329, 585415316980522496 };
byte[] starBytes = { 171, 185, 77, 14 };
for (int i = 0; i < starStates.Length; i++)
{
    long oldState = star.now;
    star.Next();
    Check(star.previous == oldState && star.now == starStates[i], "xorshift64* reference states");
    starRandom.NextBytes(starByte);
    Check(starByte[0] == starBytes[i], "xorshift64* reference output bytes");
    Check(starRandom.GetSeed().now == starStates[i], "xorshift64* retains unmultiplied state");
}

var bytes = new byte[8];
var modulo = new ModuloRandom(seed: 123);
modulo.NextBytes(bytes);
var seed = new XORShift64Seed(123);
for (int i = 0; i < bytes.Length; i++)
{
    seed.Next();
    Check(bytes[i] == (byte)(unchecked((ulong)seed.now) >> 56), "byte state advancement");
}

for (int i = 0; i < 1000; i++)
{
    long value = modulo.NextLong(long.MinValue, long.MaxValue);
    Check(value >= long.MinValue && value < long.MaxValue, "wide modulo range");
}

for (int draw = 0; draw < 5; draw++)
{
    var pool = new GachaRBatched<string>(new FixedRandom(draw));
    pool.AddItem("A", 2);
    pool.AddItem("B", 3);
    Check(pool.Pull() == (draw < 2 ? "A" : "B"), "weighted draw mapping");
}
var equal = new GachaRBatched<string>(new FixedRandom(0));
equal.AddItem("A", 1); equal.AddItem("B", 1);
Check(equal.Pull() == "A", "equal weights");

var percent = new TruePercentageR(25);
int trues = 0;
for (int i = 0; i < 10000; i++) if (percent.Next()) trues++;
Check(trues > 2200 && trues < 2800, "percentage frequency");
Check(!new TruePercentageR(new FixedRandom(1_073_741_824), 25).Next(), "percentage upper boundary");
Check(new TruePercentageR(new FixedRandom(1_073_741_823), 25).Next(), "percentage lower boundary");

var distance = new DistanceR(42, 10);
long previous = distance.Next(0L, 100L);
for (int i = 0; i < 100; i++)
{
    long next = distance.Next(0L, 100L);
    Check(next >= 0 && next < 100 && Math.Abs(next - previous) >= 10, "distance");
    previous = next;
}
Check(new SlotR(42).Next(999, 1000) == 999, "narrow slot range");
// Visit every branch path and every item position in each endpoint.
for (int endpoint = 0; endpoint < 4; endpoint++)
{
    for (int item = 0; item < 3; item++)
    {
        var source = new SequenceRandom(endpoint / 2, endpoint % 2, item);
        var itemTree = new DistributedTreeR<int>(Enumerable.Range(1, 12), source, step: 2);
        Check(itemTree.Pull() == endpoint * 3 + item + 1, "ordered endpoint mapping");
        Check(source.Calls == 3 && itemTree.Count() == 12, "branch draws and non-removing pull");
    }
}
for (int endpoint = 0; endpoint < 3; endpoint++)
{
    int count = endpoint < 2 ? 4 : 3;
    int start = endpoint * 4;
    for (int item = 0; item < count; item++)
        Check(new DistributedTreeR<int>(Enumerable.Range(0, 11),
            new SequenceRandom(endpoint, item), child: 3).Pull() == start + item,
            "uneven endpoint mapping");
}
var originalItems = Enumerable.Range(0, 16).ToList();
var shuffledTree = new DistributedTreeR<int>(originalItems,
    new ModuloRandom(new XORShift64Seed(123)), step: 3, do_shuffle: true);
var replayTree = new DistributedTreeR<int>(originalItems,
    new ModuloRandom(new XORShift64Seed(123)), step: 3, do_shuffle: true);
Check(shuffledTree.ToList().OrderBy(x => x).SequenceEqual(originalItems), "shuffle preserves items");
Check(!shuffledTree.ToList().SequenceEqual(originalItems), "shuffle changes placement");
Check(originalItems.SequenceEqual(Enumerable.Range(0, 16)), "tree copies input");
for (int i = 0; i < 100; i++)
    Check(shuffledTree.Pull() == replayTree.Pull(), "modular seed and random replay");
Check(new DistributedTreeR<int>(new[] { 7 }, new SequenceRandom(0), step: 0).Pull() == 7,
    "zero step endpoint");
Check(new DistributedTreeR<int>(new[] { 7 }, new SequenceRandom(0, 0, 0), step: 2, child: 1).Pull() == 7,
    "single branch tree");
foreach (var config in new[] { (step: -1, child: 2), (step: 1, child: 0), (step: 2, child: 2), (step: 100, child: int.MaxValue) })
{
    bool rejected = false;
    try { new DistributedTreeR<int>(new[] { 1, 2 }, step: config.step, child: config.child); }
    catch (ArgumentOutOfRangeException) { rejected = true; }
    Check(rejected, "invalid item tree configuration");
}
bool emptyRejected = false;
try { new DistributedTreeR<int>(Array.Empty<int>()); }
catch (ArgumentException) { emptyRejected = true; }
Check(emptyRejected, "empty item tree");
var removalSource = new SequenceRandom(
    0, 0, 0,  // first endpoint: 1
    0, 1, 0,  // second endpoint still starts at 3
    0, 0, 0,  // empty first endpoint
    0, 0, 0,  // only second endpoint remains on left; empty left subtree
    0, 1, 0,  // only right subtree remains; select last endpoint
    0, 1, 0,  // empty last endpoint
    0, 0, 0,
    0, 0, 0);
var removalTree = new DistributedTreeR<int>(Enumerable.Range(1, 8), removalSource,
    step: 2, remove_on_pull: true);
int[] removalOrder = { 1, 3, 2, 4, 7, 8, 5, 6 };
for (int i = 0; i < removalOrder.Length; i++)
{
    Check(removalTree.Pull() == removalOrder[i], "locked endpoint and subtree traversal");
    Check(removalTree.Count() == 7 - i, "remaining tree count");
    Check(removalTree.ToList().SequenceEqual(Enumerable.Range(1, 8).Except(removalOrder.Take(i + 1))),
        "remaining items stay in original endpoints");
}
Check(removalTree.Pull() == 0 && removalSource.Calls == 24, "empty tree does not draw or refill");
foreach (int branches in new[] { 1, 2 })
{
    var single = new DistributedTreeR<string>(new[] { "A" }, 42L,
        step: branches == 1 ? 3 : 0, child: branches, remove_on_pull: true);
    Check(single.Pull() == "A" && single.Pull() == null && single.Count() == 0,
        "single endpoint exhaustion");
}
var shuffledRemoval = new DistributedTreeR<int>(Enumerable.Range(0, 17),
    new ModuloRandom(new XORShift64Seed(123)), step: 2, child: 3,
    do_shuffle: true, remove_on_pull: true);
var removedItems = new HashSet<int>();
while (shuffledRemoval.Count() > 0)
    Check(removedItems.Add(shuffledRemoval.Pull()), "shuffled removal has no repeated index");
Check(removedItems.SetEquals(Enumerable.Range(0, 17)), "shuffled uneven endpoints fully drain");
var duplicates = new DistributedTreeR<string>(new[] { "A", "A" }, remove_on_pull: true);
Check(duplicates.Pull() == "A" && duplicates.Count() == 1 && duplicates.Pull() == "A"
    && duplicates.Count() == 0, "duplicate values removed separately");
using (var client = new HttpClient(new SeedHttpHandler((request, _) =>
{
    Check(request.Method == HttpMethod.Get && request.RequestUri?.Scheme == "https"
        && request.RequestUri.Host == "www.random.org"
        && request.RequestUri.Query.Contains("num=2"), "Random.org request");
    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
    { Content = new StringContent("123456789\n987654321\n") });
})))
    Check(await SeedGenerator.GetRandomOrgSeedAsync(client) == 123456789987654321L,
        "Random.org seed combines both values");
using (var client = new HttpClient(new SeedHttpHandler((_, _) =>
    Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
    { Content = new StringContent("Error: quota exhausted") }))))
{
    bool rejected = false;
    try { await SeedGenerator.GetRandomOrgSeedAsync(client); }
    catch (InvalidOperationException ex) { rejected = ex.Message.Contains("quota exhausted"); }
    Check(rejected, "Random.org service error");
}
using (var client = new HttpClient(new SeedHttpHandler((_, _) =>
    Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
    { Content = new StringContent("1\n1000000000\n") }))))
{
    bool rejected = false;
    try { await SeedGenerator.GetRandomOrgSeedAsync(client); }
    catch (System.IO.InvalidDataException) { rejected = true; }
    Check(rejected, "Random.org invalid result");
}
using (var client = new HttpClient(new SeedHttpHandler((_, _) =>
    Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)))))
{
    bool rejected = false;
    try { await SeedGenerator.GetRandomOrgSeedAsync(client); }
    catch (HttpRequestException) { rejected = true; }
    Check(rejected, "Random.org HTTP failure");
}
int activeRequests = 0;
int maxActiveRequests = 0;
using (var client = new HttpClient(new SeedHttpHandler(async (_, token) =>
{
    int active = Interlocked.Increment(ref activeRequests);
    maxActiveRequests = Math.Max(maxActiveRequests, active);
    await Task.Delay(20, token);
    Interlocked.Decrement(ref activeRequests);
    return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("1\n2\n") };
})))
{
    long[] seeds = await Task.WhenAll(SeedGenerator.GetRandomOrgSeedAsync(client),
        SeedGenerator.GetRandomOrgSeedAsync(client));
    Check(seeds.SequenceEqual(new[] { 1000000002L, 1000000002L }) && maxActiveRequests == 1,
        "Random.org calls are sequential");
}
Console.WriteLine("All verification checks passed.");

sealed class SeedHttpHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> respond;
    public SeedHttpHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> respond)
    {
        this.respond = respond;
    }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => respond(request, cancellationToken);
}

sealed class SequenceRandom : IRandomR
{
    private readonly Queue<int> draws;
    public int Calls { get; private set; }
    public SequenceRandom(params int[] draws) { this.draws = new Queue<int>(draws); }
    public string GetSeedSTR() => "0";
    public ISeedR GetSeed() => new XORShift64Seed(1);
    public int NextInt(int min, int max)
    {
        int value = draws.Dequeue();
        if (value < min || value >= max) throw new Exception("Unexpected tree draw range");
        Calls++;
        return value;
    }
    public long NextLong(long min, long max) => throw new NotSupportedException();
    public void NextBytes(byte[] buffers) => throw new NotSupportedException();
}

sealed class FixedRandom : IRandomR
{
    private readonly long draw;
    public FixedRandom(long draw) { this.draw = draw; }
    public string GetSeedSTR() => "0";
    public ISeedR GetSeed() => new XORShift64Seed(1);
    public int NextInt(int min, int max) => min + (int)draw;
    public long NextLong(long min, long max) => min + draw;
    public void NextBytes(byte[] buffers) => Array.Clear(buffers, 0, buffers.Length);
}


