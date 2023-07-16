using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    internal class RebarUpdateStrategy : IUpdateStrategy<RebarPrimitive>
    {
        static readonly BaseUpdateStrategy basePrimitiveUpdateStrategy = new();
        public void Update(RebarPrimitive targetObject, RebarPrimitive sourceObject)
        {
            basePrimitiveUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.Area = sourceObject.Area;
            targetObject.HostPrimitive = sourceObject.HostPrimitive;
        }
    }
}
