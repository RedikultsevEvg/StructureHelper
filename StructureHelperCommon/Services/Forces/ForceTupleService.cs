using System.Collections.Generic;
using System.Linq;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Services.Forces
{
    public static class ForceTupleService
    {
        /// <summary>
        /// Copy properties from target to source
        /// </summary>
        /// <param name="source">Source tuple</param>
        /// <param name="target">Target tuple</param>
        /// <param name="factor">factor</param>
        public static void CopyProperties(IForceTuple source, IForceTuple target, double factor = 1d)
        {
            CheckTuples(source, target);
            target.Clear();
            SumTupleToTarget(source, target, factor);
        }
        public static IForceTuple MoveTupleIntoPoint(IForceTuple forceTuple, IPoint2D point2D)
        {
            var newTuple = forceTuple.Clone() as IForceTuple;
            newTuple.Mx += newTuple.Nz * point2D.Y;
            newTuple.My -= newTuple.Nz * point2D.X;
            return newTuple;
        }
        public static IForceTuple SumTuples(IForceTuple first, IForceTuple second, double factor = 1d)
        {
            CheckTuples(first, second);
            IForceTuple result = GetNewTupleSameType(first);
            SumTupleToTarget(first, result, 1d);
            SumTupleToTarget(second, result, factor);
            return result;
        }
        public static IForceTuple MergeTupleCollection(IEnumerable<IForceTuple> tupleCollection)
        {
            CheckTupleCollection(tupleCollection);
            var result = GetNewTupleSameType(tupleCollection.First());
            foreach (var item in tupleCollection)
            {
                SumTuples(result, item);
            };
            return result;
        }
        public static IForceTuple MultiplyTuples(IForceTuple first, double factor)
        {
            var result = GetNewTupleSameType(first);
            CopyProperties(first, result, factor);
            return result;
        }
        public static IForceTuple InterpolateTuples(IForceTuple endTuple, IForceTuple startTuple = null, double coefficient = 0.5d)
        {
            if (startTuple is null) startTuple = GetNewTupleSameType(endTuple);
            else { CheckTuples(startTuple, endTuple); }
            var deltaTuple = SumTuples(endTuple, startTuple, -1d);
            return SumTuples(startTuple, deltaTuple, coefficient);
        }
        public static List<IDesignForceTuple> InterpolateDesignTuple(IDesignForceTuple finishDesignForce, IDesignForceTuple startDesignForce = null, int stepCount = 10)
        {
            if (startDesignForce.LimitState != finishDesignForce.LimitState) throw new StructureHelperException(ErrorStrings.LimitStatesIsNotValid);
            if (startDesignForce.CalcTerm != finishDesignForce.CalcTerm) throw new StructureHelperException(ErrorStrings.LoadTermIsNotValid);
            var tuples =new List<IDesignForceTuple>();
            double step = 1d / stepCount;
            for (int i = 0; i <= stepCount; i++)
            {
                var currentTuple = InterpolateTuples(finishDesignForce.ForceTuple, startDesignForce.ForceTuple, i * step) as ForceTuple;
                var currentDesignTuple = new DesignForceTuple() { LimitState = finishDesignForce.LimitState, CalcTerm = finishDesignForce.CalcTerm, ForceTuple = currentTuple };
                tuples.Add(currentDesignTuple);
            }
            return tuples;
        }
        public static void SumTupleToTarget(IForceTuple source, IForceTuple target, double factor = 1d)
        {
            target.Mx += source.Mx * factor;
            target.My += source.My * factor;
            target.Nz += source.Nz * factor;
            target.Qx += source.Qx * factor;
            target.Qy += source.Qy * factor;
            target.Mz += source.Mz * factor;
        }
        private static void CheckTuples(IForceTuple first, IForceTuple second)
        {
            if (first.GetType() != second.GetType())
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect +
                    $": Type of first parameter (type = {first.GetType()}) doesn't corespond second parameter type ({second.GetType()})");
            }
        }
        private static void CheckTupleCollection(IEnumerable<IForceTuple> tupleCollection)
        {
            if (tupleCollection.Count() == 0)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect +  $": Collection is Empty");
            }
            foreach (var item in tupleCollection)
            {
                CheckTuples(tupleCollection.First(), item);
            }
        }
        private static IForceTuple GetNewTupleSameType(IForceTuple first)
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
