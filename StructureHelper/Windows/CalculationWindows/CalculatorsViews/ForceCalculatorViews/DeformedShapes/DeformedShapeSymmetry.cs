using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public class DeformedShapeSymmetry : IDeformedShapeSymmetry
    {
        public int ElementCount { get; set; } = 1;
        public double DX { get; set; }
        public double DY { get; set; }
        public double DZ { get; set; }
    }
}
