using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class HasActionsUpdateCloneStrategy : IUpdateStrategy<IHasBeamShearActions>
    {
        private readonly ICloningStrategy cloningStrategy;

        public HasActionsUpdateCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IHasBeamShearActions targetObject, IHasBeamShearActions sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Actions.Clear();
            foreach (var item in sourceObject.Actions)
            {
                var newItem = cloningStrategy.Clone(item);
                targetObject.Actions.Add(newItem);
            }
        }
    }
}
