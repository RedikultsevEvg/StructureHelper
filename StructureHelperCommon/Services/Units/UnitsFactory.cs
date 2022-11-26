using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            type = UnitTypes.Stress;
            units.Add(new Unit() { UnitType = type, Name = "Pa", Multiplyer = 1d });
            units.Add(new Unit() { UnitType = type, Name = "kPa", Multiplyer = 1e-3d });
            units.Add(new Unit() { UnitType = type, Name = "MPa", Multiplyer = 1e-6d });
            return units;
        }
    }
}
