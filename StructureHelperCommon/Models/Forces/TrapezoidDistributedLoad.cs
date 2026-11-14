using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Forces
{
    public class TrapezoidDistributedLoad : ITrapezoidDistributedLoad
    {
        private double relativeLoadLevel;

        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public IForceTuple StartLoadValue { get; set; } = new ForceTuple(Guid.NewGuid());
        public IForceTuple EndLoadValue { get; set; } = new ForceTuple(Guid.NewGuid());
        public double StartCoordinate { get; set; } = double.NegativeInfinity;
        public double EndCoordinate { get; set; } = double.PositiveInfinity;
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
        public double LoadRatio { get; set; } = 1.0;
        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationProperty(Guid.NewGuid()) { LimitState = LimitStates.ULS, CalcTerm = CalcTerms.ShortTerm };


        public TrapezoidDistributedLoad(Guid id)
        {
            Id = id;
        }

        public IBeamSpanLoad GetClone(IBeamSpanLoad sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            TrapezoidDistributedLoad newItem = new(id);
            if (sourceObject is not ITrapezoidDistributedLoad trapezoid)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
            new TrapezoidDistributedLoadUpdateStrategy().Update(newItem, trapezoid);
            return newItem;
        }
    }
}
