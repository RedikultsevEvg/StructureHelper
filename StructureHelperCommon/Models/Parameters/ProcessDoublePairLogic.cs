using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace StructureHelperCommon.Models.Parameters
{
    public enum DigitPlace
    {
        Start,
        Any
    }
    public class ProcessDoublePairLogic : IProcessValuePairLogic<double>
    {
        const string digitalPattern = @"^[-]?[+]?\d*\.?\,?\d*";
        const string allowedPattern = @"[0-9]|\.|\,";
        const string characterPattern = "[a-z]+$";
        const string target = "";

        public DigitPlace DigitPlace { get; set; } = DigitPlace.Start;

        public ValuePair<double> GetValuePairByString(string s)
        {
            s = s.Replace(" ", string.Empty);
            
            Regex regexText = new (allowedPattern);
            string textString = regexText.Replace(s, target);
            var textMatch = Regex.Match(textString, characterPattern, RegexOptions.IgnoreCase);
            if (textMatch.Success == true)
            {
                textString = textMatch.Value.ToLower();
            }
            var digitalOnlyString = DigitPlace == DigitPlace.Start ? s : s.ToLower().Replace(textString, string.Empty);
            var match = Regex.Match(digitalOnlyString, digitalPattern);
            if (match.Success == true)
            {
                return GetDoubleValue(textString, match);
            }
            throw new StructureHelperException(ErrorStrings.DataIsInCorrect);
        }

        private static ValuePair<double> GetDoubleValue(string textString, Match match)
        {
            string digitalString = match.Value;
            if (digitalString != string.Empty || digitalString != "")
            {
                double digit = ProcessString.ConvertCommaToCultureSettings(digitalString);
                return new ValuePair<double>()
                {
                    Value = digit,
                    Text = textString
                };
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": value does not contain digital simbols");
            }
        }
    }
}
