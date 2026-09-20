# 4.1.1 Public: `SeedGenerator`

[Parent: 4.1 Utility namespace](utility.md) · [Guide map](../index.md)

`SeedGenerator` offers a local clock seed and an optional seed from [Random.org's HTTPS integer service](https://www.random.org/clients/http/api/). Both return a `long` that you can pass to a seeded constructor.

## Local clock seed

```csharp
using RandEXom.RandomLib;
using RandEXom.Utility;

long seed = SeedGenerator.GetJoinedCurrentDate();
var random = new ModuloRandom(seed);
```

`GetJoinedCurrentDate()` concatenates parts of the *local* current date and time (milliseconds, seconds, minutes, hours, day, month, and two-digit year). It does not pad fields, so different timestamps can form the same digits. This method does not promise a unique or cryptographically secure seed. Use a fixed seed for reproducible tests.

## Random.org seed

```csharp
using RandEXom.RandomLib;
using RandEXom.Utility;

long seed = await SeedGenerator.GetRandomOrgSeedAsync();
var random = new ModuloRandom(seed);
```

`GetRandomOrgSeedAsync()` makes one HTTPS request for two new independent integers in `[0, 999999999]`, then combines them as `first * 1_000_000_000 + second`. The result is nonnegative and below `1_000_000_000_000_000_000`, so it fits in a `long`. It is a seed for your local generator, not a stream of Random.org results. This method uses Random.org's [keyless HTTP integer interface](https://www.random.org/clients/http/api/); no API key is required. It does not use the separate [JSON-RPC API](https://api.random.org/json-rpc/4/basic), which requires a key.

The request is asynchronous and accepts an optional `CancellationToken`. The default HTTP client has a three-minute timeout, and requests made through this helper are serialized so it does not send parallel calls. Network, HTTP, quota, cancellation, and invalid-response failures are propagated; the helper does not substitute a local seed. Random.org's [automated-client guidelines](https://www.random.org/clients/) recommend infrequent requests, a generous timeout, quota awareness, and a contact email in the User-Agent for regular automated use. For frequent use, generate one seed and reuse a local random source, or use an appropriate Random.org API client. The default client identifies this project by its repository URL.

If you need a configured client or want to test without using Random.org's quota, use the overload:

```csharp
using System.Net.Http;
using RandEXom.Utility;

using var client = new HttpClient { Timeout = TimeSpan.FromMinutes(3) };
client.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp/1.0 (contact@example.com)");
long seed = await SeedGenerator.GetRandomOrgSeedAsync(client);
```

You own and dispose a supplied client. For tests, supply a client with a fake `HttpMessageHandler`; the project's [Verification](extending.md#verification) uses this approach and makes no live Random.org request. A Random.org seed is not reproducible unless you save its returned value. It does not make RandEXom's random generators cryptographically secure.

---

← Previous: [4.1 Utility namespace](utility.md) · [Parent: 4.1 Utility namespace](utility.md) · Next: [4.1.2 Internal: TypeR.RoundLongToInt(long)](utility-typer.md) →
