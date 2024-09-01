using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class HasForceCombinationUpdateStrategy : IUpdateStrategy<IHasForceCombinations>
    {
        public void Update(IHasForceCombinations targetObject, IHasForceCombinations sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject, "Interface IHasForceCombination");
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.ForceActions.Clear();
            targetObject.ForceActions.AddRange(sourceObject.ForceActions);
        }
    }
}
