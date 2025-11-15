using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public interface IGetMoqNdmLogic
    {
        INdm GetMockNdm(INdmPrimitive ndmPrimitive, IStateCalcTermPair stateCalcTermPair, IPoint2D point, double area = 0);
    }
}
