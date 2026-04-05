using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public abstract class AbaqusMaterialPythonScriptBuilder : IScriptBuilder
    {
       
        public KeywordBuilder Builder { get; } = new();
        public abstract string Build(IFeaMaterial material);
        public string ModelName { get; set; } = "model";
        public string ShortMaterialName { get; set; }
        public string MaterialVariableName { get; set; }
        public string MaterialName { get; set; }

        public bool AddCommentForAbaqusModel { get; set; } = true;
        public void AddReferenceToModel()
        {
            Builder.AddComment($"Uncomment next line if you need");
            Builder.AddComment($"model = mdb.models['Model-1']");
            Builder.AddKeyword(string.Empty);
        }

        public string FormatDouble(double value)
        {
            var formatted = Convert.ToString(value, CultureInfo.InvariantCulture);
            return formatted;
        }
    }
}
