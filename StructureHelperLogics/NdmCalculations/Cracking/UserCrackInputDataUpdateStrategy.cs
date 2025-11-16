using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class UserCrackInputDataUpdateStrategy : IUpdateStrategy<IUserCrackInputData>
    {
        public void Update(IUserCrackInputData targetObject, IUserCrackInputData sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.SetSofteningFactor = sourceObject.SetSofteningFactor;
            targetObject.SofteningFactor = sourceObject.SofteningFactor;
            targetObject.SetLengthBetweenCracks = sourceObject.SetLengthBetweenCracks;
            targetObject.LengthBetweenCracks = sourceObject.LengthBetweenCracks;
            targetObject.UltimateLongCrackWidth = sourceObject.UltimateLongCrackWidth;
            targetObject.UltimateShortCrackWidth = sourceObject.UltimateShortCrackWidth;
        }
    }
}
