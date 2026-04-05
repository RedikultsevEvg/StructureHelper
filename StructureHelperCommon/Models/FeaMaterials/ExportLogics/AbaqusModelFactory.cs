using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.FeaMaterials.ExportLogics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public enum ModelType
    {
        Cube = 0,
        Prism = 1,
        Cylinder = 2,
    }
    public static class AbaqusModelFactory
    {
            const string modelVaiableName = "model";
            const string materialVariableName = "material";
            const string partVariableName = "part";
            const string sectionVariableName = "section";
            const string initialStepName = "Initial";
            const string secondStepName = "Compression";

        public static string GetScript(IFeaMaterial feaMaterial, ModelType modelType)
        {
            if (modelType == ModelType.Cube)
            {
                var geometryBlock = GetPrismGeometry(0.15, 0.15, 0.15);
                return ProcessSolid(feaMaterial, geometryBlock);
            }
            else if (modelType == ModelType.Prism)
            {
                var geometryBlock = GetPrismGeometry(0.1, 0.1, 0.3);
                return ProcessSolid(feaMaterial, geometryBlock);
            }
            else if (modelType == ModelType.Cylinder)
            {
                var geometryBlock = GetCylinderGeometry(0.1, 0.2);
                return ProcessSolid(feaMaterial, geometryBlock);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(modelType));
            }

        }

        private static string ProcessSolid(IFeaMaterial feaMaterial, IAbaqusScriptBlock geometryBlock)
        {
            ModelBlock modelBlock = GetModel();
            var materialBlock = MaterialBlockFactory.GetMaterialBlock(feaMaterial, modelVaiableName, materialVariableName);
            SolidSectionBlock sectionBlock = GetSectionBlock();
            AssemblyBlock assemblyBlock = GetAssemblyBlock();
            StepBlock stepBlock = GetStepBlock();
            BoundaryConditionBlock boundaryConditionBlock = GetBCBlock();
            MeshBlock meshBlock = GetMeshBlock(feaMaterial);
            FieldBlock fieldBlock = new(modelVaiableName);
            JobBlock jobBlock = new(modelVaiableName)
            {
                JobName = "CompressionTest"
            };

            var script = new AbaqusScript()
                .Add(modelBlock)
                .Add(geometryBlock)
                .Add(materialBlock)
                .Add(sectionBlock)
                .Add(assemblyBlock)
                .Add(stepBlock)
                .Add(boundaryConditionBlock)
                .Add(meshBlock)
                .Add(fieldBlock)
                .Add(jobBlock)
                .Build();
            return script;
        }

        private static BoundaryConditionBlock GetBCBlock()
        {
            return new(modelVaiableName)
            {
                InitialStepName = initialStepName,
                SecondStepName = secondStepName,
                DisplacementX = 0.0,
                DisplacementY = 0.0,
                DisplacementZ = -0.001,
            };
        }

        private static StepBlock GetStepBlock()
        {
            return new()
            {
                ModelVariableName = modelVaiableName,
                InitialStepName = initialStepName,
                SecondStepName = secondStepName
            };
        }

        private static AssemblyBlock GetAssemblyBlock()
        {
            return new()
            {
                ModelVariableName = modelVaiableName,
                PartVariableName = partVariableName
            };
        }

        private static SolidSectionBlock GetSectionBlock()
        {
            return new()
            {
                MaterialVariableName = materialVariableName,
                PartVariableName = partVariableName,
                SectionVariableName = sectionVariableName,
            };
        }

        private static MeshBlock GetMeshBlock(IFeaMaterial feaMaterial)
        {
            double meshSize = 0.01;
            if (feaMaterial is IConcreteFeaMaterial concreteFeaMaterial)
            {
                meshSize = concreteFeaMaterial.TensionProperties.FeSize;
            }

            MeshBlock meshBlock = new()
            {
                ModelVariableName = modelVaiableName,
                MeshSize = meshSize,
            };
            return meshBlock;
        }

        private static PrismGeometryBlock GetPrismGeometry(double width, double depth, double heigth)
        {
            return new()
            {
                PartName = "Prism",
                PartVariableName = partVariableName,
                ModelVariableName = modelVaiableName,
                Width = width,
                Depth = depth,
                Height = heigth,
            };
        }

        private static CylinderGeometryBlock GetCylinderGeometry(double diameter, double heigth)
        {
            return new()
            {
                PartName = "Cylinder",
                PartVariableName = partVariableName,
                ModelVariableName = modelVaiableName,
                Diameter = diameter,
                Height = heigth,
            };
        }

        private static ModelBlock GetModel()
        {
            return new("Prism model")
            {
                ModelVariableName = modelVaiableName,
                AddImport = true
            };
        }
    }
}
