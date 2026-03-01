using System.Collections.Generic;
using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Services.Units
{
    public static class UnitsFactory
    {
        /// <summary>
        /// Returns collection of unit
        /// </summary>
        /// <returns></returns>
        public static List<IUnit> GetUnitCollection()
        {
            List<IUnit> units = new List<IUnit>();
            UnitTypes type = UnitTypes.Length;
            units.Add(new Unit() { UnitType = type, Name = "m", Multiplayer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "mm", Multiplayer = 1e3d });
            units.Add(new Unit() { UnitType = type, Name = "cm", Multiplayer = 1e2d });
            units.Add(new Unit() { UnitType = type, Name = "km", Multiplayer = 1e-3d });
            type = UnitTypes.Area;
            units.Add(new Unit() { UnitType = type, Name = "m2", Multiplayer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "mm2", Multiplayer = 1e6d });
            units.Add(new Unit() { UnitType = type, Name = "cm2", Multiplayer = 1e4d });
            type = UnitTypes.Stress;
            units.Add(new Unit() { UnitType = type, Name = "Pa", Multiplayer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "kPa", Multiplayer = 1e-3d });
            units.Add(new Unit() { UnitType = type, Name = "MPa", Multiplayer = 1e-6d });
            type = UnitTypes.Force;
            units.Add(new Unit() { UnitType = type, Name = "N", Multiplayer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "kN", Multiplayer = 1e-3d });
            units.Add(new Unit() { UnitType = type, Name = "MN", Multiplayer = 1e-6d });
            type = UnitTypes.Moment;
            units.Add(new Unit() { UnitType = type, Name = "Nm", Multiplayer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "kNm", Multiplayer = 1e-3d });
            units.Add(new Unit() { UnitType = type, Name = "kgfm", Multiplayer = 9.81d });
            units.Add(new Unit() { UnitType = type, Name = "tfm", Multiplayer = 9.81e-3d });
            type = UnitTypes.Curvature;
            units.Add(new Unit() { UnitType = type, Name = "1/m", Multiplayer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "1/mm", Multiplayer = 1e-3d });
            units.Add(new Unit() { UnitType = type, Name = "1/cm", Multiplayer = 1e-2d });
            type = UnitTypes.DistributedLoad;
            units.Add(new Unit() { UnitType = type, Name = "N/m", Multiplayer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "kN/m", Multiplayer = 1e-3d });
            units.Add(new Unit() { UnitType = type, Name = "MN/m", Multiplayer = 1e-6d });
            type = UnitTypes.Strain;
            units.Add(new Unit() { UnitType = type, Name = "Dimensionless", Multiplayer = 1d });
            type = UnitTypes.FractureEnergy;
            units.Add(new Unit() { UnitType = type, Name = "N/m", Multiplayer = 1.0 });
            units.Add(new Unit() { UnitType = type, Name = "N/mm", Multiplayer = 1.0e-3 });
            return units;
        }
    }
}
