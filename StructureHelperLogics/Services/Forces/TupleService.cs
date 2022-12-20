using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.Forces
{
    internal static class TupleService
    {
        public static IForceTuple MoveTupleIntoPoint(IForceTuple forceTuple, IPoint2D point2D)
        {
            var newTuple = forceTuple.Clone() as IForceTuple;
            newTuple.Mx += newTuple.Nz * point2D.Y;
            newTuple.My -= newTuple.Nz * point2D.X;
            return newTuple;
        }
    }
}
