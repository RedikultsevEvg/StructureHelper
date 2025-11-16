using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramEntityLogic : IValueDiagramEntityLogic
    {
        public IValueDiagramEntity ValueDiagramEntity { get; set; }

        private IValueDiagramEntityResult result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public IValueDiagramEntityResult Result => result;

        public void Run()
        {
            result = new ValueDiagramEntityResult(ValueDiagramEntity);
            result.PointList = GetPoints();
        }

        private List<IPoint2D> GetPoints()
        {
            TraceLogger?.AddMessage($"Getting point for diagram {ValueDiagramEntity.Name} has been started");
            var startPoint = ValueDiagramEntity.ValueDiagram.Point2DRange.StartPoint;
            var endPoint = ValueDiagramEntity.ValueDiagram.Point2DRange.EndPoint;
            double dx = (endPoint.X - startPoint.X) / ValueDiagramEntity.ValueDiagram.StepNumber;
            double dy = (endPoint.Y - startPoint.Y) / ValueDiagramEntity.ValueDiagram.StepNumber;
            List<IPoint2D> point2Ds = [];
            for (int i = 0; i < ValueDiagramEntity.ValueDiagram.StepNumber + 1; i++)
            {
                double x = startPoint.X + dx * i;
                double y = startPoint.Y + dy * i;
                point2Ds.Add(new Point2D(x, y));
            }
            TraceLogger?.AddMessage($"Getting point for diagram {ValueDiagramEntity.Name} has been finished, total {point2Ds.Count} points obtained");
            return point2Ds;
        }
    }
}
