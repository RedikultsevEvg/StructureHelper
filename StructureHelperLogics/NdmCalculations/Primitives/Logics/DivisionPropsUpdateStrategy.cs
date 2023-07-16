using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    internal class DivisionPropsUpdateStrategy : IUpdateStrategy<IHasDivisionSize>
    {
        public void Update(IHasDivisionSize targetObject, IHasDivisionSize sourceObject)
        {
            targetObject.NdmMaxSize = sourceObject.NdmMaxSize;
            targetObject.NdmMinDivision = sourceObject.NdmMinDivision;
            targetObject.ClearUnderlying = sourceObject.ClearUnderlying;
        }
    }
}
