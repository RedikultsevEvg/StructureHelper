using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Shapes.Logics;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForcePairUpdateStrategy : IUpdateStrategy<IDesignForcePair>
    {
        private readonly IUpdateStrategy<IForceTuple> tupleUpdateStrategy = new ForceTupleUpdateStrategy();
        public void Update(IDesignForcePair targetObject, IDesignForcePair sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);
            targetObject.LimitState = sourceObject.LimitState;
            tupleUpdateStrategy.Update(targetObject.LongForceTuple, sourceObject.LongForceTuple);
            tupleUpdateStrategy.Update(targetObject.FullForceTuple, sourceObject.FullForceTuple);
        }
    }
}
