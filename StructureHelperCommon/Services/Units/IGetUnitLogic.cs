using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Services.Units
{
    public interface IGetUnitLogic
    {
        IUnit GetUnit(UnitTypes unitType, string unitName = null);
    }
}