using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces
{
    public class ForceActionUpdateStrategy : IUpdateStrategy<IForceAction>
    {
        private IUpdateStrategy<IForceAction> forceUpdateStrategy;
        private IUpdateStrategy<IDesignForcePair> pairUpdateStrategy;
        private IUpdateStrategy<IForceFactoredList> factorUpdateStrategy;
        private IUpdateStrategy<IForceCombinationList> forceListUpdateStrategy;
        private IUpdateStrategy<IForceCombinationFromFile> fileCombinationUpdateStrategy;
        private IUpdateStrategy<IForceAction> ForceUpdateStrategy => forceUpdateStrategy ??= new ForceActionBaseUpdateStrategy();
        private IUpdateStrategy<IDesignForcePair> PairUpdateStrategy => pairUpdateStrategy ??= new ForcePairUpdateStrategy();
        private IUpdateStrategy<IForceFactoredList> FactorUpdateStrategy => factorUpdateStrategy ??= new ForceFactoredListUpdateStrategy();
        private IUpdateStrategy<IForceCombinationList> ForceListUpdateStrategy => forceListUpdateStrategy ??= new ForceCombinationListUpdateStrategy();
        private IUpdateStrategy<IForceCombinationFromFile> FileCombinationUpdateStrategy => fileCombinationUpdateStrategy ??= new ForceCombinationFromFileUpdateStrategy();

        public ForceActionUpdateStrategy(
            IUpdateStrategy<IForceAction> forceActionUpdateStrategy,
            IUpdateStrategy<IDesignForcePair> forcePairUpdateStrategy,
            IUpdateStrategy<IForceFactoredList> factorUpdateStrategy,
            IUpdateStrategy<IForceCombinationList> forceListUpdateStrategy,
            IUpdateStrategy<IForceCombinationFromFile> fileCombinationUpdateStrategy)
        {
            this.forceUpdateStrategy = forceActionUpdateStrategy;
            this.pairUpdateStrategy = forcePairUpdateStrategy;
            this.factorUpdateStrategy = factorUpdateStrategy;
            this.forceListUpdateStrategy = forceListUpdateStrategy;
            this.fileCombinationUpdateStrategy = fileCombinationUpdateStrategy;
        }

        public ForceActionUpdateStrategy() 
        {
            
        }

        public void Update(IForceAction targetObject, IForceAction sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            ForceUpdateStrategy.Update(targetObject, sourceObject);
            UpdateChildProperties(targetObject, sourceObject);
        }

        private void UpdateChildProperties(IForceAction targetObject, IForceAction sourceObject)
        {
            if (sourceObject.GetType() != targetObject.GetType())
                {
                    throw new StructureHelperException(ErrorStrings.ExpectedWas(targetObject.GetType(), sourceObject.GetType() + $": source object type is not {targetObject.GetType()}"));
                }
            if (targetObject is IDesignForcePair pair)
            {
                PairUpdateStrategy.Update(pair, (IDesignForcePair)sourceObject);
            }
            else if (targetObject is IForceFactoredList combination)
            {
                FactorUpdateStrategy.Update(combination, (IForceFactoredList)sourceObject);
            }
            else if (targetObject is IForceCombinationList forceCombinationList)
            {
                ForceListUpdateStrategy.Update(forceCombinationList, (IForceCombinationList)sourceObject);
            }
            else if (targetObject is IForceCombinationFromFile fileCombination)
            {
                FileCombinationUpdateStrategy.Update(fileCombination, (IForceCombinationFromFile)sourceObject);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(IForceAction), targetObject.GetType());
            }
        }
    }
}
