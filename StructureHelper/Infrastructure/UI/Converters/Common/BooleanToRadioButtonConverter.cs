using System;
using System.Globalization;
using System.Windows.Data;

namespace StructureHelper.Infrastructure.UI.Converters.Common
{

    public class BooleanToRadioButtonConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is not bool current)
                return false;

            bool expected = System.Convert.ToBoolean(parameter, culture);
            return current == expected;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is not true)
                return Binding.DoNothing;

            return System.Convert.ToBoolean(parameter, culture);
        }
    }
}
