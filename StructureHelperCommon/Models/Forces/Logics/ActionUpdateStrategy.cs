using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ActionUpdateStrategy : IUpdateStrategy<IAction>
    {
        readonly IUpdateStrategy<IForceAction> forceUpdateStrategy = new ForceActionUpdateStrategy();
        public void Update(IAction targetObject, IAction sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            if (targetObject is IForceAction forceAction)
            {
                forceUpdateStrategy.Update(forceAction, (IForceAction)sourceObject);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(IAction), sourceObject.GetType());
            }
        }
    }
}
