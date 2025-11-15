using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class GetMoqNdmLogic : IGetMoqNdmLogic
    {
        public INdm GetMockNdm(INdmPrimitive ndmPrimitive, IStateCalcTermPair stateCalcTermPair, IPoint2D point, double area = 0)
        {
            var limitState = stateCalcTermPair.LimitState;
            var calcTerm = stateCalcTermPair.CalcTerm;
            var material = ndmPrimitive.NdmElement.HeadMaterial.GetLoaderMaterial(limitState, calcTerm);
            var userPrestrain = ndmPrimitive.NdmElement.UsersPrestrain;
            var autoPrestrain = ndmPrimitive.NdmElement.AutoPrestrain;
            var ndm = new Ndm()
            {
                Area = area,
                CenterX = point.X,
                CenterY = point.Y,
                Material = material,
            };
            var prestrain = (userPrestrain.Mx + autoPrestrain.Mx) * point.Y
                + (userPrestrain.My + autoPrestrain.My) * point.X
                + userPrestrain.Nz + autoPrestrain.Nz;
            ndm.PrestrainLogic.Add(PrestrainTypes.Prestrain, prestrain);
            return ndm;
        }
    }
}
