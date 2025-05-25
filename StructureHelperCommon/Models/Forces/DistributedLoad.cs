using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using System;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces
{
    public class DistributedLoad : IDistributedLoad
    {
        private double relativeLoadLevel;
        private IUpdateStrategy<IDistributedLoad> updateStrategy;

        public Guid Id { get; }

        public string Name { get; set; } = string.Empty;
        public IForceTuple LoadValue { get; set; } = new ForceTuple(Guid.NewGuid());
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

        public double StartCoordinate { get; set; } = double.NegativeInfinity;
        public double EndCoordinate { get; set; } = double.PositiveInfinity;
        /// <inheritdoc/>
        public double LoadRatio { get; set; } = 1;
        /// <inheritdoc/>
        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationProperty(Guid.NewGuid()) { LimitState = LimitStates.ULS, CalcTerm = CalcTerms.ShortTerm};

        public DistributedLoad(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            DistributedLoad newItem = new(Guid.NewGuid());
            updateStrategy ??= new DistributedLoadUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
