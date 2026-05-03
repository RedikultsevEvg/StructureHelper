namespace StructureHelperCommon.Models.Forces
{
    public interface IDistributedBeamSpanLoad : IBeamSpanLoad
    {
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