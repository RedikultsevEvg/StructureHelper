using StructureHelperCommon.Infrastructures.Interfaces;
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
    }
}
