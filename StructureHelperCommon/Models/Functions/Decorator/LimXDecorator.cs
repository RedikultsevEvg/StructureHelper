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
    public class LimXDecorator : FunctionDecorator
    {
        private double leftBound;
        private double rightBound;
        public LimXDecorator(IOneVariableFunction function, double leftBound, double rightBound) : base(function)
        {
            Name = $"x\u2208[{leftBound};{rightBound}]";
            this.leftBound = leftBound;
            this.rightBound = rightBound;
        }
        public override bool Check()
        {
            return base.Check();
        }
        public override double GetByX(double xValue)
        {
            if (xValue > leftBound && xValue < rightBound)
            {
                return base.GetByX(xValue);
            }
            return 0;
        }
        public override GraphSettings GetGraphSettings()
        {
            var graphSettings = base.GetGraphSettings();
            var graphLimitGraphPoint = new List<GraphPoint>();
            var leftPoint = new GraphPoint(leftBound, GetByX(leftBound));
            var rightPoint = new GraphPoint(rightBound, GetByX(rightBound));
            graphLimitGraphPoint.Add(leftPoint);
            foreach (GraphPoint point in graphSettings.GraphPoints)
            {
                if (point.X > leftBound && point.X < rightBound)
                {
                    graphLimitGraphPoint.Add(point);
                }
            }
            graphLimitGraphPoint.Add(rightPoint);
            graphSettings.GraphPoints = graphLimitGraphPoint;
            return graphSettings;
        }
    }
}
