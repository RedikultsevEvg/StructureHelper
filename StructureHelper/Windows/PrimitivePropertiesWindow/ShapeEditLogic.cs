using StructureHelper.Windows.Shapes;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelper.Windows.PrimitivePropertiesWindow
{
    public class ShapeEditLogic
    {
        public double CenterX { get; set; }
        public double CenterY { get; set; }

        public void ShapeEdit(IShape shape)
        {
            if (shape is ILinePolygonShape polygon)
            {
                var viewModel = new PolygonShapeViewModel(polygon, new Point2D() { X = CenterX, Y = CenterY });
                var window = new PolygonView(viewModel);
                window.ShowDialog();
                if (window.DialogResult == true)
                {
                    var newPolygon = viewModel.GetPolygonShape();
                    var updateStrategy = new LinePolygonShapeUpdateStrategy();
                    updateStrategy.Update(polygon, newPolygon);
                }
            }
            else if (shape is IVerticalTShape tShape)
            {
                var logic = new VerticalTShapeCloneStrategy();
                IVerticalTShape tShapeClone = logic.GetClone(tShape);
                var window = new VerticalTShapeView(tShape);
                window.ShowDialog();
                if (window.DialogResult != true)
                {
                    var updateTShapeLogic = new VerticalTShapeUpdateStrategy();
                    updateTShapeLogic.Update(tShape, tShapeClone);
                }
            }
            else if (shape is IVerticalDoubleTShape doubleTShape)
            {
                var logic = new VerticalDoubleTShapeCloneStrategy();
                IVerticalDoubleTShape shapeClone = logic.GetClone(doubleTShape);
                var window = new VerticalDoubleTShapeView(doubleTShape);
                window.ShowDialog();
                if (window.DialogResult != true)
                {
                    var updateLogic = new VerticalDoubleTShapeUpdateStrategy();
                    updateLogic.Update(doubleTShape, shapeClone);
                }
            }
            else if (shape is ITrapezoidShape trapezoidShape)
            {
                var logic = new TrapezoidShapeCloneStrategy();
                ITrapezoidShape shapeClone = logic.GetClone(trapezoidShape);
                var window = new TrapezoidShapeView(trapezoidShape);
                window.ShowDialog();
                if (window.DialogResult != true)
                {
                    var updateLogic = new TrapezoidShapeUpdateStrategy();
                    updateLogic.Update(trapezoidShape, shapeClone);
                }
            }
            else if (shape is IRingShape ringShape)
            {
                var logic = new RingShapeCloneStrategy();
                IRingShape ringShapeClone = logic.GetClone(ringShape);
                var window = new RingShapeView(ringShape);
                window.ShowDialog();
                if (window.DialogResult != true)
                {
                    var updateRingLogic = new RingShapeUpdateStrategy();
                    updateRingLogic.Update(ringShape, ringShapeClone);
                }
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(shape));
            }
        }
    }
}
