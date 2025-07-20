using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    internal class CircleShapeCloneStrategy : ICloneStrategy<ICircleShape>
    {
        IUpdateStrategy<ICircleShape> updateStrategy;
        public ICircleShape GetClone(ICircleShape sourceObject)
        {
            updateStrategy ??= new CircleShapeUpdateStrategy();
            CircleShape clone = new CircleShape(Guid.NewGuid());
            updateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
