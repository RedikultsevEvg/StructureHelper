using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class JobBlock : IAbaqusModelScriptBlock
    {
        private IKeywordBuilder builder;

        public string ModelVariableName { get; set; }
        public object JobName { get; set; }

        public JobBlock(string modelVariableName)
        {
            ModelVariableName = modelVariableName;
        }

        public void Build(IAbaqusContext context)
        {
            var model = context.Get<ModelContext>();
            builder = context.Builder;

            builder.AddCommentedHeader("Job");
            builder.AddComment("Change numCpus to your number of CPUs, i.e. 8, if you have 8 CPUs");
            builder.AddKeyword($"mdb.Job(name='{JobName}', model={ModelVariableName}.name, numCpus=1)");
        }
    }
}
