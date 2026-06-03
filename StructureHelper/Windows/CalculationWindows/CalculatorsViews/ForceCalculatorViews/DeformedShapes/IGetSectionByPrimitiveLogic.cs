using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public interface IGetSectionByPrimitiveLogic
    {
        List<Vector2> GetSection(INdmPrimitive primitive);
    }
}
