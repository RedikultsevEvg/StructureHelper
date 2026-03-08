using System.Linq;

namespace StructureHelperCommon.Models.ScriptExports
{
    public interface IKeywordBuilder
    {
        void AddData(params double[] values);
        void AddKeyword(string keyword);
        void AddKeyword(string keyword, string parameterName, string parameterValue);
        public void AddParenthesisData(params object[] values);
        public void AddRaw(string text);
        string ToString();
    }
}