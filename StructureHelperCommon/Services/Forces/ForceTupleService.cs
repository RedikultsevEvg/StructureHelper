using System.Collections.Generic;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Services.Forces
{
    public static class ForceTupleService
    {
        public static IForceTuple MoveTupleIntoPoint(IForceTuple forceTuple, IPoint2D point2D)
        {
            var newTuple = forceTuple.Clone() as IForceTuple;
            newTuple.Mx += newTuple.Nz * point2D.Y;
            newTuple.My -= newTuple.Nz * point2D.X;
            return newTuple;
        }
        public static IForceTuple SumTuples(IForceTuple first, IForceTuple second)
        {
            var result = new ForceTuple();
            result.Mx += first.Mx + second.Mx;
            result.My += first.My + second.My;
            result.Mz += first.Mz + second.Mz;
            result.Qx += first.Qx + second.Qx;
            result.Qy += first.Qy + second.Qy;
            result.Nz += first.Nz + second.Nz;
            return result;
        }
        public static IForceTuple MultiplyTuples(IForceTuple first, double factor)
        {
            var result = new ForceTuple();
            result.Mx += first.Mx * factor;
            result.My += first.My * factor;
            result.Mz += first.Mz * factor;
            result.Qx += first.Qx * factor;
            result.Qy += first.Qy * factor;
            result.Nz += first.Nz * factor;
            return result;
        }
        public static IForceTuple InterpolateTuples(IForceTuple endTuple, IForceTuple startTuple = null, double coefficient = 0.5d)
        {
            if (startTuple == null) startTuple = new ForceTuple();
            double dMx, dMy, dNz;
            dMx = endTuple.Mx - startTuple.Mx;
            dMy = endTuple.My - startTuple.My;
            dNz = endTuple.Nz - startTuple.Nz;
            return new ForceTuple()
            {
                Mx = startTuple.Mx + dMx * coefficient,
                My = startTuple.My + dMy * coefficient,
                Nz = startTuple.Nz + dNz * coefficient
            };
        }


        public static List<IDesignForceTuple> InterpolateDesignTuple(IDesignForceTuple finishDesignForce, IDesignForceTuple startDesignForce = null, int stepCount = 10)
        {
            if (startDesignForce.LimitState != finishDesignForce.LimitState) throw new StructureHelperException(ErrorStrings.LimitStatesIsNotValid);
            if (startDesignForce.CalcTerm != finishDesignForce.CalcTerm) throw new StructureHelperException(ErrorStrings.LoadTermIsNotValid);
            var tuples =new List<IDesignForceTuple>();
            double step = 1d / stepCount;
            for (int i = 0; i <= stepCount; i++)
            {
                var currentTuple = InterpolateTuples(finishDesignForce.ForceTuple, startDesignForce.ForceTuple, i * step);
                var currentDesignTuple = new DesignForceTuple() { LimitState = finishDesignForce.LimitState, CalcTerm = finishDesignForce.CalcTerm, ForceTuple = currentTuple };
                tuples.Add(currentDesignTuple);
            }
            return tuples;
        }
    }
}
