using StructureHelperCommon.Models.ScriptExports;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class MeshBlock : IAbaqusModelScriptBlock
    {
        private IKeywordBuilder builder;

        public string ModelVariableName { get; set; }
        public double MeshSize { get; set; }

        public void Build(IAbaqusContext context)
        {

            var model = context.Get<ModelContext>();
            builder = context.Builder;

            builder.AddCommentedHeader("Mesh");
            builder.AddKeyword($"part.seedPart(size={FormatConverter.FormatDouble(MeshSize)} * lengthFactor)");
            builder.AddKeyword($"part.setElementType(regions=(cells,), elemTypes=(mesh.ElemType(elemCode=C3D8R, elemLibrary=STANDARD),))");
            builder.AddKeyword($"part.generateMesh()");
            builder.AddKeyword($"assembly.regenerate()");
        }
    }
}
