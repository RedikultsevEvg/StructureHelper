using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    internal class DivisionPropsUpdateStrategy : IUpdateStrategy<IDivisionSize>
    {
        public void Update(IDivisionSize targetObject, IDivisionSize sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.NdmMaxSize = sourceObject.NdmMaxSize;
            targetObject.NdmMinDivision = sourceObject.NdmMinDivision;
            targetObject.ClearUnderlying = sourceObject.ClearUnderlying;
        }
    }
}
