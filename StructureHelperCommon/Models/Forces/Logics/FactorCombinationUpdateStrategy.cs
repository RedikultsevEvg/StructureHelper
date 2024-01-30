using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class FactorCombinationUpdateStrategy : IUpdateStrategy<IForceCombinationByFactor>
    {
        readonly IUpdateStrategy<IForceTuple> tupleUpdateStrategy = new ForceTupleUpdateStrategy();
        public void Update(IForceCombinationByFactor targetObject, IForceCombinationByFactor sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);
            tupleUpdateStrategy.Update(targetObject.FullSLSForces, sourceObject.FullSLSForces);
            targetObject.ULSFactor = sourceObject.ULSFactor;
            targetObject.LongTermFactor = sourceObject.LongTermFactor;
        }
    }
}
