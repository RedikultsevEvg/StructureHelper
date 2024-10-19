using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class PointPrimitiveUpdateStrategy : IUpdateStrategy<IPointNdmPrimitive>
    {
        static readonly BaseUpdateStrategy basePrimitiveUpdateStrategy = new();
        public void Update(IPointNdmPrimitive targetObject, IPointNdmPrimitive sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            basePrimitiveUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.Area = sourceObject.Area;
        }
    }
}
