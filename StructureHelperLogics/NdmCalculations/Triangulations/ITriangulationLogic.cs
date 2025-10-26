using LoaderCalculator.Data.Ndms;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <summary>
    /// Implements logic of obtaining of collection of ndm parts 
    /// </summary>
    public interface ITriangulationLogic
    {
        /// <summary>
        /// Returns collection of ndm parts
        /// </summary>
        /// <returns></returns>
        IEnumerable<INdm> GetNdmCollection();
        /// <summary>
        /// Check options of triangulation
        /// </summary>
        /// <param name="options"></param>
        void ValidateOptions(ITriangulationLogicOptions options);
    }
}
