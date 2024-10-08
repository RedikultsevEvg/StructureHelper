using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class MaterialPartialFactorUpdateStrategy : IUpdateStrategy<IMaterialPartialFactor>
    {
        public void Update(IMaterialPartialFactor targetObject, IMaterialPartialFactor sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.LimitState = sourceObject.LimitState;
            targetObject.StressState = sourceObject.StressState;
            targetObject.CalcTerm = sourceObject.CalcTerm;
            targetObject.FactorValue = sourceObject.FactorValue;
        }
    }
}
