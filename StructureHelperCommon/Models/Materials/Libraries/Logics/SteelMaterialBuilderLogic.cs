using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.DiagramTemplates;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials.Libraries.Logics;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class SteelMaterialBuilderLogic : IMaterialLogic
    {
        private ISteelMaterialLogicOption option;
        private ICheckEntityLogic<ISteelMaterialLogicOption> checkEntityLogic;
        private ICheckEntityLogic<ISteelMaterialLogicOption> CheckEntityLogic => checkEntityLogic ??= new SteelMaterialLogicOptionCheckStrategy() { Entity = option};
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
            CheckOptions();
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

        private void CheckOptions()
        {
            if (Options is not ISteelMaterialLogicOption steelOption)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(Options));
            }
            option = steelOption;
            if (CheckEntityLogic.Check() == false)
            {
                throw new StructureHelperException("Option is not correct: \n" + CheckEntityLogic.CheckResult);
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
            IDiagram compressionDiagram = GetDiagram(compressionStrength);
            IDiagram tensionDiagram = GetDiagram(tensionStrength);
            var posNegDiagram = new PosNegDigramDecorator(tensionDiagram, compressionDiagram);
            return posNegDiagram.GetStressByStrain;
        }

        private IDiagram GetDiagram(double strength)
        {
            if (strength == 0.0)
            {
                ConstantValueDiagram diagram = new() { ConstantValue = 0.0 };
                return diagram;
            }
            MultiLinearStressStrainDiagram multiDiagram = GetMultiLinearDiagram(strength);
            return multiDiagram;
        }

        private MultiLinearStressStrainDiagram GetMultiLinearDiagram(double strength)
        {
            convertStrategy = new SteelRelativeToAbsoluteDiagramConvertLogic(Options.MaterialEntity.InitialModulus, strength);
            var diagramProperty = SteelDiagramPropertyFactory.GetProperty(((ISteelMaterialEntity)Options.MaterialEntity).PropertyType);
            var absoluteProperty = convertStrategy.Convert(diagramProperty);
            List<IStressStrainPair> stressStrainPairs;
            if (DiagramType == DiagramType.Bilinear)
            {
                stressStrainPairs = GetBiLinearStressStrainPairs(absoluteProperty);
            }
            else if (DiagramType == DiagramType.TripleLinear)
            {
                stressStrainPairs = GetTripleLinearStressStrainPairs(absoluteProperty);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(DiagramType));
            }
            var multiDiagram = new MultiLinearStressStrainDiagram(stressStrainPairs);
            return multiDiagram;
        }

        private static List<IStressStrainPair> GetBiLinearStressStrainPairs(ISteelDiagramAbsoluteProperty absoluteProperty)
        {
            return new()
            {
                new StressStrainPair() { Stress = 0, Strain = 0},
                new StressStrainPair() { Stress = absoluteProperty.BaseStrength, Strain = absoluteProperty.BaseStrain},
                new StressStrainPair() { Stress = absoluteProperty.BaseStrength, Strain = absoluteProperty.StrainOfEndOfYielding},
                new StressStrainPair() { Stress = absoluteProperty.StressOfUltimateStrength, Strain = absoluteProperty.StrainOfUltimateStrength},
                new StressStrainPair() { Stress = absoluteProperty.StressOfFracture, Strain = absoluteProperty.StrainOfFracture},

            };
        }

        private static List<IStressStrainPair> GetTripleLinearStressStrainPairs(ISteelDiagramAbsoluteProperty absoluteProperty)
        {
            return new()
            {
                new StressStrainPair() { Stress = 0, Strain = 0},
                new StressStrainPair() { Stress = absoluteProperty.StressOfProportionality, Strain = absoluteProperty.StrainOfProportionality},
                new StressStrainPair() { Stress = absoluteProperty.BaseStrength, Strain = absoluteProperty.StrainOfStartOfYielding},
                new StressStrainPair() { Stress = absoluteProperty.BaseStrength, Strain = absoluteProperty.StrainOfEndOfYielding},
                new StressStrainPair() { Stress = absoluteProperty.StressOfUltimateStrength, Strain = absoluteProperty.StrainOfUltimateStrength},
                new StressStrainPair() { Stress = absoluteProperty.StressOfFracture, Strain = absoluteProperty.StrainOfFracture},

            };
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
                factor = option.UlsFactor;
            }
            else if (Options.LimitState == Infrastructures.Enums.LimitStates.SLS)
            {
                factor = option.SlsFactor;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(Options.LimitState));
            }
            double strength = baseStength * option.ThicknessFactor * option.WorkConditionFactor / factor;
            compressionStrength = strength * compressionFactor;
            tensionStrength = strength * tensionFactor;
        }
    }
}
