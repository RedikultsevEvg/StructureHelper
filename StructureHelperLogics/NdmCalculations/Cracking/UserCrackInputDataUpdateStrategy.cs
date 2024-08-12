using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    internal class UserCrackInputDataUpdateStrategy : IUpdateStrategy<IUserCrackInputData>
    {
        public void Update(IUserCrackInputData targetObject, IUserCrackInputData sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);

            targetObject.SetSofteningFactor = sourceObject.SetSofteningFactor;
            targetObject.SofteningFactor = sourceObject.SofteningFactor;
            targetObject.SetLengthBetweenCracks = sourceObject.SetLengthBetweenCracks;
            targetObject.LengthBetweenCracks = sourceObject.LengthBetweenCracks;
        }
    }
}
