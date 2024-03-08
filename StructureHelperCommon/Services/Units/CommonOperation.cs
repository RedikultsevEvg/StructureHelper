using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace StructureHelperCommon.Services.Units
{
    public static class CommonOperation
    {
        private static IEnumerable<IUnit> units = UnitsFactory.GetUnitCollection();

        public static double ConvertToDoubleChangeComma(string s)
        {
            double result;
            if (!double.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                !double.TryParse(s, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                throw new StructureHelperException($"{ErrorStrings.IncorrectValue}: {s}");
            }
            return result;
        }

        public static IStringDoublePair DivideIntoStringDoublePair(string s)
        {
            s = s.Replace(" ", "");
            //string digitPattern = @"^[-]?[+]?\d+(\.?)|(\,?)\d*";
            string digitPattern = @"^[-]?[+]?\d*\.?\,?\d*";
            string textPattern = @"[0-9]|\.|\,";
            string caracterPattern = "[a-z]+$";
            string target = "";
            Regex regexText = new Regex(textPattern);
            string textString = regexText.Replace(s, target);
            var textMatch = Regex.Match(textString, caracterPattern, RegexOptions.IgnoreCase);
            if (textMatch.Success) {textString = textMatch.Value.ToLower();}
            var match = Regex.Match(s, digitPattern);
            if (match.Success)
            {
                string digitString = match.Value;
                double digit = ConvertToDoubleChangeComma(digitString);
                return new StringDoublePair() { Digit = digit, Text = textString };
            }
            throw new StructureHelperException(ErrorStrings.DataIsInCorrect);
        }

        public static IUnit GetUnit(UnitTypes unitType, string unitName = null)
        {
            if (unitName is null)
            {
                var boolResult = DefaultUnitNames.TryGetValue(unitType, out unitName);
                if (boolResult == false)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": unit type{unitType} is unknown");
                }
            }
            return units.Where(u => u.UnitType == unitType & u.Name == unitName).Single();
        }

        public static Dictionary<UnitTypes, string> DefaultUnitNames => new()
        {
            { UnitTypes.Length, "m"},
            { UnitTypes.Area, "m2"},
            { UnitTypes.Force, "kN" },
            { UnitTypes.Moment, "kNm"},
            { UnitTypes.Stress, "MPa"},
            { UnitTypes.Curvature, "1/m"},
        };


        public static string Convert(IUnit unit, string unitName, object value)
        {
            double val;
            if (value != null) { val = (double)value; }
            else { throw new Exception($"{unitName} value is null"); }
            val *= unit.Multiplyer;
            string strValue = $"{val} {unit.Name}";
            return strValue;
        }

        public static double ConvertBack(UnitTypes unitType, IUnit unit, object value)
        {
            double val;
            double multy;
            double coefficient = unit.Multiplyer;
            var strVal = value as string;
            IStringDoublePair pair = DivideIntoStringDoublePair(strVal);
            try
            {
                multy = GetMultiplyer(unitType, pair.Text);
            }
            catch (Exception ex)
            {
                multy = coefficient;
            }
            val = pair.Digit / multy;
            return val;
        }

        public static double GetMultiplyer(UnitTypes unitType, string unitName)
        {
            try
            {
                return units.Where(u => u.UnitType == unitType & u.Name == unitName).Single().Multiplyer;
            }
            catch (Exception ex)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ex);
            }
        }
    }
}
