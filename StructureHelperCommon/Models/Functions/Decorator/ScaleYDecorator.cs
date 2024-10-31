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
    public class ScaleYDecorator : FunctionDecorator
    {
        private double factor;
        public ScaleYDecorator(IOneVariableFunction function, double factor) : base(function)
        {
            this.factor = factor;
            Name = $"y={factor}f(x)";
        }
        public override bool Check()
        {
            return base.Check();
        }
        public override double GetByX(double xValue)
        {
            return factor * base.GetByX(xValue);
        }
        public override GraphSettings GetGraphSettings()
        {
            var graphSettings = base.GetGraphSettings();
            foreach (GraphPoint point in graphSettings.GraphPoints)
            {
                point.Y = GetByX(point.X);
            }
            return graphSettings;
        }
    }
}
