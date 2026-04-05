using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FieldBlock : IAbaqusModelScriptBlock
    {
        private IKeywordBuilder builder;

        public string ModelVariableName { get; set; }

        public FieldBlock(string modelVariableName)
        {
            ModelVariableName = modelVariableName;
        }

        public void Build(IAbaqusContext context)
        {
            var model = context.Get<ModelContext>();
            builder = context.Builder;

            builder.AddCommentedHeader("Field Output");
            builder.AddKeyword($"{ModelVariableName}.fieldOutputRequests['F-Output-1'].setValues(variables=(");
            builder.AddKeyword($"'S', 'U', 'E', 'PE', 'PEEQ','DAMAGET', 'DAMAGEC', 'STATUS'))");
            builder.AddCommentedHeader("History Output");
            builder.AddKeyword($"regionDef={ModelVariableName}.rootAssembly.sets['LoadRP']");
            builder.AddKeyword($"{ModelVariableName}.historyOutputRequests['H-Output-1'].setValues(variables=(");
            builder.AddKeyword($"'U1', 'U2', 'U3', 'RF1', 'RF2', 'RF3', 'TF1', 'TF2', 'TF3'),");
            builder.AddKeyword($"timeInterval=0.01, timeMarks=OFF, region=regionDef, sectionPoints=DEFAULT, rebar=EXCLUDE)");
            //builder.AddKeyword($"frequency=10, region=rpRegion, sectionPoints=DEFAULT, rebar=EXCLUDE)");
        }
    }
}
