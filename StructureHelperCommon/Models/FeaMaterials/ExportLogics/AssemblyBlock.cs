using StructureHelperCommon.Models.ScriptExports;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class AssemblyBlock : IAbaqusScriptBlock
    {
        private IKeywordBuilder builder;

        public string ModelVariableName { get; set; }
        public string PartVariableName { get; set; }
        public void Build(IAbaqusContext context)
        {
            var model = context.Get<ModelContext>();
            builder = context.Builder;

            builder.AddCommentedHeader("Assembly");
            builder.AddKeyword($"assembly = {ModelVariableName}.rootAssembly");
            builder.AddKeyword($"rp = assembly.ReferencePoint(point=(0, 0, height))");
            builder.AddKeyword($"rpRegion = regionToolset.Region(referencePoints=(assembly.referencePoints[rp.id],))");
            builder.AddKeyword($"assembly.Set(name='LoadRP',referencePoints=(assembly.referencePoints[rp.id],))");
            builder.AddKeyword($"instance = assembly.Instance(name='PrismInstance',part={PartVariableName}, dependent=ON)");
        }
    }
}
