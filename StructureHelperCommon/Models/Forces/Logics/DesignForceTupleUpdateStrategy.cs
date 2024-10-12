using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class DesignForceTupleUpdateStrategy : IUpdateStrategy<IDesignForceTuple>
    {
        private IUpdateStrategy<IForceTuple> forceTupleUpdateStrategy;

        public DesignForceTupleUpdateStrategy(IUpdateStrategy<IForceTuple> forceTupleUpdateStrategy)
        {
            this.forceTupleUpdateStrategy = forceTupleUpdateStrategy;
        }

        public DesignForceTupleUpdateStrategy() : this (new ForceTupleUpdateStrategy())
        {
            
        }

        public void Update(IDesignForceTuple targetObject, IDesignForceTuple sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.LimitState = sourceObject.LimitState;
            targetObject.CalcTerm = sourceObject.CalcTerm;
            if (sourceObject.ForceTuple is not null)
            {
                forceTupleUpdateStrategy.Update(targetObject.ForceTuple, sourceObject.ForceTuple);
            }
        }
    }
}
