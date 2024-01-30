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

namespace StructureHelperCommon.Models.Forces
{
    public class ForceActionUpdateStrategy : IUpdateStrategy<IForceAction>
    {
        private readonly IUpdateStrategy<IPoint2D> pointStrategy = new Point2DUpdateStrategy();
        private readonly IUpdateStrategy<IDesignForcePair> forcePairUpdateStrategy = new ForcePairUpdateStrategy();
        private readonly IUpdateStrategy<IForceCombinationByFactor> factorUpdateStrategy = new FactorCombinationUpdateStrategy();
        private readonly IUpdateStrategy<IForceCombinationList> forceListUpdateStrategy = new ForceCombinationListUpdateStrategy();
        public void Update(IForceAction targetObject, IForceAction sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);
            targetObject.SetInGravityCenter = sourceObject.SetInGravityCenter;
            pointStrategy.Update(targetObject.ForcePoint, sourceObject.ForcePoint);
            UpdateChildProperties(targetObject, sourceObject);
        }

        private void UpdateChildProperties(IForceAction targetObject, IForceAction sourceObject)
        {
            if (targetObject is IDesignForcePair pair)
            {
                forcePairUpdateStrategy.Update(pair, (IDesignForcePair)sourceObject);
            }
            else if (targetObject is IForceCombinationByFactor combination)
            {
                factorUpdateStrategy.Update(combination, (IForceCombinationByFactor)sourceObject);
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
