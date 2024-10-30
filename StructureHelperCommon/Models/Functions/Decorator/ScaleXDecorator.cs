using LiveCharts;
using StructureHelperCommon.Infrastructures.Interfaces;
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
        }
        public override bool Check()
        {
            return base.Check();
        }
        public override double GetByX(double xValue)
        {
            return base.GetByX(factor * xValue);
        }
        public override SeriesCollection GetSeriesCollection()
        {
            return base.GetSeriesCollection();
        }
    }
}
