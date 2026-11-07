using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Non-generic bridge used when only a runtime <see cref="ISaveable"/> reference is available.
    /// </summary>
    public interface ICloneStrategy
    {
        object GetClone(object sourceObject, CloningMode cloningMode);
    }

    /// <summary>
    /// Creates deep clone of object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneStrategy<T> : ICloneStrategy
    {
        /// <summary>
        /// Returns deep clone of object using new IDs.
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        T GetClone(T sourceObject) => GetClone(sourceObject, CloningMode.NewEntity);

        /// <summary>
        /// Returns deep clone of object using the requested cloning mode.
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="cloningMode"></param>
        /// <returns></returns>
        T GetClone(T sourceObject, CloningMode cloningMode)
            => (T)LegacyCloneHelper.Clone(sourceObject!, cloningMode);

        object ICloneStrategy.GetClone(object sourceObject, CloningMode cloningMode)
            => GetClone((T)sourceObject, cloningMode);
    }
}
