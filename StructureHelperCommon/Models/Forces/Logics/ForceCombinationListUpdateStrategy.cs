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
            CheckObject.CompareTypes(targetObject, sourceObject);
            var forcesList = new List<IDesignForceTuple>(sourceObject.DesignForces);
            targetObject.DesignForces.Clear();
            foreach (var item in forcesList)
            {
                targetObject.DesignForces.Add((IDesignForceTuple)item.Clone());
            }
        }
    }
}
