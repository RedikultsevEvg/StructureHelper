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
            FullName = $"{function.FullName}/{Name}";
            this.downBound = downBound;
            this.upBound = upBound;
        }
        public override bool Check()
        {
            return base.Check();
        }
        public override double GetByX(double xValue)
        {
            double retValue = 0;
            var y = base.GetByX(xValue);
            if (y > downBound && y < upBound)
            {
                retValue = y;
            }
            else if (y <= downBound)
            {
                retValue = downBound;
            }
            else
            {
                retValue = upBound;
            }
            Trace = string.Empty;
            Trace += base.GetTrace();
            Trace += $"Lim Y: {Name}, Input: {y}, Output: {retValue};\n";
            return retValue;
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
                else if (point.Y <= downBound)
                {
                    graphLimitGraphPoint.Add(new GraphPoint(point.X, downBound));
                }
                else
                {
                    graphLimitGraphPoint.Add(new GraphPoint(point.X, upBound));
                }
            }
            graphLimitGraphPoint.Add(upPoint);
            graphSettings.GraphPoints = graphLimitGraphPoint;
            return graphSettings;
        }
        public override string GetTrace()
        {
            return Trace;
        }
    }
}
