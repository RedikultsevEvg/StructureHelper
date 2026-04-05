using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class CylinderGeometryBlock : IAbaqusModelScriptBlock
    {
        private IKeywordBuilder builder;

        public string ModelVariableName { get; set; }
        public double Diameter { get; set; }
        public double Height { get; set; }
        public string PartName { get; set; } = "Part";
        public string PartVariableName { get; set; } = "part";
        public string ScetchName { get; set; } = "Scetch";
        public string ScetchVariableName { get; set; } = "scetch";

        public void Build(IAbaqusContext context)
        {
            var modelContext = context.Get<ModelContext>();
            builder = context.Builder;

            string lengthFactor = modelContext.LengthFactorName;

            builder.AddCommentedHeader("Geometry");
            builder.AddComment("Cylinder geometry, height of cylinder is placed along z-axis");
            builder.AddComment("Center of cylinder at point 0,0,0");
            builder.AddKeyword($"diameter = {FormatConverter.FormatDouble(Diameter)} * {lengthFactor}");
            builder.AddKeyword($"radius = {FormatConverter.FormatDouble(Diameter / 2.0)} * {lengthFactor}");
            builder.AddKeyword($"height = {FormatConverter.FormatDouble(Height)} * {lengthFactor}");
            builder.AddRaw(string.Empty);

            builder.AddKeyword($"{ScetchVariableName} = {ModelVariableName}.ConstrainedSketch(name='{ScetchName}', sheetSize= 2 * height)");
            builder.AddKeyword($"{ScetchVariableName}.CircleByCenterPerimeter(center=(0.0, 0.0), point1=(radius, 0.0));");
            builder.AddKeyword($"{PartVariableName} = model.Part(name='{PartName}', dimensionality=THREE_D, type=DEFORMABLE_BODY)");
            builder.AddKeyword($"{PartVariableName}.BaseSolidExtrude(sketch={ScetchVariableName}, depth=height)");
        }
    }
}
