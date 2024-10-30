using LiveCharts;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Functions.Decorator
{
    public class LimYDecorator : FunctionDecorator
    {
        private double leftBound;
        private double rightBound;
        public LimYDecorator(IOneVariableFunction function, double leftBound, double rightBound) : base(function)
        {
            Name = $"y\u2208[{leftBound};{rightBound}]";
            this.leftBound = leftBound;
            this.rightBound = rightBound;
        }
        public override bool Check()
        {
            return base.Check();
        }
        public override double GetByX(double xValue)
        {
            var y = base.GetByX(xValue);
            if (y > leftBound && y < rightBound)
            {
                return y;
            }
            return 0;
        }
        public override SeriesCollection GetSeriesCollection()
        {
            return base.GetSeriesCollection();
        }
    }
}
