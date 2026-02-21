using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class ShapeUpdateStrategy : IUpdateStrategy<IShape>
    {
        private const string targetIsNotSuitableType = ": target object is not";
        private IUpdateStrategy<IRingShape> oShapeUpdateStrategy;
        private IUpdateStrategy<IVerticalTShape> tShapeUpdateStrategy;
        private IUpdateStrategy<IVerticalDoubleTShape> doubleTShapeUpdateStrategy;
        private IUpdateStrategy<ITrapezoidShape> trapezoidUpdateStrategy;
        private IUpdateStrategy<IRingShape> OShapeUpdateStrategy => oShapeUpdateStrategy ??= new RingShapeUpdateStrategy();
        private IUpdateStrategy<IVerticalTShape> TShapeUpdateStrategy => tShapeUpdateStrategy ??= new VerticalTShapeUpdateStrategy();
        private IUpdateStrategy<IVerticalDoubleTShape> DoubleTShapeUpdateStrategy => doubleTShapeUpdateStrategy ??= new VerticalDoubleTShapeUpdateStrategy();
        private IUpdateStrategy<ITrapezoidShape> TrapezoidUpdateStrategy => trapezoidUpdateStrategy ??= new TrapezoidShapeUpdateStrategy();

        public void Update(IShape targetObject, IShape sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (sourceObject is IRectangleShape sourceRectangle)
            {
                ProcessRectangles(targetObject, sourceRectangle);
            }
            else if (sourceObject is ICircleShape sourceCircle)
            {
                ProcessCircles(targetObject, sourceCircle);
            }
            else if (sourceObject is IEllipseShape ellipseShape)
            {
                ProcessEllipse(targetObject, ellipseShape);
            }
            else if (sourceObject is IRingShape sourceOShape)
            {
                ProcessOShape(targetObject, sourceOShape);
            }
            else if (sourceObject is ILinePolygonShape sourcePolygon)
            {
                ProcessPolygon(targetObject, sourcePolygon);
            }
            else if (sourceObject is IVerticalTShape sourceTShape)
            {
                ProcessTShape(targetObject, sourceTShape);
            }
            else if (sourceObject is IVerticalDoubleTShape sourceDoubleTShape)
            {
                ProcessDoubleTShape(targetObject, sourceDoubleTShape);
            }
            else if (sourceObject is ITrapezoidShape trapezoid)
            {
                ProcessTrapezoidShape(targetObject, trapezoid);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
            }
        }

        private void ProcessTrapezoidShape(IShape targetObject, ITrapezoidShape sourceTrapezoid)
        {
            if (targetObject is not ITrapezoidShape targetTrapezoidShape)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $"{targetIsNotSuitableType} a trapezoid shape");
            }
            TrapezoidUpdateStrategy.Update(targetTrapezoidShape, sourceTrapezoid);
        }

        private void ProcessDoubleTShape(IShape targetObject, IVerticalDoubleTShape sourceDoubleTShape)
        {
            if (targetObject is not IVerticalDoubleTShape targetTShape)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $"{targetIsNotSuitableType} a double T-shape");
            }
            DoubleTShapeUpdateStrategy.Update(targetTShape, sourceDoubleTShape);
        }

        private void ProcessTShape(IShape targetObject, IVerticalTShape sourceTShape)
        {
            if (targetObject is not IVerticalTShape targetTShape)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $"{targetIsNotSuitableType} an T-shape");
            }
            TShapeUpdateStrategy.Update(targetTShape, sourceTShape);
        }

        private void ProcessOShape(IShape targetObject, IRingShape sourceOShape)
        {
            if (targetObject is not IRingShape targetOShape)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $"{targetIsNotSuitableType} a O-shape");
            };
            OShapeUpdateStrategy.Update(targetOShape, sourceOShape);
        }

        private void ProcessEllipse(IShape targetObject, IEllipseShape sourceEllipse)
        {
            if (targetObject is IEllipseShape targetEllipse)
            {
                var updateLogic = new EllipseShapeUpdateStrategy();
                updateLogic.Update(targetEllipse, sourceEllipse);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": target object is not an ellipse");
            }
        }

        private void ProcessPolygon(IShape targetObject, ILinePolygonShape sourcePolygon)
        {
            if (targetObject is ILinePolygonShape targetPolygon)
            {
                var updateLogic = new LinePolygonShapeUpdateStrategy();
                updateLogic.Update(targetPolygon, sourcePolygon);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": target object is not a polygon");
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
