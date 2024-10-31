using LiveCharts;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Functions.Decorator
{
    public class LimYDecorator : FunctionDecorator
    {
        private double downBound;
        private double upBound;
        public LimYDecorator(IOneVariableFunction function, double downBound, double upBound) : base(function)
        {
            Name = $"y\u2208[{downBound};{upBound}]";
            this.downBound = downBound;
            this.upBound = upBound;
        }
        public override bool Check()
        {
            return base.Check();
        }
        public override double GetByX(double xValue)
        {
            var y = base.GetByX(xValue);
            if (y > downBound && y < upBound)
            {
                return y;
            }
            return 0;
        }
        public override GraphSettings GetGraphSettings()
        {
            var graphSettings = base.GetGraphSettings();
            var graphLimitGraphPoint = new List<GraphPoint>();
            var downPoint = new GraphPoint(downBound, GetByX(downBound));
            var upPoint = new GraphPoint(upBound, GetByX(upBound));
            graphLimitGraphPoint.Add(downPoint);
            foreach (GraphPoint point in graphSettings.GraphPoints)
            {
                if (point.Y > downBound && point.Y < upBound)
                {
                    graphLimitGraphPoint.Add(point);
                }
            }
            graphLimitGraphPoint.Add(upPoint);
            graphSettings.GraphPoints = graphLimitGraphPoint;
            return graphSettings;
        }
    }
}
