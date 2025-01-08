using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes.Logics;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces.Logics;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceActionUpdateStrategy : IUpdateStrategy<IForceAction>
    {
        private readonly IUpdateStrategy<IForceAction> forceActionUpdateStrategy;
        private readonly IUpdateStrategy<IDesignForcePair> forcePairUpdateStrategy;
        private readonly IUpdateStrategy<IForceFactoredList> factorUpdateStrategy;
        private readonly IUpdateStrategy<IForceCombinationList> forceListUpdateStrategy;

        public ForceActionUpdateStrategy(
            IUpdateStrategy<IForceAction> forceActionUpdateStrategy,
            IUpdateStrategy<IDesignForcePair> forcePairUpdateStrategy,
            IUpdateStrategy<IForceFactoredList> factorUpdateStrategy,
            IUpdateStrategy<IForceCombinationList> forceListUpdateStrategy)
        {
            this.forceActionUpdateStrategy = forceActionUpdateStrategy;
            this.forcePairUpdateStrategy = forcePairUpdateStrategy;
            this.factorUpdateStrategy = factorUpdateStrategy;
            this.forceListUpdateStrategy = forceListUpdateStrategy;
        }

        public ForceActionUpdateStrategy() : this(
             new ForceActionBaseUpdateStrategy(),
             new ForcePairUpdateStrategy(),
             new ForceFactoredListUpdateStrategy(),
             new ForceCombinationListUpdateStrategy()
             )
        {
            
        }

        public void Update(IForceAction targetObject, IForceAction sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            forceActionUpdateStrategy.Update(targetObject, sourceObject);
            UpdateChildProperties(targetObject, sourceObject);
        }

        private void UpdateChildProperties(IForceAction targetObject, IForceAction sourceObject)
        {
            if (targetObject is IDesignForcePair pair)
            {
                forcePairUpdateStrategy.Update(pair, (IDesignForcePair)sourceObject);
            }
            else if (targetObject is IForceFactoredList combination)
            {
                factorUpdateStrategy.Update(combination, (IForceFactoredList)sourceObject);
            }
            else if (targetObject is IForceCombinationList forceCombinationList)
            {
                forceListUpdateStrategy.Update(forceCombinationList, (IForceCombinationList)sourceObject);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(IForceAction), targetObject.GetType());
            }
        }
    }
}
