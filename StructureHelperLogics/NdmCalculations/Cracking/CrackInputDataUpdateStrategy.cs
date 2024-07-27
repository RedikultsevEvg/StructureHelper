using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackInputDataUpdateStrategy : IUpdateStrategy<CrackInputData>
    {
        private IUpdateStrategy<UserCrackInputData> userCrackInputDataUpdateStrategy;
        public CrackInputDataUpdateStrategy(IUpdateStrategy<UserCrackInputData> userCrackInputDataUpdateStrategy)
        {
            this.userCrackInputDataUpdateStrategy = userCrackInputDataUpdateStrategy;
        }

        public CrackInputDataUpdateStrategy() : this(new UserCrackInputDataUpdateStrategy())
        {
            
        }
        public void Update(CrackInputData targetObject, CrackInputData sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);
            targetObject.ForceActions.Clear();
            targetObject.ForceActions.AddRange(sourceObject.ForceActions);
            targetObject.Primitives.Clear();
            targetObject.Primitives.AddRange(sourceObject.Primitives);
            targetObject.UserCrackInputData ??= new UserCrackInputData();
            userCrackInputDataUpdateStrategy.Update(targetObject.UserCrackInputData, sourceObject.UserCrackInputData);
        }
    }
}
