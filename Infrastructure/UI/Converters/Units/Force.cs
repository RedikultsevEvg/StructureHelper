using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal class Force : UnitBase
    {
        private double coeffficient = UnitConstatnts.Force;

        public override string unitName { get => "Force"; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val;
            if (value != null) { val = (double)value; }
            else { throw new Exception($"{unitName} value is null"); }
            val *= coeffficient;
            return val;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val;
            if (value != null)
            {
                var strVal = value as string;
                double.TryParse(strVal, out val);
            }
            else { throw new Exception($"{unitName} value is null"); }
            val /= coeffficient;
            return val;
        }
    }
}
