using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services
{
    public class SmartRoundLogic : IDigitRoundLogic
    {
        public int DigitQuant { get; set; } = 3;

        /// <summary>
        /// Умное окруление до нужного числа значащих цифр, например (12345, 3) дает результат 12300, например (0.12345, 3) дает результат 0,123
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quant"></param>
        /// <returns></returns>
        public double RoundValue(double value)
        {
            if (value == 0d) return 0d;
            double valueOrder = Math.Log10(Math.Abs(value));
            int order = Convert.ToInt32(Math.Ceiling(valueOrder));
            double requiredOrder = Math.Pow(10, DigitQuant - order);
            double roundedAbsValue = Math.Round(Math.Abs(value) * requiredOrder) / requiredOrder;
            double roundedValue = Math.Sign(value) * roundedAbsValue;
            return roundedValue;
        }
    }
}
