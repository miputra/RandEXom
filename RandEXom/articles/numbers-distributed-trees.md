# 3.4.4 Experimental distributed trees

[Parent: 3.4 Framework.Number](numbers.md) · [Guide map](../index.md)

`DistributedTreeR` and `DistributedTreeNestedR` partition a numeric range into tree leaves and favor branches with lower accumulated weights. Both expose `Next()` and `NextInt()` plus `CreateNodes(level, child)`. These types are marked obsolete. `DistributedTreeR` is also hidden from IntelliSense and has shared static node storage; `DistributedTreeNestedR` is explicitly still in development. **Do not rely on either for production behavior**. They are documented here for completeness, not recommended as an alternative to `SlotR` or `DistanceR`.

---

← Previous: [3.4.3 DistanceR: keep draws apart](numbers-distancer.md) · [Parent: 3.4 Framework.Number](numbers.md) · Next: [3.4.5 Debug-only placeholders](numbers-debug-placeholders.md) →
