using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public interface IModifyPathByCurvatureLogic
    {
        IDeformedPath ProcessPath(IDeformedPath path, IForceTuple curvature, double scale = 1.0);
        IDeformedPath ProcessPath(IDeformedPath path, float dx, float dy, float dz);
    }
}
