using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Shapes
{
    public class TrapezoidShapeUpdateStrategy : IUpdateStrategy<ITrapezoidShape>
    {
        public void Update(ITrapezoidShape targetObject, ITrapezoidShape sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Height = sourceObject.Height;
            targetObject.TopBase = sourceObject.TopBase;
            targetObject.BottomBase = sourceObject.BottomBase;
            targetObject.TopBaseOffset = sourceObject.TopBaseOffset;
        }
    }
}
