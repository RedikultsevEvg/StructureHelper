using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class SteelRelativeToAbsoluteDiagramConvertLogic : IObjectConvertStrategy<ISteelDiagramAbsoluteProperty, ISteelDiagramRelativeProperty>
    {
        private readonly double initialYoungsModulus;
        private readonly double baseStrength;

        /// <summary>
        /// Converts relatives properties of diagram of steel to absolute one
        /// </summary>
        /// <param name="initialYoungsModulus">Initial modulus of elasticity (Young's modulus), Pa</param>
        /// <param name="baseStrength">Stress of yelding, Pa</param>
        public SteelRelativeToAbsoluteDiagramConvertLogic(double initialYoungsModulus, double baseStrength)
        {
            this.initialYoungsModulus = initialYoungsModulus;
            this.baseStrength = baseStrength;
        }

        public ISteelDiagramAbsoluteProperty Convert(ISteelDiagramRelativeProperty source)
        {
            CheckInputData(source);
            double absoluteYieldingStrain = baseStrength / initialYoungsModulus;
            SteelDiagramAbsoluteProperty result = new()
            {
                InitialYoungsModulus = initialYoungsModulus,
                BaseStrength = baseStrength,
                StressOfProportionality = baseStrength * source.StrainOfProportionality,
                StrainOfProportionality = absoluteYieldingStrain * source.StrainOfProportionality,
                StrainOfStartOfYielding = absoluteYieldingStrain * source.StrainOfStartOfYielding,
                StrainOfEndOfYielding = absoluteYieldingStrain * source.StrainOfEndOfYielding,
                StrainOfUltimateStrength = absoluteYieldingStrain * source.StrainOfUltimateStrength,
                StrainOfFracture = absoluteYieldingStrain * source.StrainOfFracture,
                StressOfUltimateStrength = baseStrength * source.StressOfUltimateStrength,
                StressOfFracture = baseStrength * source.StressOfFracture,
            };
            return result;
        }

        private void CheckInputData(ISteelDiagramRelativeProperty source)
        {
            if (initialYoungsModulus <= 0.0)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Initial modulus is not correct, it must be positive, but was {initialYoungsModulus}");
            }
            if (baseStrength < 0.0)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Stress of yelding is not correct, it must be positive, but was {baseStrength}");
            }
        }
    }
}
