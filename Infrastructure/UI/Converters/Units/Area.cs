using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal class Area : UnitBase
    {
        public override string unitName { get => "Area"; }

        public override string MeasureUnit => throw new NotImplementedException();

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val;
            if (value != null) { val = (double)value; }
            else { throw new Exception($"{unitName} value is null"); }
            val *= UnitConstatnts.Length * UnitConstatnts.Length;
            return val;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val;
            try
            {
                var strVal = value as string;
                val = CommonOperation.ConvertToDoubleChangeComma(strVal);
            }
            catch
            {
                return null;
            }
            val /= (UnitConstatnts.Length * UnitConstatnts.Length);
            return val;
        }
    }
}
