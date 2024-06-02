using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Parameters;
using System.Collections.Generic;

namespace StructureHelperCommon.Services.Units
{
    public interface IConvertUnitLogic
    {
        IMathRoundLogic MathRoundLogic { get; set; }
        ValuePair<double> Convert(IUnit unit, string unitName, object value);
        double ConvertBack(UnitTypes unitType, IUnit unit, object value);
    }
}