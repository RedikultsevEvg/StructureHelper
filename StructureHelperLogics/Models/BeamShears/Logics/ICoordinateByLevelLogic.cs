using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implements logic of calculating of coordinate of point where source level intersect inclinid section
    /// </summary>
    public interface ICoordinateByLevelLogic : ILogic
    {
        /// <summary>
        /// Calculates coordinate of point where aource level intersect inclined section
        /// </summary>
        /// <param name="startCoord">Start coordinate of inclined section</param>
        /// <param name="endCoord">End coordinate of inclined section</param>
        /// <param name="relativeLevel">Source relative level, 0.5 - top level, -0.5 bottom level</param>
        /// <returns></returns>
        double GetCoordinate(double startCoord, double endCoord, double relativeLevel);
    }
}
