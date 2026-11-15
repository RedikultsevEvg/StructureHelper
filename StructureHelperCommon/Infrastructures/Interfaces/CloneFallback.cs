using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Transitional clone implementation for types that do not yet have a hand-written
    /// mode-aware GetClone implementation. Existing legacy Clone() methods are reused when
    /// present; otherwise public properties and collections are copied recursively.
    /// </summary>
    public static class CloneFallback
    {
        public static T GetClone<T>(T sourceObject, CloneMode cloneMode, CloneDepth cloneDepth)
            where T : class
        {
            if (sourceObject is null)
            {
                return null!;
            }

            if (cloneMode == CloneMode.Reference)
            {
                return sourceObject;
            }

            if (cloneDepth == CloneDepth.Deep)
            {
                var legacyClone = TryLegacyClone(sourceObject);
                if (legacyClone is not null && !ReferenceEquals(legacyClone, sourceObject))
                {
                    ApplyIdentity(sourceObject, legacyClone, cloneMode, cloneDepth);
                    if (cloneMode == CloneMode.PreserveIdentity)
                    {
                        SynchronizeIds(sourceObject, legacyClone);
                    }
                    return (T)legacyClone;
                }
            }

            var result = (T)CloneObject(sourceObject, cloneMode, cloneDepth, new Dictionary<object, object>());
            if (cloneMode == CloneMode.PreserveIdentity)
            {
                SynchronizeIds(sourceObject, result);
            }
            return result;
        }

        /// <summary>
        /// Restores source IDs recursively on an already-created clone graph.
        /// </summary>
        public static void SynchronizeIds(object sourceObject, object cloneObject)
        {
            SynchronizeIds(sourceObject, cloneObject, new HashSet<(object, object)>());
        }

        private static void SynchronizeIds(
            object? sourceObject,
            object? cloneObject,
            HashSet<(object, object)> visited)
        {
            if (sourceObject is null || cloneObject is null || ReferenceEquals(sourceObject, cloneObject))
            {
                return;
            }

            if (!visited.Add((sourceObject, cloneObject)))
            {
                return;
            }

            if (sourceObject is ISaveable sourceSaveable && cloneObject is ISaveable cloneSaveable
                && sourceSaveable.Id != cloneSaveable.Id)
            {
                SetId(cloneSaveable, sourceSaveable.Id);
            }

            if (sourceObject.GetType().IsValueType)
            {
                return;
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

                if (sourceValue is null || cloneValue is null || sourceValue is string)
                {
                    continue;
                }

                if (sourceValue is IEnumerable sourceEnumerable && cloneValue is IEnumerable cloneEnumerable)
                {
                    var sourceEnumerator = sourceEnumerable.GetEnumerator();
                    var cloneEnumerator = cloneEnumerable.GetEnumerator();
                    while (sourceEnumerator.MoveNext() && cloneEnumerator.MoveNext())
                    {
                        SynchronizeIds(sourceEnumerator.Current, cloneEnumerator.Current, visited);
                    }
                }
                else
                {
                    SynchronizeIds(sourceValue, cloneValue, visited);
                }
            }
        }

        private static object? TryLegacyClone(object sourceObject)
        {
            var cloneMethod = sourceObject.GetType().GetMethod(
                "Clone",
                BindingFlags.Public | BindingFlags.Instance,
                binder: null,
                types: Type.EmptyTypes,
                modifiers: null);

            if (cloneMethod is null || cloneMethod.ReturnType == typeof(void))
            {
                return null;
            }

            try
            {
                return cloneMethod.Invoke(sourceObject, null);
            }
            catch (TargetInvocationException)
            {
                return null;
            }
        }

        private static object CloneObject(
            object sourceObject,
            CloneMode cloneMode,
            CloneDepth cloneDepth,
            Dictionary<object, object> visited)
        {
            if (visited.TryGetValue(sourceObject, out var cached))
            {
                return cached;
            }

            var sourceType = sourceObject.GetType();
            if (IsSimple(sourceType))
            {
                return sourceObject;
            }

            object clone;
            var parameterlessConstructor = sourceType.GetConstructor(Type.EmptyTypes);
            if (parameterlessConstructor is not null)
            {
                clone = parameterlessConstructor.Invoke(null);
            }
            else
            {
                clone = RuntimeHelpers.GetUninitializedObject(sourceType);
            }

            visited[sourceObject] = clone;

            foreach (var property in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead || property.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                if (property.Name == nameof(ISaveable.Id))
                {
                    continue;
                }

                object? sourceValue;
                try
                {
                    sourceValue = property.GetValue(sourceObject);
                }
                catch (TargetInvocationException)
                {
                    continue;
                }

                if (sourceValue is null)
                {
                    if (property.CanWrite)
                    {
                        TrySet(property, clone, null);
                    }
                    continue;
                }

                if (IsSimple(sourceValue.GetType()))
                {
                    if (property.CanWrite)
                    {
                        TrySet(property, clone, sourceValue);
                    }
                    continue;
                }

                if (sourceValue is IEnumerable sourceEnumerable && sourceValue is not string)
                {
                    var clonedEnumerable = CloneEnumerable(sourceEnumerable, cloneMode, cloneDepth, visited);
                    if (property.CanWrite)
                    {
                        TrySet(property, clone, clonedEnumerable);
                    }
                    else if (property.GetValue(clone) is IList targetList && clonedEnumerable is IEnumerable clonedItems)
                    {
                        targetList.Clear();
                        foreach (var item in clonedItems)
                        {
                            targetList.Add(item);
                        }
                    }
                    continue;
                }

                if (cloneDepth == CloneDepth.Shallow || cloneDepth == CloneDepth.PreserveReference)
                {
                    if (property.CanWrite)
                    {
                        TrySet(property, clone, sourceValue);
                    }
                    continue;
                }

                var clonedValue = CloneObject(sourceValue, cloneMode, cloneDepth, visited);
                if (property.CanWrite)
                {
                    TrySet(property, clone, clonedValue);
                }
            }

            ApplyIdentity(sourceObject, clone, cloneMode, cloneDepth);
            return clone;
        }

        private static object CloneEnumerable(
            IEnumerable sourceEnumerable,
            CloneMode cloneMode,
            CloneDepth cloneDepth,
            Dictionary<object, object> visited)
        {
            var sourceList = new List<object?>();
            foreach (var item in sourceEnumerable)
            {
                sourceList.Add(item);
            }

            var sourceType = sourceEnumerable.GetType();
            if (sourceType.IsArray)
            {
                var elementType = sourceType.GetElementType() ?? typeof(object);
                var array = Array.CreateInstance(elementType, sourceList.Count);
                for (int i = 0; i < sourceList.Count; i++)
                {
                    array.SetValue(CloneValue(sourceList[i], cloneMode, cloneDepth, visited), i);
                }
                return array;
            }

            if (cloneDepth == CloneDepth.Shallow || cloneDepth == CloneDepth.PreserveReference)
            {
                var shallowList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(
                    sourceType.IsGenericType ? sourceType.GetGenericArguments()[0] : typeof(object)))!;
                foreach (var item in sourceList)
                {
                    shallowList.Add(item);
                }
                return shallowList;
            }

            var listType = typeof(List<>).MakeGenericType(
                sourceType.IsGenericType ? sourceType.GetGenericArguments()[0] : typeof(object));
            var list = (IList)Activator.CreateInstance(listType)!;
            foreach (var item in sourceList)
            {
                list.Add(CloneValue(item, cloneMode, cloneDepth, visited));
            }
            return list;
        }

        private static object? CloneValue(
            object? sourceValue,
            CloneMode cloneMode,
            CloneDepth cloneDepth,
            Dictionary<object, object> visited)
        {
            if (sourceValue is null)
            {
                return null;
            }

            return IsSimple(sourceValue.GetType())
                ? sourceValue
                : CloneObject(sourceValue, cloneMode, cloneDepth, visited);
        }

        private static void ApplyIdentity(
            object sourceObject,
            object cloneObject,
            CloneMode cloneMode,
            CloneDepth cloneDepth)
        {
            if (sourceObject is not ISaveable sourceSaveable || cloneObject is not ISaveable cloneSaveable)
            {
                return;
            }

            var id = cloneMode == CloneMode.NewEntity
                ? Guid.NewGuid()
                : sourceSaveable.Id;

            if (cloneSaveable.Id != id)
            {
                SetId(cloneSaveable, id);
            }
        }

        private static void SetId(ISaveable target, Guid id)
        {
            var field = target.GetType().GetField(
                "<Id>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is not null)
            {
                field.SetValue(target, id);
                return;
            }

            var property = target.GetType().GetProperty(nameof(ISaveable.Id));
            if (property?.CanWrite == true)
            {
                property.SetValue(target, id);
            }
        }

        private static bool IsSimple(Type type)
        {
            return type.IsValueType
                || type.IsPrimitive
                || type.IsEnum
                || type == typeof(string)
                || type == typeof(Guid)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(TimeSpan);
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
