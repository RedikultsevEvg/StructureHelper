using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Forces
{
    public class DesignForceTuple : IDesignForceTuple
    {
        private IUpdateStrategy<IDesignForceTuple> updateStrategy = new DesignForceTupleUpdateStrategy();
        public Guid Id { get; }


        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IForceTuple ForceTuple { get; set; } = new ForceTuple();

        public DesignForceTuple(Guid id)
        {
            Id = id;
        }

        public DesignForceTuple() : this (Guid.NewGuid())
        {
        }

        public IDesignForceTuple GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            DesignForceTuple newTuple = new(id);
            updateStrategy.Update(newTuple, this);
            return newTuple;
        }
    }
}
