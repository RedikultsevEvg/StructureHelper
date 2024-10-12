using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceCombinationByFactorUpdateStrategy : IUpdateStrategy<IForceCombinationByFactor>
    {
        private IUpdateStrategy<IForceTuple> tupleUpdateStrategy;
        public ForceCombinationByFactorUpdateStrategy(IUpdateStrategy<IForceTuple> tupleUpdateStrategy)
        {
            this.tupleUpdateStrategy = tupleUpdateStrategy;
        }
        public ForceCombinationByFactorUpdateStrategy() : this(new ForceTupleUpdateStrategy())
        {
            
        }
        public void Update(IForceCombinationByFactor targetObject, IForceCombinationByFactor sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            tupleUpdateStrategy.Update(targetObject.FullSLSForces, sourceObject.FullSLSForces);
            targetObject.ULSFactor = sourceObject.ULSFactor;
            targetObject.LongTermFactor = sourceObject.LongTermFactor;
        }
    }
}
