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
        const int descentStepNumber = 20;
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

        public CDPInelasticStrain Convert(IConcreteFeaMaterial source)
        {
            concreteMaterial = source;
            SetFields();
            SetDiagram();
            SetCompressionStrain();
            SetCompressionStress();
            CDPInelasticStrain cdp = new();
            for (int i = 0; i < inelasticStrainList.Count; i++)
            {
                cdp.InelasticStrainList.Add(inelasticStrainList[i]);
                cdp.StressList.Add(stressList[i]);
                cdp.DamageList.Add(damageList[i]);
            }
            return cdp;
        }

        private void SetCompressionStress()
        {
            stressList = new();
            elasticStrainList = new();
            inelasticStrainList = new();
            damageList = new();
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
                    damage = 1.0 - stress / (initialModulus * totalStrain);
                    damage = Math.Max(0.0, Math.Min(0.999, damage));
                }
                damageList.Add(damage);
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
            diagram = new ConcreteEC2Diagram(1.0 * modulusRatio, peakStrain, compressionStrength);
        }

        private void SetFields()
        {
            initialModulus = concreteMaterial.YoungsModulus;
            compressionStrength = concreteMaterial.CompressionProperties.Strength;
            minStrain = concreteMaterial.CompressionProperties.ElasticStressRatio / initialModulus;
            peakStrain = concreteMaterial.CompressionProperties.PeakStrain;
            maxStrain = peakStrain * 4.0;
        }
    }
}
