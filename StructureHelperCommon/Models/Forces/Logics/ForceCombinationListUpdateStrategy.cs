using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceCombinationListUpdateStrategy : IUpdateStrategy<IForceCombinationList>
    {
        private IUpdateStrategy<IDesignForceTuple> designForceTupleUpdateStrategy;

        public ForceCombinationListUpdateStrategy(IUpdateStrategy<IDesignForceTuple> designForceTupleUpdateStrategy)
        {
            this.designForceTupleUpdateStrategy = designForceTupleUpdateStrategy;
        }

        public ForceCombinationListUpdateStrategy() : this (new DesignForceTupleUpdateStrategy())
        {
            
        }

        public void Update(IForceCombinationList targetObject, IForceCombinationList sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.DesignForces.Clear();
            foreach (var item in sourceObject.DesignForces)
            {
                targetObject.DesignForces.Add((IDesignForceTuple)item.Clone());
            }
        }
    }
}
