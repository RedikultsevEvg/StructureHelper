using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public class DeformedShapeSymmetrySet : IDeformedShapeSymmetrySet
    {
        public List<IDeformedShapeSymmetry> SymmetrySet { get; } = [new DeformedShapeSymmetry()];
    }
}
