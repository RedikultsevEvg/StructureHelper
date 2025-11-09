using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrMath = StructureMath.Geometry.Points;

namespace StructureHelperCommon.Models.Shapes
{
    public class GetPointListByRange : IGetPointListByRange
    {
        public int StepNumber { get; set; } = 50;

        public List<IPoint2D> GetPoints(IPoint2DRange range)
        {
            StrMath.Point2D StartPoint2D = new(range.StartPoint.X, range.StartPoint.Y);
            StrMath.Point2D EndPoint2D = new(range.EndPoint.X, range.EndPoint.Y);
            StrMath.PointRange pointRange = new() { StartPoint = StartPoint2D, EndPoint = EndPoint2D };
            var logic = new StrMath.PointRangeInterpolate() {  DivisionNumber = StepNumber };
            var pointList = logic.InterpolateRange(pointRange);
            List<IPoint2D> result = [];
            foreach (var point in pointList)
            {
                result.Add(new Point2D(point.X, point.Y));
            }
            return result;
        }
    }
}
