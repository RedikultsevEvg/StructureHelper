using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public interface IDeformedShapeSymmetrySet
    {
        List<IDeformedShapeSymmetry> SymmetrySet { get; }
    }
}
