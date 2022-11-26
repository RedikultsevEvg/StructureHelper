using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.Converters
{
    internal static class CommonOperation
    {
        public static double ConvertToDoubleChangeComma(string s)
        {
            double result;
            if (!double.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                !double.TryParse(s, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                throw new StructureHelperException($"{ErrorStrings.IncorrectValue}: {s}");
            }
            return result;
        }

        public static IStringDoublePair DivideIntoStringDoublePair(string s)
        {
            string digitPattern = @"\d+(\.?|\,?)\d+";
            string textPattern = @"[0-9]|\.|\,";
            string caracterPattern = "[a-z]+$";
            string target = "";
            Regex regexText = new Regex(textPattern);
            string textString = regexText.Replace(s, target);
            var textMatch = Regex.Match(textString, caracterPattern, RegexOptions.IgnoreCase);
            if (textMatch.Success) {textString = textMatch.Value.ToLower();}
            var match = Regex.Match(s, digitPattern);
            if (match.Success)
            {
                string digitString = match.Value;
                double digit = ConvertToDoubleChangeComma(digitString);
                return new StringDoublePair() { Digit = digit, Text = textString };
            }
            throw new StructureHelperException(ErrorStrings.DataIsInCorrect);
        }
    }
}
