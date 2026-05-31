using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.DiagramTemplates;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;
using System;

namespace StructureHelperCommon.Models.Materials
{
    public class ConcreteBilinearLogic : IMaterialLogic
    {
        private const double limitCompessiveStrain = 0.0028;
        private const double yieldCompressiveStrain = 0.0015;
        private const double concreteFactoredCompressiveStrength = 36.3375e6;
        private const double limitTensileStrain = 0.00015;
        private const double yieldTensileStrain = 0.000075;
        private const double concreteFactoredTensileStrength = 0.0;
        private const double initialModulus = 50e9;

        public string Name { get; set; }
        public IMaterialLogicOptions Options { get; set; }
        public MaterialTypes MaterialType { get; set; }
        public DiagramType DiagramType { get; set; }

        public Guid Id { get; }

        public ConcreteBilinearLogic(Guid id)
        {
            Id = id;
        }

        public IMaterial GetLoaderMaterial()
        {
            PosNegDigramDecorator fullDiagram = GetDiagram();
            ConcreteMaterial concreteMaterial = new()
            {
                LimitNegativeStrain = - limitCompessiveStrain,
                InitModulus = initialModulus,
                CrackStrain = limitTensileStrain,
                Diagram = fullDiagram.GetStressByStrain
            };
            return concreteMaterial;
        }

        private static PosNegDigramDecorator GetDiagram()
        {
            BiLinearDiagram compressiveDiagram = new();
            compressiveDiagram.InitModulus = concreteFactoredCompressiveStrength / yieldCompressiveStrain;
            compressiveDiagram.Strain1 = yieldCompressiveStrain;
            compressiveDiagram.MaxStrain = limitCompessiveStrain;
            IDiagram tensileDigram = GetTensile();
            PosNegDigramDecorator fullDiagram = new(tensileDigram, compressiveDiagram);
            return fullDiagram;
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
