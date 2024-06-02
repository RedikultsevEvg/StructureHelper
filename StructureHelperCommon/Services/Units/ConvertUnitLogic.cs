using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace StructureHelperCommon.Services.Units
{
    public class ConvertUnitLogic : IConvertUnitLogic
    {
        private static IEnumerable<IUnit> units = UnitsFactory.GetUnitCollection();
        private static IProcessValuePairLogic<double> pairLogic = new ProcessDoublePairLogic();

        public IMathRoundLogic MathRoundLogic { get; set; } = new DirectRoundLogic();

        public ValuePair<double> Convert(IUnit unit, string unitName, object value)
        {
            double val;
            if (value != null)
            {
                try
                {
                    val = (double)value;
                }
                catch (Exception ex)
                {
                    throw new StructureHelperException($"{ErrorStrings.IncorrectValue}");
                }
            }
            else
            {
                throw new StructureHelperException($"{ErrorStrings.ParameterIsNull}: {unitName}");
            }
            val *= unit.Multiplyer;
            var pair = new ValuePair<double>
            {
                Text = unit.Name,
                Value = val
            };
            return pair;
        }

        public double ConvertBack(UnitTypes unitType, IUnit unit, object value)
        {
            double val;
            double multy;
            double factor = unit.Multiplyer;
            var strVal = value as string;
            var pair = pairLogic.GetValuePairByString(strVal);
            try
            {
                multy = GetMultiplyer(unitType, pair.Text);
            }
            catch (Exception ex)
            {
                multy = factor;
            }
            val = pair.Value / multy;
            return val;
        }

        private double GetMultiplyer(UnitTypes unitType, string unitName)
        {
            try
            {
                return units
                    .Where(u =>
                    u.UnitType == unitType
                    &
                    u.Name == unitName)
                    .Single()
                    .Multiplyer;
            }
            catch (Exception ex)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ex);
            }
        }
    }
}
