#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Core;

namespace Shop.EditorTools
{
    internal static class BundleDataValidator
    {
        public static bool Validate(BundleData bd)
        {
            if (!bd) return false;

            bool changed = false;

            if (SerializationUtility.HasManagedReferencesWithMissingTypes(bd))
            {
                SerializationUtility.ClearAllManagedReferencesWithMissingTypes(bd);
                changed = true;
            }

            changed |= AutoEnsureParams(bd.Costs as List<CostEntry>);
            changed |= AutoEnsureParams(bd.Rewards as List<RewardEntry>);

            changed |= EnsureParamsAreUnique(bd.Costs as List<CostEntry>);
            changed |= EnsureParamsAreUnique(bd.Rewards as List<RewardEntry>);

            changed |= EnsureNoDuplicateOperations(bd.Costs as List<CostEntry>, "Costs");
            changed |= EnsureNoDuplicateOperations(bd.Rewards as List<RewardEntry>, "Rewards");

            return changed;
        }

        private static bool AutoEnsureParams(List<CostEntry> list)
        {
            bool changed = false;
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (e == null) continue;

                if (!e.Operation)
                {
                    if (e.Parameter != null) { e.Parameter = null; changed = true; }
                }
                else if (e.Operation is IOperationWithParameter op)
                {
                    if (e.Parameter == null || !op.IsSupports(e.Parameter))
                    {
                        e.Parameter = op.CreateDefaultParam();
                        changed = true;
                    }
                }
                else
                {
                    if (e.Parameter != null) { e.Parameter = null; changed = true; }
                }
                list[i] = e;
            }
            return changed;
        }

        private static bool AutoEnsureParams(List<RewardEntry> list)
        {
            bool changed = false;
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (e == null) continue;

                if (!e.Operation)
                {
                    if (e.Parameter != null) { e.Parameter = null; changed = true; }
                }
                else if (e.Operation is IOperationWithParameter op)
                {
                    if (e.Parameter == null || !op.IsSupports(e.Parameter))
                    {
                        e.Parameter = op.CreateDefaultParam();
                        changed = true;
                    }
                }
                else
                {
                    if (e.Parameter != null) { e.Parameter = null; changed = true; }
                }
                list[i] = e;
            }
            return changed;
        }

        private static bool EnsureParamsAreUnique(List<CostEntry> list)
        {
            bool changed = false;
            var seen = new HashSet<IOperationParameter>(ReferenceEqualityComparer<IOperationParameter>.Instance);
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (e?.Parameter != null && !seen.Add(e.Parameter))
                {
                    e.Parameter = CloneParam(e.Parameter);
                    changed = true;
                }
                list[i] = e;
            }
            return changed;
        }

        private static bool EnsureParamsAreUnique(List<RewardEntry> list)
        {
            bool changed = false;
            var seen = new HashSet<IOperationParameter>(ReferenceEqualityComparer<IOperationParameter>.Instance);
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (e?.Parameter != null && !seen.Add(e.Parameter))
                {
                    e.Parameter = CloneParam(e.Parameter);
                    changed = true;
                }
                list[i] = e;
            }
            return changed;
        }

        private static bool EnsureNoDuplicateOperations(List<CostEntry> list, string listName)
        {
            bool changed = false;
            var seen = new HashSet<int>();
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (!e?.Operation) continue;

                int id = e.Operation.GetInstanceID();
                if (!seen.Add(id))
                {
                    Debug.LogWarning($"[BundleData] Duplicate operation in {listName}" +
                                     $" at index {i}: {e.Operation.name}. Slot cleared.");
                    e.Operation = null;
                    e.Parameter = null;
                    list[i] = e;
                    changed = true;
                }
            }
            return changed;
        }

        private static bool EnsureNoDuplicateOperations(List<RewardEntry> list, string listName)
        {
            bool changed = false;
            var seen = new HashSet<int>();
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (!e?.Operation) continue;

                int id = e.Operation.GetInstanceID();
                if (!seen.Add(id))
                {
                    Debug.LogWarning($"[BundleData] Duplicate operation in {listName}" +
                                     $" at index {i}: {e.Operation.name}. Slot cleared.");
                    e.Operation = null;
                    e.Parameter = null;
                    list[i] = e;
                    changed = true;
                }
            }
            return changed;
        }
        
        private static IOperationParameter CloneParam(IOperationParameter src)
	        => (IOperationParameter)JsonUtility.FromJson(JsonUtility.ToJson(src), src.GetType());
    }
}
#endif