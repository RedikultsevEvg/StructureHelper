using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams.Logics
{
    public class ValueDiagramCheckLogic : CheckEntityLogic<IValueDiagram>
    {
        private const double minDistance = 1e-3;

        public override bool Check()
        {
            bool result = true;
            if (Entity is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": value diagram";
                TraceMessage(errorString);
                throw new StructureHelperException(errorString);
            }
            double dx = Entity.Point2DRange.StartPoint.X - Entity.Point2DRange.EndPoint.X;
            double dy = Entity.Point2DRange.StartPoint.Y - Entity.Point2DRange.EndPoint.Y;
            if (Math.Abs(dx) < minDistance && Math.Abs(dy) < minDistance)
            {
                result = false;
                TraceMessage($"Distance between point of diagram is too small");
            }
            return result;
        }
    }
}
