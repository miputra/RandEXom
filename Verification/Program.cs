using RandEXom.SeedLib;
using RandEXom.RandomLib;
using RandEXom.Framework.Item;
using RandEXom.Framework.Number;
using RandEXom.Framework.Boolean;
using RandEXom.Interface;

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
#pragma warning disable CS0618
var tree = new DistributedTreeR(42L, level: 3, child: 2, min: 0, max: 10);
var nestedTree = new DistributedTreeNestedR(42L, level: 3, child: 2, min: 0, max: 10);
for (int i = 0; i < 100; i++)
{
    long treeValue = tree.Next();
    Check(treeValue >= 0 && treeValue < 10, "distributed tree range");
    long nestedValue = nestedTree.Next();
    Check(nestedValue >= 0 && nestedValue < 10, "nested tree range");
}
#pragma warning restore CS0618
Console.WriteLine("All verification checks passed.");

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


