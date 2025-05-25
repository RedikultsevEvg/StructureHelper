namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Interface for cloning objects
    /// </summary>
    public interface ICloningStrategy
    {
        /// <summary>
        /// Returns copy of object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="original">Source object</param>
        /// <param name="cloneStrategy">Strategy for cloning of object of specified type</param>
        /// <returns></returns>
        T Clone<T>(T original, ICloneStrategy<T>? cloneStrategy = null) where T : class;
    }
}
