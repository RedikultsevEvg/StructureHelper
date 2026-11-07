using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Temporary adapter for legacy call sites while parameterless Clone() methods are removed.
    /// </summary>
    public static class CloneLegacyExtensions
    {
        /// <summary>
        /// Transitional extension used by legacy call sites until they are migrated to
        /// <see cref="ICloneStrategy{T}.GetClone(T, CloningMode)"/>.
        /// </summary>
        public static T Clone<T>(this T sourceObject)
            where T : class
        {
            return CloneLegacy(sourceObject);
        }

        public static T CloneLegacy<T>(this T sourceObject, CloningMode cloningMode = CloningMode.NewEntity)
            where T : class
        {
            return (T)LegacyCloneHelper.Clone(sourceObject, cloningMode);
        }
    }
}
