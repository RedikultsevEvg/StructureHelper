using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.DiagramTemplates;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public class ConcreteTripleLinearLogic : IMaterialLogic
    {
        private const double limitCompessiveStrain = 0.0028;
        private const double yieldCompressiveStrengthRatio = 0.6;
        private const double yieldCompressiveStrain = 0.0020;
        private const double concreteFactoredCompressiveStrength = 36.3375e6;
        private const double limitTensileStrain = 0.00015;
        private const double yieldTensileStrain = 0.000075;
        private const double concreteFactoredTensileStrength = 0.0;
        private const double initialModulus = 50e9;

        public Guid Id {  get; }
        public string Name { get; set; }
        public IMaterialLogicOptions Options { get; set; }
        public MaterialTypes MaterialType { get; set; }
        public DiagramType DiagramType { get; set; }


        public ConcreteTripleLinearLogic(Guid id)
        {
            Id = id;
        }

        public IMaterial GetLoaderMaterial()
        {
            PosNegDigramDecorator fullDiagram = GetDiagram();
            ConcreteMaterial concreteMaterial = new()
            {
                LimitNegativeStrain = -limitCompessiveStrain,
                InitModulus = initialModulus,
                CrackStrain = limitTensileStrain,
                Diagram = fullDiagram.GetStressByStrain
            };
            return concreteMaterial;
        }

        private static PosNegDigramDecorator GetDiagram()
        {
            IDiagram compressiveDiagram = GetCompressiveDiagram();
            IDiagram tensileDigram = GetTensile();
            PosNegDigramDecorator fullDiagram = new(tensileDigram, compressiveDiagram);
            return fullDiagram;
        }

        private static IDiagram GetCompressiveDiagram()
        {
            const double fstPointStrength = yieldCompressiveStrengthRatio * concreteFactoredCompressiveStrength;
            List<IStressStrainPair> pointList = new()
            {
                new StressStrainPair()
                {
                    Stress = 0.0,
                    Strain = 0.0
                },
                new StressStrainPair()
                {
                    Stress = fstPointStrength,
                    Strain = fstPointStrength / initialModulus
                },
                new StressStrainPair()
                {
                    Stress = concreteFactoredCompressiveStrength,
                    Strain = yieldCompressiveStrain
                },
                new StressStrainPair()
                {
                    Stress = concreteFactoredCompressiveStrength,
                    Strain = limitCompessiveStrain
                },
            };
            MultiLinearStressStrainDiagram compressiveDiagram = new(pointList);
            return compressiveDiagram;
        }

        private static IDiagram GetTensile()
        {
            if (concreteFactoredTensileStrength == 0.0)
            {
                return new ConstantValueDiagram() { ConstantValue = 0.0 };
            }
            BiLinearDiagram tensileDigram = new()
            {
                Strain1 = concreteFactoredTensileStrength / yieldTensileStrain,
                MaxStrain = limitTensileStrain
            };
            return tensileDigram;
        }
    }
}
