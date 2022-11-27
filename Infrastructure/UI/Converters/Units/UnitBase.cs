using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace StructureHelper.Infrastructure.UI.Converters.Units
{
    internal abstract class UnitBase : IValueConverter
    {
        public abstract UnitTypes UnitType { get; }
        public abstract IUnit CurrentUnit { get; }
        public abstract string UnitName { get;}
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CommonOperation.Convert(CurrentUnit, UnitName, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return CommonOperation.ConvertBack(UnitType, CurrentUnit, value);
            }
            catch (Exception)
            {
                MessageBox.Show($"Value of {UnitName}={(string)value} is not correct", "Error of conversion", MessageBoxButtons.OK);
                return 0;
            }
            
        }
    }
}
