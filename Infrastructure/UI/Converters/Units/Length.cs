using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal class Length : UnitBase
    {
        private IEnumerable<IUnit> units = UnitsFactory.GetUnitCollection();
        private UnitTypes type = UnitTypes.Length;
        private IUnit currentUnit => units.Where(u => u.UnitType == type &  u.Name == "mm").Single();
        public override string MeasureUnit => currentUnit.Name;
        private double coeffficient => currentUnit.Multiplyer;

        public override string unitName { get => "Length"; }     

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val;
            if (value != null) { val = (double)value; }
            else { throw new Exception($"{unitName} value is null"); }
            val *= coeffficient;
            string strValue = $"{val} {MeasureUnit}";
            return strValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val;
            try
            {
                var strVal = value as string;
                IStringDoublePair pair = CommonOperation.DivideIntoStringDoublePair(strVal);
                double multy;
                try
                {
                    multy = coeffficient / GetMultiplyer(units, type, pair.Text);
                }
                catch (Exception ex)
                {
                    multy = 1d;
                }
                val = pair.Digit * multy;
            }
            catch
            {
                return null;
            }
            val /= coeffficient;
            return val;
        }

        private double GetMultiplyer(IEnumerable<IUnit> units, UnitTypes unitType, string unitName)
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
