using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteToCDPTensionConvertStrategy : IObjectConvertStrategy<CDPInelasticStrain, IConcreteFeaMaterial>
    {
        const int stepNumber = 40;
        const double exponentialFactor = 5.14;
        private IConcreteFeaMaterial concreteMaterial;
        private double initialModulus;
        private double fractureEnergy;
        private double strength;
        private double ultimateCrackWidth;
        private double elementSize;
        private double ultimateElasticStrain;
        private List<double> inelasticStrainList;
        private List<double> stressList;
        private List<double> damageList;


        public CDPInelasticStrain Convert(IConcreteFeaMaterial source)
        {
            concreteMaterial = source;
            SetFields();
            GetStress();
            CDPInelasticStrain cdp = new();
            for (int i = 0; i < inelasticStrainList.Count; i++)
            {
                cdp.InelasticStrainList.Add(inelasticStrainList[i]);
                cdp.StressList.Add(stressList[i]);
                cdp.DamageList.Add(damageList[i]);
            }
            return cdp;
        }

        private void GetStress()
        {
            inelasticStrainList = [];
            stressList = [];
            damageList = [];
            double step = ultimateCrackWidth / stepNumber;
            inelasticStrainList.Add(0.0);
            stressList.Add(strength);
            damageList.Add(0.0);
            for (int i = 1; i <= stepNumber; i++)
            {
                double crackWidth = step * i;
                double inelasticStrain = crackWidth / elementSize;
                inelasticStrainList.Add(inelasticStrain);
                double stress = GetTensionStress(strength, ultimateCrackWidth, crackWidth);
                stressList.Add(stress);
                double totalStrain = ultimateElasticStrain + inelasticStrain;
                double damage = 1.0 - stress / strength;
                damage = Math.Max(0.0, Math.Min(0.999, damage));
                damageList.Add(damage);
            }
        }

        private void SetFields()
        {
            initialModulus = concreteMaterial.YoungsModulus;
            fractureEnergy = concreteMaterial.TensionProperties.FractureEnergy;
            strength = concreteMaterial.TensionProperties.Strength;
            ultimateCrackWidth = exponentialFactor * fractureEnergy / strength;
            elementSize = concreteMaterial.TensionProperties.FeSize;
            ultimateElasticStrain = strength / initialModulus; 
        }

        private double GetTensionStress(double tensileStrength, double ultimateCrackWidth, double crackWidth)
        {
            //Material constants for concrete in tension
            double c1 = 3.0;
            double c2 = 6.93;
            double crackFactor = crackWidth / ultimateCrackWidth;
            double factorC1 = c1 * crackFactor;
            double factorC2 = -c2 * crackFactor;
            double stressDecreasingFactor = (1 + Math.Pow(factorC1, 3)) * (Math.Exp(factorC2));
            return stressDecreasingFactor * tensileStrength;
        }
    }
}
