using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services;
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
        IMathRoundLogic roundLogic = new DirectRoundLogic();
        public IConvertUnitLogic OperationLogic { get; set; } = new ConvertUnitLogic();
        public IGetUnitLogic UnitLogic { get; set; } = new GetUnitLogic();
        public abstract UnitTypes UnitType { get; }
        public abstract IUnit CurrentUnit { get; }
        public abstract string UnitName { get;}
        /// <summary>
        /// From variable to user
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pair = OperationLogic.Convert(CurrentUnit, UnitName, value);
            var result = pair.Value; 
            if (parameter is not null)
            {
                if (parameter is string paramString)
                {
                    var logic = new ProcessDoublePairLogic() { DigitPlace = DigitPlace.Any };
                    var paramPair = logic.GetValuePairByString(paramString);
                    string paramTextPart = paramPair.Text.ToLower();
                    int paramValuePart = (int)paramPair.Value;
                    if (paramTextPart == "smart")
                    {
                        roundLogic = new SmartRoundLogic() { DigitQuant = paramValuePart };
                    }
                    else if (paramTextPart == "fixed")
                    {
                        roundLogic = new FixedRoundLogic() { DigitQuant = paramValuePart };
                    }
                    result = roundLogic.RoundValue(result);
                }
            }
            string strValue = $"{result} {pair.Text}";
            return strValue;
        }
        /// <summary>
        /// From user to variable
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double result = OperationLogic.ConvertBack(UnitType, CurrentUnit, value);

                return result;
            }
            catch (Exception)
            {
                MessageBox.Show($"Value of {UnitName}={(string)value} is not correct", "Error of conversion", MessageBoxButtons.OK);
                return 0;
            }
            
        }
    }
}
