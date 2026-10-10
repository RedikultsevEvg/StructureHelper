using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class ConstantShapeFactorLogic : IGetShapeFactorLogic
    {
        private readonly double constantValue;

        public ConstantShapeFactorLogic(double constantValue = 1, IShiftTraceLogger? traceLogger = null)
        {
            this.constantValue = constantValue;
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetShapeFactor()
        {
            return constantValue;
        }
    }
}
