using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class MaterialPartialFactor : IMaterialPartialFactor
    {
        private double factorValue;

        public StressStates StressState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public LimitStates LimitState { get; set; }
        public double FactorValue
        {
            get => factorValue;
            set
            {
                if (value < 0 )
                {
                    throw new StructureHelperException(ErrorStrings.FactorMustBeGraterThanZero);
                }
                factorValue = value;
            }
        }

        public MaterialPartialFactor()
        {
            StressState = StressStates.Compression;
            LimitState = LimitStates.ULS;
            CalcTerm = CalcTerms.LongTerm;
            FactorValue = 1d;
        }

        public object Clone()
        {
            var newItem = new MaterialPartialFactor()
            {
                StressState = StressState,
                CalcTerm = CalcTerm,
                LimitState = LimitState,
                FactorValue = FactorValue,
            };
            return newItem;
        }
    }
}
