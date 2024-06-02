using StructureHelperCommon.Infrastructures.Exceptions;
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
    internal class PlainDouble : IValueConverter
    {
        IConvertUnitLogic operationLogic = new ConvertUnitLogic();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (double)value;
            }
            catch(Exception)
            {
                return new StructureHelperException(ErrorStrings.DataIsInCorrect);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ProcessString.ConvertCommaToCultureSettings((string)value);
            }
            catch (Exception)
            {
                return new StructureHelperException(ErrorStrings.DataIsInCorrect);
            }
        }
    }
}
