using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Reflection;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Transitional bridge for objects that expose a legacy parameterless Clone() method.
    /// It is used by the default <see cref="ICloneStrategy{T}"/> implementation while
    /// explicit clone strategies are introduced.
    /// </summary>
    public static class LegacyCloneHelper
    {
        private static readonly MethodInfo MemberwiseCloneMethod = typeof(object)
            .GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)!;

        public static object Clone(object sourceObject, CloningMode cloningMode)
        {
            if (sourceObject is null)
            {
                return null!;
            }

            if (cloningMode == CloningMode.Reference)
            {
                return sourceObject;
            }

            var method = sourceObject.GetType().GetMethod(
                "Clone",
                BindingFlags.Public | BindingFlags.Instance,
                binder: null,
                types: Type.EmptyTypes,
                modifiers: null);

            object clone;
            if (method is not null && method.ReturnType.IsAssignableTo(typeof(object)))
            {
                clone = method.Invoke(sourceObject, null)!;
            }
            else
            {
                clone = MemberwiseCloneMethod.Invoke(sourceObject, null)!;
            }

            if (ReferenceEquals(clone, sourceObject))
            {
                clone = MemberwiseCloneMethod.Invoke(sourceObject, null)!;
            }

            if (sourceObject is ISaveable sourceSaveable && clone is ISaveable cloneSaveable)
            {
                Guid id = cloningMode == CloningMode.NewEntity
                    ? Guid.NewGuid()
                    : sourceSaveable.Id;

                if (cloneSaveable.Id != id)
                {
                    SetId(cloneSaveable, id);
                }
            }

            return clone;
        }

        public static void SetId(ISaveable target, Guid id)
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
                return;
            }

            throw new StructureHelperException(
                $"{ErrorStrings.DataIsInCorrect}: cannot assign Id to {target.GetType().FullName}");
        }
    }
}
