using StructureHelperCommon.Models.Shapes;

namespace StructureHelper.Windows.Shapes
{
    public class Point2DRangeViewModel
    {
        private IPoint2DRange point2DRange;
        public string StartPointName { get; set; } = "Start point";
        public string EndPointName { get; set; } = "End point";
        public Point2DViewModel StartPoint { get; private set; }
        public Point2DViewModel EndPoint { get; private set; }

        public Point2DRangeViewModel(IPoint2DRange point2DRange)
        {
            this.point2DRange = point2DRange;
            StartPoint = new(point2DRange.StartPoint);
            EndPoint = new(point2DRange.EndPoint);
        }
    }
}
