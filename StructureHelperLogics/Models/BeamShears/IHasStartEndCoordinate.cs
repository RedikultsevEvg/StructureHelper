namespace StructureHelperLogics.Models.BeamShears
{
    public interface IHasStartEndCoordinate
    {
        /// <summary>
        /// Coordinate of start of zone of stirrups
        /// </summary>
        double StartCoordinate { get; set; }
        /// <summary>
        /// Coordinate of end of zone of stirrups
        /// </summary>
        double EndCoordinate { get; set; }
    }
}