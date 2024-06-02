using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal class Length : UnitBase
    {
        
        public override UnitTypes UnitType { get => UnitTypes.Length; }
        public override IUnit CurrentUnit { get => UnitLogic.GetUnit(UnitType, "mm"); }
        public override string UnitName { get => "Length"; }
    }
}
