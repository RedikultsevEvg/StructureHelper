using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class BeamShearActionUpdateStrategy : IUpdateStrategy<IBeamShearAction>
    {
        private IUpdateStrategy<IBeamShearAxisAction> axisActionUpdateStrategy;
        public void Update(IBeamShearAction targetObject, IBeamShearAction sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            CheckObject.IsNull(targetObject.XAxisShearAction);
            CheckObject.IsNull(sourceObject.XAxisShearAction);
            axisActionUpdateStrategy.Update(targetObject.XAxisShearAction, sourceObject.XAxisShearAction);
            CheckObject.IsNull(sourceObject.YAxisShearAction);
            CheckObject.IsNull(targetObject.YAxisShearAction);
            axisActionUpdateStrategy.Update(targetObject.YAxisShearAction, sourceObject.YAxisShearAction);
        }

        private void InitializeStrategies()
        {
            axisActionUpdateStrategy ??= new BeamShearAxisActionUpdateStrategy();
        }
    }
}
