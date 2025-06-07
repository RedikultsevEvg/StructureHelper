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
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.LoadRatio = sourceObject.LoadRatio;
            targetObject.RelativeLoadLevel = sourceObject.RelativeLoadLevel;
            CheckObject.IsNull(sourceObject.CombinationProperty);
            CheckObject.IsNull(targetObject.CombinationProperty);
            combinationUpdateStrategy ??= new FactoredCombinationPropertyUpdateStrategy();
            combinationUpdateStrategy.Update(targetObject.CombinationProperty, sourceObject.CombinationProperty);
        }
    }
}
