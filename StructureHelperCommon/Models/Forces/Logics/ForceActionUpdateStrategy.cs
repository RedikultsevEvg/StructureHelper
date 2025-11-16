using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces
{
    public class ForceActionUpdateStrategy : IUpdateStrategy<IForceAction>
    {
        private readonly IUpdateStrategy<IForceAction> forceActionUpdateStrategy;
        private readonly IUpdateStrategy<IDesignForcePair> forcePairUpdateStrategy;
        private readonly IUpdateStrategy<IForceFactoredList> factorUpdateStrategy;
        private readonly IUpdateStrategy<IForceCombinationList> forceListUpdateStrategy;
        private readonly IUpdateStrategy<IForceCombinationFromFile> fileCombinationUpdateStrategy;

        public ForceActionUpdateStrategy(
            IUpdateStrategy<IForceAction> forceActionUpdateStrategy,
            IUpdateStrategy<IDesignForcePair> forcePairUpdateStrategy,
            IUpdateStrategy<IForceFactoredList> factorUpdateStrategy,
            IUpdateStrategy<IForceCombinationList> forceListUpdateStrategy,
            IUpdateStrategy<IForceCombinationFromFile> fileCombinationUpdateStrategy)
        {
            this.forceActionUpdateStrategy = forceActionUpdateStrategy;
            this.forcePairUpdateStrategy = forcePairUpdateStrategy;
            this.factorUpdateStrategy = factorUpdateStrategy;
            this.forceListUpdateStrategy = forceListUpdateStrategy;
            this.fileCombinationUpdateStrategy = fileCombinationUpdateStrategy;
        }

        public ForceActionUpdateStrategy() : this(
             new ForceActionBaseUpdateStrategy(),
             new ForcePairUpdateStrategy(),
             new ForceFactoredListUpdateStrategy(),
             new ForceCombinationListUpdateStrategy(),
             new ForceCombinationFromFileUpdateStrategy()
             )
        {
            
        }

        public void Update(IForceAction targetObject, IForceAction sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
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
            else if (targetObject is IForceCombinationFromFile fileCombination)
            {
                fileCombinationUpdateStrategy.Update(fileCombination, (IForceCombinationFromFile)sourceObject);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(IForceAction), targetObject.GetType());
            }
        }
    }
}
