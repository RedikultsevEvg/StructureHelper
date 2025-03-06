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
    public class ScaleXDecorator : FunctionDecorator
    {
        private double factor;
        public ScaleXDecorator(IOneVariableFunction function, double factor) : base(function)
        {
            this.factor = factor;
            Name = $"y=f({factor}x)";
            FullName = $"{function.FullName}/{Name}";
        }
        public override bool Check()
        {
            return base.Check();
        }
        public override double GetByX(double xValue)
        {
            double yValue = base.GetByX(factor * xValue);
            Trace = string.Empty;
            Trace += $"Scale X: {Name}, Input: {xValue}, Output: {factor * xValue};\n";
            Trace += base.GetTrace();
            return yValue;

        }
        public override GraphSettings GetGraphSettings()
        {
            var graphSettings = base.GetGraphSettings();
            foreach(GraphPoint point in graphSettings.GraphPoints)
            {
                point.Y = GetByX(point.X);
            }
            return graphSettings;
        }
        public override string GetTrace()
        {
            return Trace;
        }
    }
}
