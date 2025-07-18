using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class HasStirrupsUpdateCloneStrategy : IUpdateStrategy<IHasStirrups>
    {
        private readonly ICloningStrategy cloningStrategy;
        private ICloneStrategy<IStirrupGroup> groupCloneStrategy;

        public HasStirrupsUpdateCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IHasStirrups targetObject, IHasStirrups sourceObject)
        {
            CheckObject.IsNull(cloningStrategy);
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Stirrups.Clear();
            foreach (var item in sourceObject.Stirrups)
            {
                IStirrup newStirrup;
                newStirrup = cloningStrategy.Clone(item);
                targetObject.Stirrups.Add(newStirrup);
            }
        }
    }
}
