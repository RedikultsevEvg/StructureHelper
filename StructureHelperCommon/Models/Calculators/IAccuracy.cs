using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Calculators
{
    /// <summary>
    /// Rate of calculation which based on iteration of finished accuracy
    /// </summary>
    public interface IAccuracy : ISaveable
    {
        /// <summary>
        /// Max accuracy of iteration
        /// </summary>
        double IterationAccuracy { get; set; }
        /// <summary>
        /// Limit iteration count for calculation
        /// </summary>
        int MaxIterationCount { get; set; }
    }
}
