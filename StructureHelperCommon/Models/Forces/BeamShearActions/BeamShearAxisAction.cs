using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    /// <inheritdoc/>
    public class BeamShearAxisAction : IBeamShearAxisAction
    {
        private IUpdateStrategy<IBeamShearAxisAction> updateStrategy;
        ///<inheritdoc/>
        public Guid Id { get; }
        ///<inheritdoc/>
        public string Name { get; set; } = string.Empty;
        ///<inheritdoc/>
        public IFactoredForceTuple SupportForce { get; set; } = new FactoredForceTuple(Guid.NewGuid());
        ///<inheritdoc/>
        public List<IBeamSpanLoad> ShearLoads { get; } = new();

        public BeamShearAxisAction(Guid id)
        {
            Id = id;
        }

        public IBeamShearAxisAction GetClone(IBeamShearAxisAction sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            BeamShearAxisAction newItem = new(id);
            updateStrategy ??= new BeamShearAxisActionUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
