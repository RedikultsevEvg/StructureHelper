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
            FullName = $"{function.FullName}/{Name}";
        }
        public override bool Check()
        {
            return base.Check();
        }
        public override double GetByX(double xValue)
        {
            double functionValue = base.GetByX(xValue);
            double yValue = factor * functionValue;
            Trace = string.Empty;
            Trace += base.GetTrace();
            Trace += $"Scale Y: {Name}, Input: {functionValue}, Output: {yValue};\n";
            return yValue;

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
        public override string GetTrace()
        {
            return Trace;
        }
    }
}
