# 2.2.4 Choosing a source

[Parent: 2.2 RandomLib: random sources](random-sources.md) · [Guide map](../index.md)

| Need | Starting point |
| --- | --- |
| A seeded random source for application or simulation use | `ModuloRandom(seed: ...)` |
| The platform `System.Random` implementation | `NetRandom(seed: ...)` |
| A historical LCG preset | `SSRNGRandom(ParameterTemplate, seed: ...)` |

None of these classes is documented as cryptographically secure. Use a platform cryptographic random-number generator for tokens, passwords, keys, or other security-sensitive values.

---

← Previous: [2.2.3 SSRNGRandom](random-sources-ssrngrandom.md) · [Parent: 2.2 RandomLib: random sources](random-sources.md) · Next: [3.1 Framework overview](framework.md) →
