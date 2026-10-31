using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class FactoredCombinationPropertyUpdateStrategy : IUpdateStrategy<IFactoredCombinationProperty>
    {
        public void Update(IFactoredCombinationProperty targetObject, IFactoredCombinationProperty sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.LimitState = sourceObject.LimitState;
            targetObject.ULSFactor = sourceObject.ULSFactor;
            targetObject.LongTermFactor = sourceObject.LongTermFactor;
            targetObject.CalcTerm = sourceObject.CalcTerm;
        }
    }
}
