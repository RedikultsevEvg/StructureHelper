using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class BoundaryConditionBlock : IAbaqusModelScriptBlock
    {
        private IKeywordBuilder builder;

        public string ModelVariableName { get; set; }

        public BoundaryConditionBlock(string modelVariableName)
        {
            ModelVariableName = modelVariableName;
        }

        public string InitialStepName { get; set; }
        public string SecondStepName { get; set; }
        public double DisplacementX { get; set; } = 0.0;
        public double DisplacementY { get; set; } = 0.0;
        public double DisplacementZ { get; set; } = 0.0;

        public void Build(IAbaqusContext context)
        {
            var model = context.Get<ModelContext>();
            builder = context.Builder;

            builder.AddCommentedHeader("Boundary Conditions");
            builder.AddComment("Bottom fixed");
            builder.AddKeyword($"bottomFace = instance.faces.findAt(((0, 0, 0.0),))");
            builder.AddKeyword($"region = regionToolset.Region(faces=bottomFace)");
            builder.AddKeyword($"{ModelVariableName}.DisplacementBC(name='FixBottom',");
            builder.AddKeyword($"   createStepName='{InitialStepName}',");
            builder.AddKeyword($"   region=region,");
            builder.AddKeyword($"   u1=0.0, u2=0.0, u3=0.0,");
            builder.AddKeyword($"   ur1=0.0, ur2=0.0, ur3=0.0)");
            builder.AddRaw(string.Empty);
            builder.AddComment("Top displacement");
            builder.AddKeyword($"topFace = instance.faces.findAt(((0, 0, height),))");
            builder.AddKeyword($"region = regionToolset.Region(faces=topFace)");
            builder.AddKeyword($"assembly.Surface(name='TopSurface', side1Faces=topFace)");
            builder.AddComment("Coupling top face to reference point");
            builder.AddKeyword($"{ModelVariableName}.Coupling(name='TopCoupling', controlPoint=assembly.sets['LoadRP'], surface=assembly.surfaces['TopSurface'],");
            builder.AddKeyword($"influenceRadius=WHOLE_SURFACE, couplingType=KINEMATIC,");
            builder.AddKeyword($"u1=ON, u2=ON, u3=ON, ur1=ON, ur2=ON, ur3=ON)");
            builder.AddKeyword($"model.DisplacementBC(name='TopLoad', createStepName='{SecondStepName}', region=assembly.sets['LoadRP'],");
            builder.AddKeyword($"   u1={FormatConverter.FormatDouble(DisplacementX)} * lengthFactor,");
            builder.AddKeyword($"   u2={FormatConverter.FormatDouble(DisplacementY)} * lengthFactor,");
            builder.AddKeyword($"   u3={FormatConverter.FormatDouble(DisplacementZ)} * lengthFactor,");
            builder.AddKeyword($"   ur1=0,");
            builder.AddKeyword($"   ur2=0,");
            builder.AddKeyword($"   ur3=0)");
        }
    }
}
