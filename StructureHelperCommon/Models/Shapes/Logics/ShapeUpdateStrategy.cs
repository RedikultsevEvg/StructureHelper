using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes.Logics
{
    public class ShapeUpdateStrategy : IUpdateStrategy<IShape>
    {
        public void Update(IShape targetObject, IShape sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (sourceObject is IRectangleShape sourceRectangle)
            {
                ProcessRectangles(targetObject, sourceRectangle);
            }
            else if (sourceObject is ICircleShape sourceCircle)
            {
                ProcessCircles(targetObject, sourceCircle);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
            }
        }

        private static void ProcessCircles(IShape targetObject, ICircleShape sourceCircle)
        {
            if (targetObject is ICircleShape targetCircle)
            {
                var updateLogic = new CircleShapeUpdateStrategy();
                updateLogic.Update(targetCircle, sourceCircle);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": target object is not circle");
            }
        }

        private static void ProcessRectangles(IShape targetObject, IRectangleShape sourceRectangle)
        {
            if (targetObject is IRectangleShape targetRectangle)
            {
                var updateLogic = new RectangleShapeUpdateStrategy();
                updateLogic.Update(targetRectangle, sourceRectangle);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": target object is not rectangle");
            }
        }
    }
}
