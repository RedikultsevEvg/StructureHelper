using StructureHelperCommon.Infrastructures.Interfaces;
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

        public object Clone()
        {
            BeamShearAxisAction beamShearAxisAction = new(Guid.NewGuid());
            updateStrategy ??= new BeamShearAxisActionUpdateStrategy();
            updateStrategy.Update(beamShearAxisAction, this);
            return beamShearAxisAction;
        }
    }
}
