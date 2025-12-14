using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.DiagramTemplates;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class SteelMaterialBuilderLogic : IMaterialLogic
    {
        private const double safetyFactorForULS = 1.05;
        private const double safetyFactorforSLS = 1.0;
        private ISteelMaterialLogicOption option;
        IObjectConvertStrategy<ISteelDiagramAbsoluteProperty, ISteelDiagramRelativeProperty> convertStrategy;
        private IMaterialFactorLogic factorLogic;
        private double compressionStrength;
        private double tensionStrength;

        public Guid Id { get; }
        public string Name { get; set; }
        public IMaterialLogicOptions Options { get; set; }
        public MaterialTypes MaterialType { get; set; }
        public DiagramType DiagramType { get; set; }


        public SteelMaterialBuilderLogic(Guid id)
        {
            Id = id;
        }

        public IMaterial GetLoaderMaterial()
        {
            option = Options as ISteelMaterialLogicOption;
            if (DiagramType == DiagramType.TripleLinear)
            {
                factorLogic = new MaterialFactorLogic(Options.SafetyFactors);
                GetStrength();
                Material material = new()
                {
                    InitModulus = option.MaterialEntity.InitialModulus,
                    Diagram = GetDiagram(),
                    LimitPositiveStrain = GetMaxStrain(tensionStrength),
                    LimitNegativeStrain = -GetMaxStrain(compressionStrength),
                };
                return material;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(DiagramType));
            }
        }

        private double GetMaxStrain(double strength)
        {
            double elasticCompressionStrain = strength / Options.MaterialEntity.InitialModulus;
            double fullCompressionStrain = elasticCompressionStrain * (1 + ((ISteelMaterialLogicOption)Options).MaxPlasticStrainRatio);
            return fullCompressionStrain;
        }

        private Func<double, double> GetDiagram()
        {
            MultiLinearStressStrainDiagram compressionDiagram = GetMultiLinearDiagram(compressionStrength);
            MultiLinearStressStrainDiagram tensionDiagram = GetMultiLinearDiagram(tensionStrength);
            var posNegDiagram = new PosNegDigramDecorator(tensionDiagram, compressionDiagram);
            return posNegDiagram.GetStressByStrain;
        }

        private MultiLinearStressStrainDiagram GetMultiLinearDiagram(double strength)
        {
            convertStrategy = new SteelRelativeToAbsoluteDiagramConvertLogic(Options.MaterialEntity.InitialModulus, strength);
            var diagramProperty = SteelDiagramPropertyFactory.GetProperty(((ISteelMaterialEntity)Options.MaterialEntity).PropertyType);
            var absoluteProperty = convertStrategy.Convert(diagramProperty);
            List<IStressStrainPair> stressStrainPairs = new()
            {
                new StressStrainPair() { Stress = 0, Strain = 0},
                new StressStrainPair() { Stress = absoluteProperty.StressOfProportionality, Strain = absoluteProperty.StrainOfProportionality},
                new StressStrainPair() { Stress = absoluteProperty.BaseStrength, Strain = absoluteProperty.StrainOfStartOfYielding},
                new StressStrainPair() { Stress = absoluteProperty.BaseStrength, Strain = absoluteProperty.StrainOfEndOfYielding},
                new StressStrainPair() { Stress = absoluteProperty.StressOfUltimateStrength, Strain = absoluteProperty.StrainOfUltimateStrength},
                new StressStrainPair() { Stress = absoluteProperty.StressOfFracture, Strain = absoluteProperty.StrainOfFracture},

            };
            var multiDiagram = new MultiLinearStressStrainDiagram(stressStrainPairs);
            return multiDiagram;
        }

        public void GetStrength()
        {
            double baseStength = Options.MaterialEntity.MainStrength;
            var factors = factorLogic.GetTotalFactor(Options.LimitState, Options.CalcTerm);
            double compressionFactor = 1d;
            double tensionFactor = 1d;
            compressionFactor *= factors.Compressive;
            tensionFactor *= factors.Tensile;
            double factor;
            if (Options.LimitState == Infrastructures.Enums.LimitStates.ULS)
            {
                factor = safetyFactorForULS;
            }
            else if (Options.LimitState == Infrastructures.Enums.LimitStates.SLS)
            {
                factor = safetyFactorforSLS;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(Options.LimitState));
            }
            double strength = baseStength / factor;
            compressionStrength = strength * compressionFactor;
            tensionStrength = strength * tensionFactor;
        }
    }
}
