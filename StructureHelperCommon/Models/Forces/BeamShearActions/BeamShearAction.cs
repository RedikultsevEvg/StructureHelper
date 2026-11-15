using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
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

        public IBeamShearAction GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            BeamShearAction newItem = new(id);
            updateStrategy ??= new BeamShearActionUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
