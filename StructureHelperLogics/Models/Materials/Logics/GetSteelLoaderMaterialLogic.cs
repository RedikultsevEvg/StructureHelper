using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.DiagramTemplates;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Windows.Media.TextFormatting;

namespace StructureHelperLogics.Models.Materials.Logics
{
    public class GetSteelLoaderMaterialLogic
    {
        IObjectConvertStrategy<ISteelDiagramAbsoluteProperty, ISteelDiagramRelativeProperty> convertStrategy;

        public double InitialModulus { get; set; }
        public double CompressionStrength { get; set; }
        public double TensionStrength { get; internal set; }
        public double MaxPlasticStrainRatio { get; set; }
        public ISteelDiagramRelativeProperty DiagramProperty { get; set; }

        public IMaterial GetMaterial()
        {
            double elasticCompressionStrain = CompressionStrength / InitialModulus;
            double fullCompressionStrain = elasticCompressionStrain * (1 + MaxPlasticStrainRatio);

            Material material = new()
            {
                Diagram = GetDiagram(),
                LimitPositiveStrain = fullCompressionStrain,
                LimitNegativeStrain = - fullCompressionStrain,
            };
            return material;
        }

        private Func<double, double> GetDiagram()
        {
            MultiLinearStressStrainDiagram compressionDiagram = GetMultiLinearDiagram(CompressionStrength);
            MultiLinearStressStrainDiagram tensionDiagram = GetMultiLinearDiagram(TensionStrength);
            var posNegDiagram = new PosNegDigramDecorator(tensionDiagram, compressionDiagram);
            return posNegDiagram.GetStressByStrain;
        }

        private MultiLinearStressStrainDiagram GetMultiLinearDiagram(double strength)
        {
            convertStrategy ??= new SteelRelativeToAbsoluteDiagramConvertLogic(InitialModulus, strength);
            var absoluteProperty = convertStrategy.Convert(DiagramProperty);
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
    }
}
