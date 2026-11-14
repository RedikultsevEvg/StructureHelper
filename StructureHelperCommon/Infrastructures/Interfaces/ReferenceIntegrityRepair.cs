using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Replaces references to source objects inside a cloned graph with the corresponding
    /// cloned objects, preserving reference identity inside the clone.
    /// </summary>
    public static class ReferenceIntegrityRepair
    {
        public static void Repair(object cloneRoot, IDictionary<object, object> sourceToClone)
        {
            if (cloneRoot is null || sourceToClone is null || sourceToClone.Count == 0)
            {
                return;
            }

            RepairObject(cloneRoot, sourceToClone, new HashSet<object>());
        }

        private static void RepairObject(
            object cloneObject,
            IDictionary<object, object> sourceToClone,
            HashSet<object> visited)
        {
            if (cloneObject is null || !visited.Add(cloneObject))
            {
                return;
            }

            if (cloneObject.GetType().IsValueType || cloneObject is string)
            {
                return;
            }

            foreach (var property in cloneObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead || property.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                object? value;
                try
                {
                    value = property.GetValue(cloneObject);
                }
                catch (TargetInvocationException)
                {
                    continue;
                }

                if (value is null)
                {
                    continue;
                }

                if (sourceToClone.TryGetValue(value, out var replacement)
                    && !ReferenceEquals(value, replacement))
                {
                    if (property.CanWrite)
                    {
                        TrySet(property, cloneObject, replacement);
                    }
                    continue;
                }

                if (value is IEnumerable enumerable && value is not string)
                {
                    if (value is IList list)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            var item = list[i];
                            if (item is not null && sourceToClone.TryGetValue(item, out var itemReplacement))
                            {
                                list[i] = itemReplacement;
                            }
                            else if (item is not null)
                            {
                                RepairObject(item, sourceToClone, visited);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in enumerable)
                        {
                            if (item is not null && sourceToClone.TryGetValue(item, out var itemReplacement))
                            {
                                // Non-list enumerables cannot be changed in place.
                                _ = itemReplacement;
                            }
                            else if (item is not null)
                            {
                                RepairObject(item, sourceToClone, visited);
                            }
                        }
                    }
                    continue;
                }

                if (!value.GetType().IsValueType)
                {
                    RepairObject(value, sourceToClone, visited);
                }
            }
        }

        private static void TrySet(PropertyInfo property, object target, object? value)
        {
            try
            {
                property.SetValue(target, value);
            }
            catch (ArgumentException)
            {
            }
            catch (TargetInvocationException)
            {
            }
        }
    }
}
