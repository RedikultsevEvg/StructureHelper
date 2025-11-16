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
    public class ForceFactoredListUpdateStrategy : IUpdateStrategy<IForceFactoredList>
    {
        private IUpdateStrategy<IForceAction> forceActionUpdateStrategy;
        private IUpdateStrategy<IForceTuple> tupleUpdateStrategy;
        private IUpdateStrategy<IFactoredCombinationProperty> propertyUpdateStrategy;
        public ForceFactoredListUpdateStrategy
            (
            IUpdateStrategy<IForceAction> forceActionUpdateStrategy,
            IUpdateStrategy<IForceTuple> tupleUpdateStrategy,
            IUpdateStrategy<IFactoredCombinationProperty> propertyUpdateStrategy)
        {
            this.forceActionUpdateStrategy = forceActionUpdateStrategy;
            this.tupleUpdateStrategy = tupleUpdateStrategy;
            this.propertyUpdateStrategy = propertyUpdateStrategy;
        }
        public ForceFactoredListUpdateStrategy() : this
            (
            new ForceActionBaseUpdateStrategy(),
            new ForceTupleUpdateStrategy(),
            new FactoredCombinationPropertyUpdateStrategy())
        {
            
        }
        public void Update(IForceFactoredList targetObject, IForceFactoredList sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            forceActionUpdateStrategy.Update(targetObject, sourceObject);
            CheckObject.ThrowIfNull(sourceObject.CombinationProperty);
            CheckObject.ThrowIfNull(targetObject.CombinationProperty);
            propertyUpdateStrategy.Update(targetObject.CombinationProperty, sourceObject.CombinationProperty);
            CheckObject.ThrowIfNull(sourceObject.ForceTuples);
            CheckObject.ThrowIfNull(targetObject.ForceTuples);
            targetObject.ForceTuples.Clear();
            foreach (var item in sourceObject.ForceTuples)
            {
                ForceTuple newTuple = new();
                tupleUpdateStrategy.Update(newTuple, item);
                targetObject.ForceTuples.Add(newTuple);
            }
        }
    }
}
