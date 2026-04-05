using StructureHelperCommon.Models.FeaMaterials.ExportLogics;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class PrismTestBlock : IAbaqusScriptBlock
    {
        const string sectionName = "PrismSection";
        const string initialStepName = "Initial";
        const string secondStepName = "Compression";

        private IKeywordBuilder builder;
        private string modelVariableName;
        private string forceFactor;
        private string lengthFactor;
        private string stressFactor;
        private string materialVariableName;

        public string MaterialVariableName { get; set;  } = string.Empty;
        public string SectionlVariableName { get; set;  } = string.Empty;

        public double Height { get; set; } = 0.3;
        public double Width { get; set; } = 0.1;
        public double Depth { get; set; } = 0.1;

        public double DisplacementX { get; set; } = 0.0;
        public double DisplacementY { get; set; } = 0.0;
        public double DisplacementZ { get; set; } = -0.001;

        public void Build(IAbaqusContext context)
        {
            var model = context.Get<ModelContext>();
            builder = context.Builder;

            modelVariableName = model.ModelVaribleName;
            forceFactor = model.ForceFactorName;
            lengthFactor = model.LengthFactorName;
            stressFactor = model.StressFactorName;

            AddHeader();
            //AddModel();
            AddGeometry();
            //AddMaterial();
            //AddSection();
            AddAssembly();
            AddStep();
            AddBoundaryConditions();
            //AddMesh();
            AddField();
            AddJob();
            builder.AddKeyword($"print(\"Model created successfully.\")");
        }

        private void AddJob()
        {
            builder.AddCommentedHeader("Job");
            builder.AddComment("Change numCpus to your number of CPUs, i.e. 8, if you have 8 CPUs");
            builder.AddKeyword($"mdb.Job(name='ConcreteCompression', model=modelName, numCpus=1)");

        }

        private void AddField()
        {
            builder.AddCommentedHeader("Field Output");
            builder.AddKeyword($"model.fieldOutputRequests['F-Output-1'].setValues(variables=(");
            builder.AddKeyword($"'S', 'U', 'E', 'PE', 'PEEQ','DAMAGET', 'DAMAGEC', 'STATUS'))");
            builder.AddCommentedHeader("History Output");
            builder.AddKeyword($"model.historyOutputRequests['H-Output-1'].setValues(variables=(");
            builder.AddKeyword($"'U1', 'U2', 'U3', 'RF1', 'RF2', 'RF3', 'TF1', 'TF2', 'TF3'),");
            builder.AddKeyword($"frequency=10, region=rpRegion, sectionPoints=DEFAULT, rebar=EXCLUDE)");
        }

        private void AddMesh(IFeaMaterial material)
        {
            builder.AddCommentedHeader("Mesh");
            double meshSize = 0.01;
            if (material is IConcreteFeaMaterial concreteFeaMaterial)
            {
                meshSize = concreteFeaMaterial.TensionProperties.FeSize;
            }
            builder.AddKeyword($"part.seedPart(size={FormatConverter.FormatDouble(meshSize)} * lengthFactor)");
            builder.AddKeyword($"part.setElementType(regions=(cells,), elemTypes=(mesh.ElemType(elemCode=C3D8R, elemLibrary=STANDARD),))");
            builder.AddKeyword($"part.generateMesh()");
            builder.AddKeyword($"assembly.regenerate()");
        }

        private void AddBoundaryConditions()
        {
            builder.AddCommentedHeader("Boundary Conditions");
            builder.AddComment("Bottom fixed");
            builder.AddKeyword($"bottomFace = instance.faces.findAt(((0, 0, 0.0),))");
            builder.AddKeyword($"region = regionToolset.Region(faces=bottomFace)");
            builder.AddKeyword($"model.DisplacementBC(name='FixBottom',");
            builder.AddKeyword($"   createStepName='{initialStepName}',");
            builder.AddKeyword($"   region=region,");
            builder.AddKeyword($"   u1=0.0, u2=0.0, u3=0.0,");
            builder.AddKeyword($"   ur1=0.0, ur2=0.0, ur3=0.0)");
            builder.AddComment(string.Empty);
            builder.AddComment("Top displacement");
            builder.AddKeyword($"topFace = instance.faces.findAt(((0, 0, height),))");
            builder.AddKeyword($"region = regionToolset.Region(faces=topFace)");
            builder.AddKeyword($"assembly.Surface(name='TopSurface', side1Faces=topFace)");
            builder.AddComment("Coupling top face to reference point");
            builder.AddKeyword($"model.Coupling(name='TopCoupling', controlPoint=assembly.sets['LoadRP'], surface=assembly.surfaces['TopSurface'],");
            builder.AddKeyword($"influenceRadius=WHOLE_SURFACE, couplingType=KINEMATIC,");
            builder.AddKeyword($"u1=ON, u2=ON, u3=ON, ur1=ON, ur2=ON, ur3=ON)");
            //Builder.AddKeyword($"model.DisplacementBC(name='TopLoad',");
            //Builder.AddKeyword($"   createStepName='{secondStepName}',");
            //Builder.AddKeyword($"   region=region,");
            //Builder.AddKeyword($"   u1={FormatDouble(DisplacementX * lengthFactor)},");
            //Builder.AddKeyword($"   u2={FormatDouble(DisplacementY * lengthFactor)},");
            //Builder.AddKeyword($"   u3={FormatDouble(DisplacementZ * lengthFactor)},");
            //Builder.AddKeyword($"   amplitude=UNSET)");
            builder.AddKeyword($"model.DisplacementBC(name='TopLoad', createStepName='Compression', region=assembly.sets['LoadRP'],");
            builder.AddKeyword($"   u1={FormatConverter.FormatDouble(DisplacementX)} * lengthFactor,");
            builder.AddKeyword($"   u2={FormatConverter.FormatDouble(DisplacementY)} * lengthFactor,");
            builder.AddKeyword($"   u3={FormatConverter.FormatDouble(DisplacementZ)} * lengthFactor,");
            builder.AddKeyword($"   ur1=0,");
            builder.AddKeyword($"   ur2=0,");
            builder.AddKeyword($"   ur3=0)");
        }

        private void AddStep()
        {
            builder.AddCommentedHeader("Step");
            builder.AddKeyword($"model.StaticStep(name='{secondStepName}',previous='{initialStepName}', nlgeom=ON)");
            builder.AddKeyword($"mdb.models['{modelVariableName}'].steps['{secondStepName}'].setValues(maxNumInc=100, initialInc=0.05, maxInc=0.05)");
        }

        private void AddAssembly()
        {
            builder.AddCommentedHeader("Assembly");
            builder.AddKeyword($"assembly = model.rootAssembly");
            builder.AddKeyword($"rp = assembly.ReferencePoint(point=(0, 0, height))");
            builder.AddKeyword($"rpRegion = regionToolset.Region(referencePoints=(assembly.referencePoints[rp.id],))");
            builder.AddKeyword($"assembly.Set(name='LoadRP',referencePoints=(assembly.referencePoints[rp.id],))");
            builder.AddKeyword($"instance = assembly.Instance(name='PrismInstance',part=part, dependent=ON)");
        }

        //private void AddSection()
        //{
        //    var sectionBlockBuilder = new SolidSectionBlock()
        //    {
        //        MaterialVariableName = materialVariableName,
        //        SectionVariableName = "PrismSection"
        //    };
        //    var sectionBlock = 
        //    builder.AddCommentedHeader("Section");
        //    builder.AddKeyword($"model.HomogeneousSolidSection(name={sectionVariableName},material={materialVariableName}, thickness=None)");
        //    builder.AddKeyword($"cells = part.cells");
        //    builder.AddKeyword($"region = regionToolset.Region(cells=cells)");
        //    builder.AddKeyword($"part.SectionAssignment(region=region, sectionName='{sectionName}')");

        //}

        //private void AddMaterial(IFeaMaterial material)
        //{
        //    this.builder.AddCommentedHeader("Material");
        //    var builder = new FeaMaterialPyBuilder();
        //    var script = builder.Build(material);
        //    scriptMaterialName = builder.ScriptMaterialName;
        //    materialName = builder.MaterialName;
        //    this.builder.AddRaw(script);
        //}

        private void AddGeometry()
        {
            builder.AddCommentedHeader("Geometry");
            builder.AddKeyword($"width = {FormatConverter.FormatDouble(Width)} * lengthFactor");
            builder.AddKeyword($"depth = {FormatConverter.FormatDouble(Depth)} * lengthFactor");
            builder.AddKeyword($"height = {FormatConverter.FormatDouble(Height)} * lengthFactor");
            builder.AddRaw(string.Empty);

            builder.AddKeyword($"sketch = model.ConstrainedSketch(name='profile', sheetSize= 2 * height)");
            builder.AddKeyword($"sketch.rectangle(point1=(- width / 2, - depth / 2), point2=(width / 2, depth / 2))");
            builder.AddKeyword($"part = model.Part(name='Prism', dimensionality=THREE_D, type=DEFORMABLE_BODY)");
            builder.AddKeyword($"part.BaseSolidExtrude(sketch=sketch, depth=height)");
        }

        //private void AddModel()
        //{
        //    builder.AddCommentedHeader("Model");
        //    builder.AddKeyword($"modelName = '{modelName}'");
        //    builder.AddKeyword($"if modelName in mdb.models:");
        //    builder.AddKeyword($"   del mdb.models[modelName]");
        //    builder.AddKeyword($"mdb.Model(name = modelName)");
        //    builder.AddKeyword($"model = mdb.models[modelName]");
        //}

        private void AddHeader()
        {
            builder.AddCommentedHeader("Script of prism, created by StructureHelper");
            builder.AddKeyword("from abaqus import *");
            builder.AddKeyword("from abaqusConstants import *");
            builder.AddKeyword("import regionToolset");
            builder.AddKeyword("import mesh");
        }
    }
}
