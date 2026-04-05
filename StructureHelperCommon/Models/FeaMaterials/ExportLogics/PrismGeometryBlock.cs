using StructureHelperCommon.Models.ScriptExports;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class PrismGeometryBlock : IAbaqusScriptBlock
    {
        private IKeywordBuilder builder;

        public string ModelVariableName { get; set; }
        public string PartName { get; set; } = "Part";
        public string PartVariableName { get; set; } = "part";
        public string ScetchName { get; set; } = "Scetch";
        public string ScetchVariableName { get; set; } = "scetch";

        public double Width { get; set; } = 0.0;
        public double Depth { get; set; } = 0.0;
        public double Height { get; set; } = 0.0;

        public void Build(IAbaqusContext context)
        {
            var modelContext = context.Get<ModelContext>();
            builder = context.Builder;

            string lengthFactor = modelContext.LengthFactorName;

            builder.AddCommentedHeader("Geometry");
            builder.AddKeyword($"width = {FormatConverter.FormatDouble(Width)} * {lengthFactor}");
            builder.AddKeyword($"depth = {FormatConverter.FormatDouble(Depth)} * {lengthFactor}");
            builder.AddKeyword($"height = {FormatConverter.FormatDouble(Height)} * {lengthFactor}");
            builder.AddRaw(string.Empty);

            builder.AddKeyword($"{ScetchVariableName} = {ModelVariableName}.ConstrainedSketch(name='{ScetchName}', sheetSize= 2 * height)");
            builder.AddKeyword($"{ScetchVariableName}.rectangle(point1=(- width / 2, - depth / 2), point2=(width / 2, depth / 2))");
            builder.AddKeyword($"{PartVariableName} = model.Part(name='{PartName}', dimensionality=THREE_D, type=DEFORMABLE_BODY)");
            builder.AddKeyword($"{PartVariableName}.BaseSolidExtrude(sketch={ScetchVariableName}, depth=height)");
        }
    }
}
