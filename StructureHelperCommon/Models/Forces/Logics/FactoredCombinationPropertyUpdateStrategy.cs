using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class FactoredCombinationPropertyUpdateStrategy : IUpdateStrategy<IFactoredCombinationProperty>
    {
        public void Update(IFactoredCombinationProperty targetObject, IFactoredCombinationProperty sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.ULSFactor = sourceObject.ULSFactor;
            targetObject.LongTermFactor = sourceObject.LongTermFactor;
            targetObject.CalcTerm = sourceObject.CalcTerm;
            targetObject.LimitState = sourceObject.LimitState;
        }
    }
}
