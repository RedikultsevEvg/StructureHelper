using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Globalization;

namespace StructureHelperCommon.Models.FeaMaterials.ExportLogics
{
    public class AbaqusPrismTestBuilder : IScriptBuilder
    {
        const double lengthFactor = 1000; //metres to millymetres
        const string sectionName = "Section-1";
        const string initialStepName = "Initial";
        const string secondStepName = "Compression";

        private string modelName = "ConcreteCDP";
        private string scriptMaterialName;
        private string materialName;


        public KeywordBuilder Builder { get; } = new();
        public double Height { get; set; } = 0.3;
        public double Width { get; set; } = 0.1;
        public double Depth { get; set; } = 0.1;

        public string Build(IFeaMaterial material)
        {
            AddHeader();
            AddModel();
            AddGeometry();
            AddMaterial(material);
            AddSection();
            AddAssembly();
            AddStep();
            AddBoundaryConditions();
            AddMesh(material);
            AddField();
            AddJob();
            Builder.AddKeyword($"print(\"Model created successfully.\")");

            return Builder.ToString();
        }

        private void AddJob()
        {
            Builder.AddCommentedHeader("Job");
            Builder.AddKeyword($"mdb.Job(name='ConcreteCompression',\r\n        model=modelName,\r\n        numCpus=1)");
            
        }

        private void AddField()
        {
            Builder.AddCommentedHeader("Field Output");
            Builder.AddKeyword($"model.fieldOutputRequests['F-Output-1'].setValues(variables=('S', 'U', 'E', 'PE', 'PEEQ','DAMAGET', 'DAMAGEC', 'STATUS'))");
        }

        private void AddMesh(IFeaMaterial material)
        {
            Builder.AddCommentedHeader("Mesh");
            double meshSize = 0.01;
            if (material is IConcreteFeaMaterial concreteFeaMaterial)
            {
                meshSize = concreteFeaMaterial.TensionProperties.FeSize;
            }
            Builder.AddKeyword($"part.seedPart(size={FormatDouble(meshSize * lengthFactor)})");
            Builder.AddKeyword($"part.setElementType(regions=(cells,), elemTypes=(mesh.ElemType(elemCode=C3D8R, elemLibrary=STANDARD),))");
            Builder.AddKeyword($"part.generateMesh()");
            Builder.AddKeyword($"assembly.regenerate()");    
        }

        private void AddBoundaryConditions()
        {
            Builder.AddCommentedHeader("Boundary Conditions");
            Builder.AddComment("Bottom fixed");
            Builder.AddKeyword($"bottomFace = instance.faces.findAt(((width/2.0, depth/2.0, 0.0),))");
            Builder.AddKeyword($"region = regionToolset.Region(faces=bottomFace)");
            Builder.AddKeyword($"model.DisplacementBC(name='FixBottom',");
            Builder.AddKeyword($"   createStepName='{initialStepName}',");
            Builder.AddKeyword($"   region=region,");
            Builder.AddKeyword($"   u1=0.0, u2=0.0, u3=0.0,");
            Builder.AddKeyword($"   ur1=0.0, ur2=0.0, ur3=0.0)");
            Builder.AddComment("Top displacement (compression)");
            Builder.AddKeyword($"topFace = instance.faces.findAt(((width/2.0, depth/2.0, height),))");
            Builder.AddKeyword($"region = regionToolset.Region(faces=topFace)");
            Builder.AddKeyword($"model.DisplacementBC(name='TopLoad',");
            Builder.AddKeyword($"   createStepName='{secondStepName}',");
            Builder.AddKeyword($"   region=region,");
            Builder.AddKeyword($"   u1=0.0, u2=0.05, u3=-1.0,   # 1 mm compression");
            Builder.AddKeyword($"   amplitude=UNSET)");
        }

        private void AddStep()
        {
            Builder.AddCommentedHeader("Step");
            Builder.AddKeyword($"model.StaticStep(name='{secondStepName}',previous='{initialStepName}', nlgeom=ON)");
        }

        private void AddAssembly()
        {
            Builder.AddCommentedHeader("Assembly");
            Builder.AddKeyword($"assembly = model.rootAssembly");
            Builder.AddKeyword($"instance = assembly.Instance(name='PrismInstance',part=part, dependent=ON)");
        }

        private void AddSection()
        {
            Builder.AddCommentedHeader("Section");
            Builder.AddKeyword($"model.HomogeneousSolidSection(name='{sectionName}',material='{materialName}', thickness=None)");
            Builder.AddKeyword($"cells = part.cells");
            Builder.AddKeyword($"region = regionToolset.Region(cells=cells)");
            Builder.AddKeyword($"part.SectionAssignment(region=region, sectionName='{sectionName}')");

        }

        private void AddMaterial(IFeaMaterial material)
        {
            Builder.AddCommentedHeader("Material");
            var builder = new FeaMaterialPyBuilder();
            var script = builder.Build(material);
            scriptMaterialName = builder.ScriptMaterialName;
            materialName = builder.MaterialName;
            Builder.AddRaw(script);
        }

        private void AddGeometry()
        {
            Builder.AddCommentedHeader("Geometry");
            Builder.AddKeyword($"width = {FormatDouble(Width * lengthFactor)}");
            Builder.AddKeyword($"depth = {FormatDouble(Depth * lengthFactor)}");
            Builder.AddKeyword($"height = {FormatDouble(Height * lengthFactor)}");
            Builder.AddKeyword(string.Empty);

            Builder.AddKeyword($"sketch = model.ConstrainedSketch(name='profile', sheetSize= 2 * height)");
            Builder.AddKeyword($"sketch.rectangle(point1=(- width / 2, - depth / 2), point2=(width / 2, depth / 2))");
            Builder.AddKeyword($"part = model.Part(name='Prism', dimensionality=THREE_D, type=DEFORMABLE_BODY)");
            Builder.AddKeyword($"part.BaseSolidExtrude(sketch=sketch, depth=height)");
        }

        private void AddModel()
        {
            Builder.AddCommentedHeader("Model");
            Builder.AddKeyword($"modelName = '{modelName}'");
            Builder.AddKeyword($"mdb.Model(name = modelName)");
            Builder.AddKeyword($"model = mdb.models[modelName]");
        }

        private void AddHeader()
        {
            Builder.AddCommentedHeader("Script of prism, created by StructureHelper");
            Builder.AddKeyword("from abaqus import *");
            Builder.AddKeyword("from abaqusConstants import *");
            Builder.AddKeyword("import regionToolset");
            Builder.AddKeyword("import mesh");
        }

        public string FormatDouble(double value)
        {
            var formatted = Convert.ToString(value, CultureInfo.InvariantCulture);
            return formatted;
        }
    }
}
