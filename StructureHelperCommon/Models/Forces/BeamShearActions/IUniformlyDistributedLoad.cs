namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Implement properties of 
    /// </summary>
    public interface IUniformlyDistributedLoad : IBeamShearLoad
    {
        /// <summary>
        /// Value of uniformly distributed load, N/m
        /// </summary>
        double LoadValue { get; set; }
        /// <summary>
        /// Coordinate of start of load, m
        /// </summary>
        double StartCoordinate { get; set; }
        /// <summary>
        /// Coordinate of end of load, m
        /// </summary>
        double EndCoordinate { get; set; }
    }
}