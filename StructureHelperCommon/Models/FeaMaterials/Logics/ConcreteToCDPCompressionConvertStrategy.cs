using LoaderCalculator.Data.Materials.DiagramTemplates;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteToCDPCompressionConvertStrategy : IObjectConvertStrategy<CDPInelasticStrain, IConcreteFeaMaterial>
    {
        const int incentStepNumber = 15;
        const int descentStepNumber = 75;
        private IConcreteFeaMaterial concreteMaterial;
        private double initialModulus;
        private double compressionStrength;
        private double minStrain;
        private double peakStrain;
        private double maxStrain;
        private ConcreteEC2Diagram diagram;
        private List<double> totalStrainList;
        private List<double> elasticStrainList;
        private List<double> inelasticStrainList;
        private List<double> stressList;
        private List<double> damageList;
        private List<double> plasticStrainList;

        public CDPInelasticStrain Convert(IConcreteFeaMaterial source)
        {
            concreteMaterial = source;
            SetFields();
            SetDiagram();
            SetCompressionStrain();
            SetCompressionStress();
            CDPInelasticStrain cdp = new();
            double deltaStrain = inelasticStrainList[0];
            for (int i = 0; i < incentStepNumber; i++)
            {
                inelasticStrainList[i] -= deltaStrain / incentStepNumber * (incentStepNumber - i);
            }
            for (int i = 0; i < inelasticStrainList.Count; i++)
            {
                cdp.ElasticStrainList.Add(elasticStrainList[i]);
                cdp.InelasticStrainList.Add(inelasticStrainList[i]);
                cdp.StressList.Add(stressList[i]);
                cdp.DamageList.Add(damageList[i]);
                cdp.PlasticStrainList.Add(plasticStrainList[i]);
            }
            return cdp;
        }

        private void SetCompressionStress()
        {
            stressList = new();
            elasticStrainList = new();
            inelasticStrainList = new();
            damageList = new();
            plasticStrainList = new();
            foreach (var totalStrain in totalStrainList)
            {
                double stress = diagram.GetStressByStrain(totalStrain);
                stressList.Add(stress);
                double elasticStrain = stress / initialModulus;
                double inelasticStrain = Math.Max(0.0, totalStrain - elasticStrain);
                elasticStrainList.Add(elasticStrain);
                inelasticStrainList.Add(inelasticStrain);
                double damage = 0.0;
                if (totalStrain > peakStrain)
                {
                    damage = 1.0 - stress / compressionStrength;
                    damage = Math.Max(0.0, Math.Min(0.999, damage));
                }
                damageList.Add(damage);
                double plasticStrain = inelasticStrain - damage * stress / (1.0 - damage) / initialModulus;
                plasticStrainList.Add(plasticStrain);
            }
        }

        private void SetCompressionStrain()
        {
            totalStrainList = new();
            double insentStep = (peakStrain - minStrain) / incentStepNumber;
            for (int i = 1; i <= incentStepNumber; i++)
            {
                var strain = minStrain + i * insentStep;
                totalStrainList.Add(strain);
            }
            double descentStep = (maxStrain - peakStrain) / descentStepNumber;
            for (int i = 1; i <= descentStepNumber; i++)
            {
                var strain = peakStrain + i * descentStep;
                totalStrainList.Add(strain);
            }
        }

        private void SetDiagram()
        {
            var secantModulus = compressionStrength / peakStrain;
            if (initialModulus <= secantModulus)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": initial modulus of elasticity of concrete must be greater than secant modulus at peak point, but was initial modulus E0 = {initialModulus}, secant modulus Esec = {secantModulus}");
            }
            var modulusRatio = initialModulus / secantModulus;
            diagram = new ConcreteEC2Diagram(1.0 * modulusRatio, peakStrain, compressionStrength, 1.0 / concreteMaterial.CompressionProperties.DescendingScaleFactor);
        }

        private void SetFields()
        {
            initialModulus = concreteMaterial.YoungModulus;
            compressionStrength = concreteMaterial.CompressionProperties.Strength;
            minStrain = concreteMaterial.CompressionProperties.ElasticStressRatio * compressionStrength / initialModulus;
            peakStrain = concreteMaterial.CompressionProperties.PeakStrain;
            maxStrain = peakStrain * 4.0;
        }
    }
}
