using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public interface IDeformedSection
    {
        List<Vector2> Vertices { get; set; }
    }
}
