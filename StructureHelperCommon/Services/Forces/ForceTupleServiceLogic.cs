using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Services.Forces
{
    public class ForceTupleServiceLogic : IForceTupleServiceLogic
    {
        /// <summary>
        /// Copy properties from target to source
        /// </summary>
        /// <param name="source">Source tuple</param>
        /// <param name="target">Target tuple</param>
        /// <param name="factor">factor</param>
        public void CopyProperties(IForceTuple source, IForceTuple target, double factor = 1d)
        {
            CheckTuples(source, target);
            target.Clear();
            SumTupleToTarget(source, target, factor);
        }
        public IForceTuple SumTuples(IForceTuple first, IForceTuple second, double factor = 1d)
        {
            CheckTuples(first, second);
            IForceTuple result = GetNewTupleSameType(first);
            SumTupleToTarget(first, result, 1d);
            SumTupleToTarget(second, result, factor);
            return result;
        }
        public IForceTuple MergeTupleCollection(IEnumerable<IForceTuple> tupleCollection)
        {
            CheckTupleCollection(tupleCollection);
            var result = GetNewTupleSameType(tupleCollection.First());
            foreach (var item in tupleCollection)
            {
                result = SumTuples(result, item);
            }
            ;
            return result;
        }
        /// <summary>
        /// Multyplies force tuple by factor
        /// </summary>
        /// <param name="forceTuple">Source force tuple</param>
        /// <param name="factor">Factor which tuple multyplies by</param>
        /// <returns></returns>
        public IForceTuple MultiplyTupleByFactor(IForceTuple forceTuple, double factor)
        {
            var result = GetNewTupleSameType(forceTuple);
            CopyProperties(forceTuple, result, factor);
            return result;
        }
        public IForceTuple InterpolateTuples(IForceTuple startTuple, IForceTuple endTuple, double coefficient = 0.5d)
        {
            if (startTuple is null) startTuple = GetNewTupleSameType(endTuple);
            else { CheckTuples(startTuple, endTuple); }
            var deltaTuple = SumTuples(endTuple, startTuple, -1d);
            return SumTuples(startTuple, deltaTuple, coefficient);
        }

        public List<IForceTuple> InterpolateTuples(IForceTuple startTuple, IForceTuple endTuple, int stepCount)
        {
            var tuples = new List<IForceTuple>();
            double step = 1d / stepCount;
            for (int i = 0; i <= stepCount; i++)
            {
                var interpolatedTuple = InterpolateTuples(startTuple, endTuple, i * step);
                tuples.Add(interpolatedTuple);
            }
            return tuples;
        }

        public List<IDesignForceTuple> InterpolateDesignTuple(IDesignForceTuple startDesignForce, IDesignForceTuple finishDesignForce, int stepCount = 10)
        {
            if (startDesignForce.LimitState != finishDesignForce.LimitState) throw new StructureHelperException(ErrorStrings.LimitStatesIsNotValid);
            if (startDesignForce.CalcTerm != finishDesignForce.CalcTerm) throw new StructureHelperException(ErrorStrings.LoadTermIsNotValid);
            var tuples = new List<IDesignForceTuple>();
            double step = 1d / stepCount;
            for (int i = 0; i <= stepCount; i++)
            {
                var currentTuple = InterpolateTuples(startDesignForce.ForceTuple, finishDesignForce.ForceTuple, i * step);
                var currentDesignTuple = new DesignForceTuple()
                {
                    LimitState = finishDesignForce.LimitState,
                    CalcTerm = finishDesignForce.CalcTerm,
                    ForceTuple = currentTuple
                };
                tuples.Add(currentDesignTuple);
            }
            return tuples;
        }
        /// <summary>
        /// Renew target force tuple with adding source tuple
        /// </summary>
        /// <param name="source">Source tuple</param>
        /// <param name="target">Target tuple</param>
        /// <param name="factor">Factor which source tuple will be multiplied by (1d is default value)</param>
        public void SumTupleToTarget(IForceTuple source, IForceTuple target, double factor = 1d)
        {
            target.Mx += source.Mx * factor;
            target.My += source.My * factor;
            target.Nz += source.Nz * factor;
            target.Qx += source.Qx * factor;
            target.Qy += source.Qy * factor;
            target.Mz += source.Mz * factor;
        }
        private void CheckTuples(IForceTuple first, IForceTuple second)
        {
            if (first.GetType() != second.GetType())
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect +
                    $": Type of first parameter (type = {first.GetType()}) doesn't corespond second parameter type ({second.GetType()})");
            }
        }
        private void CheckTupleCollection(IEnumerable<IForceTuple> tupleCollection)
        {
            if (tupleCollection.Count() == 0)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Collection is Empty");
            }
            foreach (var item in tupleCollection)
            {
                CheckTuples(tupleCollection.First(), item);
            }
        }
        private IForceTuple GetNewTupleSameType(IForceTuple first)
        {
            IForceTuple result;
            if (first is ForceTuple) { result = new ForceTuple(); }
            else if (first is StrainTuple) { result = new StrainTuple(); }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
            }
            return result;
        }
    }
}
