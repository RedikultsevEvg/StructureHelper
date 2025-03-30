namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Implement properties of 
    /// </summary>
    public interface IDistributedLoad : IBeamSpanLoad
    {
        /// <summary>
        /// Value of uniformly distributed load, N/m
        /// </summary>
        IForceTuple LoadValue { get; set; }
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