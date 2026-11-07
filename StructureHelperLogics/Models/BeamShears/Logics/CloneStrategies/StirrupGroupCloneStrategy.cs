using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupGroupCloneStrategy : ICloneStrategy<IStirrupGroup>
    {
        private readonly ICloningStrategy cloningStrategy;

        public StirrupGroupCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public IStirrupGroup GetClone(IStirrupGroup sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            return cloningStrategy.Clone(sourceObject, null, cloningMode);
        }
    }
}
