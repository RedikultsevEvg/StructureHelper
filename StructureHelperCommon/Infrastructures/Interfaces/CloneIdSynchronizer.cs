using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Synchronizes IDs through a source/clone object graph after a legacy clone operation.
    /// Used while explicit mode-aware strategies are being introduced.
    /// </summary>
    public static class CloneIdSynchronizer
    {
        public static void Synchronize(object sourceObject, object cloneObject, CloningMode cloningMode)
        {
            if (cloningMode != CloningMode.PreserveIdentity)
            {
                return;
            }

            Synchronize(sourceObject, cloneObject, new HashSet<(object, object)>());
        }

        private static void Synchronize(object? sourceObject, object? cloneObject, HashSet<(object, object)> visited)
        {
            if (sourceObject is null || cloneObject is null || ReferenceEquals(sourceObject, cloneObject))
            {
                return;
            }

            if (!visited.Add((sourceObject, cloneObject)))
            {
                return;
            }

            if (sourceObject.GetType().IsValueType)
            {
                return;
            }

            if (sourceObject is ISaveable sourceSaveable && cloneObject is ISaveable cloneSaveable
                && sourceSaveable.Id != cloneSaveable.Id)
            {
                LegacyCloneHelper.SetId(cloneSaveable, sourceSaveable.Id);
            }

            foreach (var property in sourceObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetIndexParameters().Length != 0 || property.Name == nameof(ISaveable.Id))
                {
                    continue;
                }

                object? sourceValue;
                object? cloneValue;
                try
                {
                    sourceValue = property.GetValue(sourceObject);
                    cloneValue = property.GetValue(cloneObject);
                }
                catch (TargetInvocationException)
                {
                    continue;
                }

                if (sourceValue is null || cloneValue is null)
                {
                    continue;
                }

                if (sourceValue is string)
                {
                    continue;
                }

                if (sourceValue is IEnumerable sourceEnumerable && cloneValue is IEnumerable cloneEnumerable)
                {
                    SynchronizeEnumerable(sourceEnumerable, cloneEnumerable, visited);
                }
                else
                {
                    Synchronize(sourceValue, cloneValue, visited);
                }
            }
        }

        private static void SynchronizeEnumerable(
            IEnumerable sourceEnumerable,
            IEnumerable cloneEnumerable,
            HashSet<(object, object)> visited)
        {
            var sourceEnumerator = sourceEnumerable.GetEnumerator();
            var cloneEnumerator = cloneEnumerable.GetEnumerator();
            while (sourceEnumerator.MoveNext() && cloneEnumerator.MoveNext())
            {
                Synchronize(sourceEnumerator.Current, cloneEnumerator.Current, visited);
            }
        }
    }
}
