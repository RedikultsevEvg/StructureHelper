using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
