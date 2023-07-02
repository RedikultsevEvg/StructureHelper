using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class MaterialPartialFactor : IMaterialPartialFactor
    {
        private double factorValue;
        public Guid Id { get; }
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


        public MaterialPartialFactor(Guid id)
        {
            Id = id;
            StressState = StressStates.Compression;
            LimitState = LimitStates.ULS;
            CalcTerm = CalcTerms.LongTerm;
            FactorValue = 1d;
        }

        public MaterialPartialFactor() : this (Guid.NewGuid())
        { }

        public object Clone()
        {
            var newItem = new MaterialPartialFactor();
            var updateStrategy = new MaterialPartialFactorUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
