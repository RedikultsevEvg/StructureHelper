using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Services.Units
{
    public class Unit : IUnit
    {
        public UnitTypes UnitType { get; set; }
        public string Name { get; set; }
        public double Multiplyer { get; set; }
    }
}
