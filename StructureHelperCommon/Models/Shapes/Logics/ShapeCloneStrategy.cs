using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public class ShapeCloneStrategy : ICloneStrategy<IShape>
    {
        private ICloneStrategy<IRectangleShape> rectangleCloneStrategy;
        private ICloneStrategy<IRectangleShape> RectangleCloneStrategy => rectangleCloneStrategy ??= new RectangleShapeCloneStrategy();
        private ICloneStrategy<IEllipseShape> ellipseCloneStrategy;
        private ICloneStrategy<IEllipseShape> EllipseCloneStrategy => ellipseCloneStrategy ??= new EllipseShapeCloneStrategy();
        private ICloneStrategy<ICircleShape> circleCloneStrategy;
        private ICloneStrategy<ICircleShape> CircleCloneStrategy => circleCloneStrategy ??= new CircleShapeCloneStrategy();

        public IShape GetClone(IShape sourceObject)
        {
            if (sourceObject is IRectangleShape rectangleShape)
            {
                return RectangleCloneStrategy.GetClone(rectangleShape);
            }
            else if (sourceObject is IEllipseShape ellipseShape)
            {
                return EllipseCloneStrategy.GetClone(ellipseShape);
            }
            else if (sourceObject is ICircleShape circleShape)
            {
                return CircleCloneStrategy.GetClone(circleShape);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
        }
    }
}
