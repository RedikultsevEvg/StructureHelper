using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace StructureHelperCommon.Models.ScriptExports
{
    public class KeywordBuilder : IKeywordBuilder
    {
        const string commentString = "#";
        const string commentLongString = "----------------------------------------";
        private readonly StringBuilder _sb = new();

        public void AddKeyword(string keyword)
        {
            _sb.AppendLine(keyword);
        }

        public void AddKeyword(string keyword, string parameterName, string parameterValue)
        {
            _sb.AppendLine($"{keyword}, {parameterName}={parameterValue}");
        }

        public void AddCommentedHeader(string header)
        {
            _sb.AppendLine($"{commentString}{commentLongString}");
            _sb.AppendLine($"{commentString} {header} ");
            _sb.AppendLine($"{commentString}{commentLongString}");
        }

        public void AddComment(string comment)
        {
            _sb.AppendLine($"{commentString} {comment} ");
        }

        public void AddData(params double[] values)
        {
            var formattedValues = values
                .Select(v => v.ToString("G", CultureInfo.InvariantCulture));

            _sb.AppendLine(string.Join(", ", formattedValues));
        }

        public void AddParenthesisData(params object[] values)
        {
            var formatted = values
                .Select(v => Convert.ToString(v, CultureInfo.InvariantCulture));

            _sb.AppendLine($"({string.Join(", ", formatted)},)");
        }

        public void AddRaw(string text)
        {
            _sb.AppendLine(text);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
