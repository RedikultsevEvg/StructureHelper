using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;

namespace StructureHelper.Services.Points
{
    /// <summary>
    /// Converts point coordinates between the model unit (metres) and the unit used by the points editor (millimetres).
    /// </summary>
    public static class PointUnits
    {
        private static readonly IUnit millimeter = new GetUnitLogic().GetUnit(UnitTypes.Length, "mm");

        public static double ToMillimeters(double meters) => meters * millimeter.Multiplayer;

        public static double FromMillimeters(double millimeters) => millimeters / millimeter.Multiplayer;
    }
}
