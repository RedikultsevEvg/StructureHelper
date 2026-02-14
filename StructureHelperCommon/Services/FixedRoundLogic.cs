using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services
{
    public class FixedRoundLogic : IDigitRoundLogic
    {
        public int DigitQuant { get; set; }

        /// <summary>
        /// Oкруление до нужного числа цифр, например (12345, 3) дает результат 12345,678, например (0.12345, 3) дает результат 0,123
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quant"></param>
        /// <returns></returns>
        public double RoundValue(double value)
        {
            double roundedValue = Math.Round(value, DigitQuant);
            return roundedValue;
        }
    }
}
