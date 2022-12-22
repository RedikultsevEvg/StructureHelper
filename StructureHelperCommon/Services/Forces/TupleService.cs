using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Forces
{
    public static class TupleService
    {
        public static IForceTuple MoveTupleIntoPoint(IForceTuple forceTuple, IPoint2D point2D)
        {
            var newTuple = forceTuple.Clone() as IForceTuple;
            newTuple.Mx += newTuple.Nz * point2D.Y;
            newTuple.My -= newTuple.Nz * point2D.X;
            return newTuple;
        }

        public static IForceTuple InterpolateTuples(IForceTuple startTuple, IForceTuple endTuple, double coefficient)
        {
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

        public static IForceTuple InterpolateTuples(IForceTuple endTuple, double coefficient)
        {
            IForceTuple startTuple = new ForceTuple();
            return InterpolateTuples(startTuple, endTuple, coefficient);
        }

        public static List<IDesignForceTuple> InterpolateDesignTuple(IDesignForceTuple endTuple, int stepCount)
        {
            var tuples =new List<IDesignForceTuple>();
            double step = 1d / stepCount;
            for (int i = 0; i <= stepCount; i++)
            {
                var currentTuple = InterpolateTuples(endTuple.ForceTuple, i * step);
                var currentDesignTuple = new DesignForceTuple() { LimitState = endTuple.LimitState, CalcTerm = endTuple.CalcTerm, ForceTuple = currentTuple };
                tuples.Add(currentDesignTuple);
            }
            return tuples;
        }
    }
}
