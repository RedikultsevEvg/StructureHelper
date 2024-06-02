using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class StabLimitCurveLogic : ILimitCurveLogic
    {
        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public List<IPoint2D> GetPoints(IEnumerable<IPoint2D> points)
        {
            var result = new List<IPoint2D>();
            foreach (var item in points)
            {
                result.Add(new Point2D() { X = item.X * 0.5d, Y = item.Y * 0.5d });
                Thread.Sleep(10);
            }
            return result;
        }
    }
}
