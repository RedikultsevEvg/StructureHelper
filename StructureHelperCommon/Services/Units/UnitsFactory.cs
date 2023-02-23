using System.Collections.Generic;
using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Services.Units
{
    public static class UnitsFactory
    {
        public static List<IUnit> GetUnitCollection()
        {
            List<IUnit> units = new List<IUnit>();
            UnitTypes type = UnitTypes.Length;
            units.Add(new Unit() { UnitType = type, Name = "m", Multiplyer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "mm", Multiplyer = 1e3d });
            units.Add(new Unit() { UnitType = type, Name = "cm", Multiplyer = 1e2d });
            units.Add(new Unit() { UnitType = type, Name = "km", Multiplyer = 1e-3d });
            type = UnitTypes.Area;
            units.Add(new Unit() { UnitType = type, Name = "m2", Multiplyer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "mm2", Multiplyer = 1e6d });
            units.Add(new Unit() { UnitType = type, Name = "cm2", Multiplyer = 1e4d });
            type = UnitTypes.Stress;
            units.Add(new Unit() { UnitType = type, Name = "Pa", Multiplyer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "kPa", Multiplyer = 1e-3d });
            units.Add(new Unit() { UnitType = type, Name = "MPa", Multiplyer = 1e-6d });
            type = UnitTypes.Force;
            units.Add(new Unit() { UnitType = type, Name = "N", Multiplyer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "kN", Multiplyer = 1e-3d });
            units.Add(new Unit() { UnitType = type, Name = "MN", Multiplyer = 1e-6d });
            type = UnitTypes.Moment;
            units.Add(new Unit() { UnitType = type, Name = "Nm", Multiplyer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "kNm", Multiplyer = 1e-3d });
            units.Add(new Unit() { UnitType = type, Name = "kgfm", Multiplyer = 9.8d });
            units.Add(new Unit() { UnitType = type, Name = "tfm", Multiplyer = 9.8e-3d });
            type = UnitTypes.Curvature;
            units.Add(new Unit() { UnitType = type, Name = "1/m", Multiplyer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "1/mm", Multiplyer = 1e-3d });
            units.Add(new Unit() { UnitType = type, Name = "1/cm", Multiplyer = 1e-2d });
            return units;
        }
    }
}
