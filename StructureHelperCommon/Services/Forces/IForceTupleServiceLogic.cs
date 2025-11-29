using StructureHelperCommon.Models.Forces;
using System.Collections.Generic;

namespace StructureHelperCommon.Services.Forces
{
    public interface IForceTupleServiceLogic
    {
        void CopyProperties(IForceTuple source, IForceTuple target, double factor = 1);
        List<IDesignForceTuple> InterpolateDesignTuple(IDesignForceTuple startDesignForce, IDesignForceTuple finishDesignForce, int stepCount = 10);
        IForceTuple InterpolateTuples(IForceTuple startTuple, IForceTuple endTuple, double coefficient = 0.5);
        List<IForceTuple> InterpolateTuples(IForceTuple startTuple, IForceTuple endTuple, int stepCount);
        IForceTuple MergeTupleCollection(IEnumerable<IForceTuple> tupleCollection);
        IForceTuple MultiplyTupleByFactor(IForceTuple forceTuple, double factor);
        IForceTuple SumTuples(IForceTuple first, IForceTuple second, double factor = 1);
        void SumTupleToTarget(IForceTuple source, IForceTuple target, double factor = 1);
    }
}