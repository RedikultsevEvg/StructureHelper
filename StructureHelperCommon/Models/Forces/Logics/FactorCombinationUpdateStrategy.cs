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
        private IUpdateStrategy<IForceTuple> tupleUpdateStrategy;
        public FactorCombinationUpdateStrategy(IUpdateStrategy<IForceTuple> tupleUpdateStrategy)
        {
            this.tupleUpdateStrategy = tupleUpdateStrategy;
        }
        public FactorCombinationUpdateStrategy() : this(new ForceTupleUpdateStrategy())
        {
            
        }
        public void Update(IForceCombinationByFactor targetObject, IForceCombinationByFactor sourceObject)
        {
            CheckObject.CompareTypes(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            tupleUpdateStrategy.Update(targetObject.FullSLSForces, sourceObject.FullSLSForces);
            targetObject.ULSFactor = sourceObject.ULSFactor;
            targetObject.LongTermFactor = sourceObject.LongTermFactor;
        }
    }
}
