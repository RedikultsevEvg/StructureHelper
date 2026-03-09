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

        public double DisplacementX { get; set; } = 0.0;
        public double DisplacementY { get; set; } = 0.0;
        public double DisplacementZ { get; set; } = -0.001;

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
            Builder.AddComment("Change numCpus to your number of CPUs, i.e. 8, if you have 8 CPUs");
            Builder.AddKeyword($"mdb.Job(name='ConcreteCompression', model=modelName, numCpus=1)");
            
        }

        private void AddField()
        {
            Builder.AddCommentedHeader("Field Output");
            Builder.AddKeyword($"model.fieldOutputRequests['F-Output-1'].setValues(variables=(");
            Builder.AddKeyword($"'S', 'U', 'E', 'PE', 'PEEQ','DAMAGET', 'DAMAGEC', 'STATUS'))");
            Builder.AddCommentedHeader("History Output");
            Builder.AddKeyword($"model.historyOutputRequests['H-Output-1'].setValues(variables=(");
            Builder.AddKeyword($"'U1', 'U2', 'U3', 'RF1', 'RF2', 'RF3', 'TF1', 'TF2', 'TF3'),");
            Builder.AddKeyword($"frequency=10, region=rpRegion, sectionPoints=DEFAULT, rebar=EXCLUDE)");
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
            Builder.AddKeyword($"bottomFace = instance.faces.findAt(((0, 0, 0.0),))");
            Builder.AddKeyword($"region = regionToolset.Region(faces=bottomFace)");
            Builder.AddKeyword($"model.DisplacementBC(name='FixBottom',");
            Builder.AddKeyword($"   createStepName='{initialStepName}',");
            Builder.AddKeyword($"   region=region,");
            Builder.AddKeyword($"   u1=0.0, u2=0.0, u3=0.0,");
            Builder.AddKeyword($"   ur1=0.0, ur2=0.0, ur3=0.0)");
            Builder.AddComment(string.Empty);
            Builder.AddComment("Top displacement");
            Builder.AddKeyword($"topFace = instance.faces.findAt(((0, 0, height),))");
            Builder.AddKeyword($"region = regionToolset.Region(faces=topFace)");
            Builder.AddKeyword($"assembly.Surface(name='TopSurface', side1Faces=topFace)");
            Builder.AddComment("Coupling top face to reference point");
            Builder.AddKeyword($"model.Coupling(name='TopCoupling', controlPoint=assembly.sets['LoadRP'], surface=assembly.surfaces['TopSurface'],");
            Builder.AddKeyword($"influenceRadius=WHOLE_SURFACE, couplingType=KINEMATIC,");
            Builder.AddKeyword($"u1=ON, u2=ON, u3=ON, ur1=ON, ur2=ON, ur3=ON)");
            //Builder.AddKeyword($"model.DisplacementBC(name='TopLoad',");
            //Builder.AddKeyword($"   createStepName='{secondStepName}',");
            //Builder.AddKeyword($"   region=region,");
            //Builder.AddKeyword($"   u1={FormatDouble(DisplacementX * lengthFactor)},");
            //Builder.AddKeyword($"   u2={FormatDouble(DisplacementY * lengthFactor)},");
            //Builder.AddKeyword($"   u3={FormatDouble(DisplacementZ * lengthFactor)},");
            //Builder.AddKeyword($"   amplitude=UNSET)");
            Builder.AddKeyword($"model.DisplacementBC(name='TopLoad', createStepName='Compression', region=assembly.sets['LoadRP'],");
            Builder.AddKeyword($"   u1={FormatDouble(DisplacementX * lengthFactor)},");
            Builder.AddKeyword($"   u2={FormatDouble(DisplacementY * lengthFactor)},");
            Builder.AddKeyword($"   u3={FormatDouble(DisplacementZ * lengthFactor)},");
            Builder.AddKeyword($"   ur1=0,");
            Builder.AddKeyword($"   ur2=0,");
            Builder.AddKeyword($"   ur3=0)");
        }

        private void AddStep()
        {
            Builder.AddCommentedHeader("Step");
            Builder.AddKeyword($"model.StaticStep(name='{secondStepName}',previous='{initialStepName}', nlgeom=ON)");
            Builder.AddKeyword($"mdb.models['{modelName}'].steps['{secondStepName}'].setValues(maxNumInc=100, initialInc=0.05, maxInc=0.05)");
        }

        private void AddAssembly()
        {
            Builder.AddCommentedHeader("Assembly");
            Builder.AddKeyword($"assembly = model.rootAssembly");
            Builder.AddKeyword($"rp = assembly.ReferencePoint(point=(0, 0, height))");
            Builder.AddKeyword($"rpRegion = regionToolset.Region(referencePoints=(assembly.referencePoints[rp.id],))");
            Builder.AddKeyword($"assembly.Set(name='LoadRP',referencePoints=(assembly.referencePoints[rp.id],))");
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
            Builder.AddKeyword($"if modelName in mdb.models:");
            Builder.AddKeyword($"   del mdb.models[modelName]");
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
