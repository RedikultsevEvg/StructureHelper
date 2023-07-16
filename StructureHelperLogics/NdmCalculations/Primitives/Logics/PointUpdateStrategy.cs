using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    internal class PointUpdateStrategy : IUpdateStrategy<PointPrimitive>
    {
        static readonly BaseUpdateStrategy basePrimitiveUpdateStrategy = new();
        public void Update(PointPrimitive targetObject, PointPrimitive sourceObject)
        {
            basePrimitiveUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.Area = sourceObject.Area;
        }
    }
}
