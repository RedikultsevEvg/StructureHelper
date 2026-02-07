using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class EllipseShapeCloneStrategy : ICloneStrategy<IEllipseShape>
    {
        IUpdateStrategy<IEllipseShape> updateStrategy;
        IUpdateStrategy<IEllipseShape> UpdateStrategy => updateStrategy ??= new EllipseShapeUpdateStrategy();
        public IEllipseShape GetClone(IEllipseShape sourceObject)
        {
            EllipseShape clone = new EllipseShape(Guid.NewGuid());
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
