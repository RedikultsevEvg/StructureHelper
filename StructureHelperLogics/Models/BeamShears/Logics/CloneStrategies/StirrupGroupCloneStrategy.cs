using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
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


        public IStirrupGroup GetClone(IStirrupGroup sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            IStirrupGroup newItem;
            if (cloneDepth == CloneDepth.Deep)
            {
                newItem = (IStirrupGroup)sourceObject.GetClone(cloneMode, cloneDepth);
            }
            else if (cloneDepth == CloneDepth.PreserveReference)
            {
                newItem = cloningStrategy.Clone(sourceObject, null, cloneMode);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cloneDepth) + ": cloning depth is not right");
            }
            return newItem;
        }
    }
}
