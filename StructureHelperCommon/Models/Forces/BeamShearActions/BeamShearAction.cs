using StructureHelperCommon.Infrastructures.Interfaces;
using System;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    /// <inheritdoc/>
    public class BeamShearAction : IBeamShearAction
    {
        private IUpdateStrategy<IBeamShearAction> updateStrategy;
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public IFactoredForceTuple ExternalForce { get; set; } = new FactoredForceTuple(Guid.NewGuid());
        /// <inheritdoc/>
        public IBeamShearAxisAction SupportAction { get; set; } = new BeamShearAxisAction(Guid.NewGuid());

        public BeamShearAction(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            BeamShearAction newItem = new(Guid.NewGuid());
            updateStrategy ??= new BeamShearActionUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
