using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.Logics;
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

        public object Clone()
        {
            var newTuple = new DesignForceTuple();
            updateStrategy.Update(newTuple, this);
            return newTuple;
        }
    }
}
