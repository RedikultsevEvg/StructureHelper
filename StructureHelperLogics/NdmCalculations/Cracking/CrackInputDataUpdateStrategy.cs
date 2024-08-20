using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackInputDataUpdateStrategy : IUpdateStrategy<ICrackCalculatorInputData>
    {
        private IUpdateStrategy<IUserCrackInputData> userCrackInputDataUpdateStrategy;
        public CrackInputDataUpdateStrategy(IUpdateStrategy<IUserCrackInputData> userCrackInputDataUpdateStrategy)
        {
            this.userCrackInputDataUpdateStrategy = userCrackInputDataUpdateStrategy;
        }

        public CrackInputDataUpdateStrategy() : this(new UserCrackInputDataUpdateStrategy())
        {
            
        }
        public void Update(ICrackCalculatorInputData targetObject, ICrackCalculatorInputData sourceObject)
        {
            CheckObject.CompareTypes(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.ForceActions.Clear();
            targetObject.ForceActions.AddRange(sourceObject.ForceActions);
            targetObject.Primitives.Clear();
            targetObject.Primitives.AddRange(sourceObject.Primitives);
            targetObject.UserCrackInputData ??= new UserCrackInputData();
            userCrackInputDataUpdateStrategy.Update(targetObject.UserCrackInputData, sourceObject.UserCrackInputData);
        }
    }
}
