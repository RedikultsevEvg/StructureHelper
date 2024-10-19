using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class RebarNdmPrimitiveUpdateStrategy : IUpdateStrategy<IRebarNdmPrimitive>
    {
        static readonly BaseUpdateStrategy basePrimitiveUpdateStrategy = new();
        public void Update(IRebarNdmPrimitive targetObject, IRebarNdmPrimitive sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (sourceObject.HostPrimitive is not null)
            basePrimitiveUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.Area = sourceObject.Area;
            targetObject.HostPrimitive = sourceObject.HostPrimitive;
        }
    }
}
