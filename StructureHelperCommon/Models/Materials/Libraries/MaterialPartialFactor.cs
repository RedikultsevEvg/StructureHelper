using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class MaterialPartialFactor : IMaterialPartialFactor
    {
        private double factorValue;
        private IUpdateStrategy<IMaterialPartialFactor> updateStrategy;
        private IUpdateStrategy<IMaterialPartialFactor> UpdateStrategy => updateStrategy = new MaterialPartialFactorUpdateStrategy();

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


        public IPartialFactor GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            var newItem = new MaterialPartialFactor(id);
            UpdateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
