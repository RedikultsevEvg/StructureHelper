using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class StepBlock : IAbaqusModelScriptBlock
    {
        private IKeywordBuilder builder;

        public string ModelVariableName { get; set; }
        public string InitialStepName { get; set; } = "Initial";
        public string SecondStepName { get; set; } = "NotAssigned";

        public void Build(IAbaqusContext context)
        {
            var model = context.Get<ModelContext>();
            builder = context.Builder;

            builder.AddCommentedHeader("Step");
            builder.AddKeyword($"{ModelVariableName}.StaticStep(name='{SecondStepName}',previous='{InitialStepName}', nlgeom=ON)");
            builder.AddKeyword($"{ModelVariableName}.steps['{SecondStepName}'].setValues(maxNumInc=10000, initialInc=0.01, maxInc=0.01)");
        }
    }
}
