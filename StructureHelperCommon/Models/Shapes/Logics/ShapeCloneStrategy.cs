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
        private ICloneStrategy<ICircleShape> circleCloneStrategy;
        public IShape GetClone(IShape sourceObject)
        {
            if (sourceObject is IRectangleShape rectangleShape)
            {
                rectangleCloneStrategy ??= new RectangleShapeCloneStrategy();
                return rectangleCloneStrategy.GetClone(rectangleShape);
            }
            else if (sourceObject is ICircleShape circleShape)
            {
                circleCloneStrategy ??= new CircleShapeCloneStrategy();
                return circleCloneStrategy.GetClone(circleShape);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
        }
    }
}
