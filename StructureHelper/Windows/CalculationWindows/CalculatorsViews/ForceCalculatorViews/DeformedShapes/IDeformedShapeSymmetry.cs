using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public interface IDeformedShapeSymmetry
    {
        int ElementCount { get; set; }
        double DX { get; set; }
        double DY { get; set; }
        double DZ { get; set; }
    }
}
