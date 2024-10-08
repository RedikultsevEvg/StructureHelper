using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class MaterialPartialFactor : IMaterialPartialFactor
    {
        private double factorValue;
        private IUpdateStrategy<IMaterialPartialFactor> updateStrategy = new MaterialPartialFactorUpdateStrategy();

        public Guid Id { get; }
        public StressStates StressState { get; set; } = StressStates.Compression;
        public CalcTerms CalcTerm { get; set; } = CalcTerms.LongTerm;
        public LimitStates LimitState { get; set; } = LimitStates.ULS;
        public double FactorValue
        {
            get => factorValue;
            set
            {
                if (value < 0)
                {
                    throw new StructureHelperException(ErrorStrings.FactorMustBeGraterThanZero);
                }
                factorValue = value;
            }
        }


        public MaterialPartialFactor(Guid id)
        {
            Id = id;
            FactorValue = 1d;
        }

        public MaterialPartialFactor() : this (Guid.NewGuid())
        { }

        public object Clone()
        {
            var newItem = new MaterialPartialFactor();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
