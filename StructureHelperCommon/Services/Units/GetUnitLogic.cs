using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Units
{
    public class GetUnitLogic : IGetUnitLogic
    {
        private static IEnumerable<IUnit> units = UnitsFactory.GetUnitCollection();
        private Dictionary<UnitTypes, string> defaultUnitNames;


        public GetUnitLogic()
        {
            defaultUnitNames = new()
            {
                { UnitTypes.Length, "m"},
                { UnitTypes.Area, "m2"},
                { UnitTypes.Force, "kN" },
                { UnitTypes.Moment, "kNm"},
                { UnitTypes.Stress, "MPa"},
                { UnitTypes.Curvature, "1/m"},
            };
        }

        public IUnit GetUnit(UnitTypes unitType, string unitName = null)
        {
            if (unitName is null)
            {
                var boolResult = defaultUnitNames.TryGetValue(unitType, out unitName);
                if (boolResult == false)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": unit type{unitType} is unknown");
                }
            }
            return units
                .Where(u =>
                u.UnitType == unitType
                &
                u.Name == unitName)
                .Single();
        }


    }
}
