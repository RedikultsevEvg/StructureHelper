using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ConcentratedForce : IConcentratedForce
    {
        private double relativeLoadLevel;
        private IUpdateStrategy<IConcentratedForce>? updateStrategy;

        /// <inheritdoc/>
        public Guid Id { get;}

        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public double ForceCoordinate { get; set; }
        /// <inheritdoc/>
        public IForceTuple ForceValue { get; set; } = new ForceTuple(Guid.NewGuid());
        /// <inheritdoc/>
        public double RelativeLoadLevel
        {
            get => relativeLoadLevel;
            set
            {
                if (value > 0.5d) { relativeLoadLevel = 0.5d; }
                if (value < -0.5d) { relativeLoadLevel = -0.5d; }
                relativeLoadLevel = value;
            }
        }
        /// <inheritdoc/>
        public double LoadRatio { get; set; } = 1;
        /// <inheritdoc/>
        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationProperty(Guid.NewGuid()) { LimitState = LimitStates.ULS, CalcTerm = CalcTerms.ShortTerm };

        public ConcentratedForce(Guid id)
        {
            Id = id;
        }

        public IConcentratedForce GetClone(IConcentratedForce sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            ConcentratedForce newItem = new(id);
            updateStrategy ??= new ConcentratedForceUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }

        public IBeamSpanLoad GetClone(IBeamSpanLoad sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
