using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Services.Units
{
    /// <summary>
    /// Interface for measurements Unit
    /// </summary>
    public interface IUnit
    {

        UnitTypes UnitType { get; }
        string Name { get; }
        double Multiplyer { get; }
    }
}
