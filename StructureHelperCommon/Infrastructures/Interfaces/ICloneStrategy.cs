namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Creates deep clone of object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneStrategy<T>
    {
        /// <summary>
        /// Returns deep clone of object
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        T GetClone(T sourceObject);
    }
}
