using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Non-generic bridge used when only a runtime <see cref="ISaveable"/> reference is available.
    /// </summary>
    //public interface ICloneStrategy
    //{
    //    object GetClone(object sourceObject, CloningMode cloningMode);
    //}

    /// <summary>
    /// Creates deep clone of object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneStrategy<T> where T : class// : ICloneStrategy
    {
        /// <summary>
        /// Returns deep clone of object using the requested cloning mode.
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="cloneMode"></param>
        /// <returns></returns>
        T GetClone(T sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep);
    }
}
