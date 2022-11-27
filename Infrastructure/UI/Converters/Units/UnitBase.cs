using StructureHelperCommon.Infrastructures.Enums;
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
    internal abstract class UnitBase : IValueConverter
    {
        public abstract UnitTypes unitType { get; }
        public abstract IUnit currentUnit { get; }
        public abstract string unitName { get;}
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CommonOperation.Convert(currentUnit, unitName, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CommonOperation.ConvertBack(unitType, currentUnit, value);
        }
    }
}
