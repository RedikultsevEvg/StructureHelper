using StructureHelperCommon.Infrastructures.Interfaces;
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
        public void Update(IForceCombinationList targetObject, IForceCombinationList sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);
            targetObject.DesignForces.Clear();
            foreach (var item in sourceObject.DesignForces)
            {
                targetObject.DesignForces.Add((IDesignForceTuple)item.Clone());
            }
        }
    }
}
