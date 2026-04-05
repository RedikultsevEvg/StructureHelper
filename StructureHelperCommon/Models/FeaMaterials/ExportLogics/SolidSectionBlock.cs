using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials.ExportLogics
{
    public class SolidSectionBlock : IAbaqusScriptBlock
    {
        private IKeywordBuilder builder;
        private string modelVariableName;

        public string MaterialVariableName { get;set;  } = "NotAssigned";
        public string PartVariableName { get;set;  } = "NotAssigned";
        
        public string SectionVariableName { get;set;  } = "solidSection";
        public string SectionName { get;set;  } = "SolidSection";
        public void Build(IAbaqusContext context)
        {
            var model = context.Get<ModelContext>();
            builder = context.Builder;

            modelVariableName = model.ModelVaribleName;

            builder.AddCommentedHeader("Section");
            builder.AddKeyword($"{modelVariableName}.HomogeneousSolidSection(name='{SectionName}',material={MaterialVariableName}.name, thickness=None)");
            builder.AddKeyword($"cells = part.cells");
            builder.AddKeyword($"region = regionToolset.Region(cells=cells)");
            builder.AddKeyword($"{PartVariableName}.SectionAssignment(region=region, sectionName='{SectionName}')");
        }
    }
}
