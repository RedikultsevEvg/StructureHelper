using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class BeamShearLoadBaseUpdateStrategy : IUpdateStrategy<IBeamSpanLoad>
    {
        private IUpdateStrategy<IFactoredCombinationProperty> combinationUpdateStrategy;
        public void Update(IBeamSpanLoad targetObject, IBeamSpanLoad sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.LoadRatio = sourceObject.LoadRatio;
            targetObject.RelativeLoadLevel = sourceObject.RelativeLoadLevel;
            CheckObject.ThrowIfNull(sourceObject.CombinationProperty);
            CheckObject.ThrowIfNull(targetObject.CombinationProperty);
            combinationUpdateStrategy ??= new FactoredCombinationPropertyUpdateStrategy();
            combinationUpdateStrategy.Update(targetObject.CombinationProperty, sourceObject.CombinationProperty);
        }
    }
}
