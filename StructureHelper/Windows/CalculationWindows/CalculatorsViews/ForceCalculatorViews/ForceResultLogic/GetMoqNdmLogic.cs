using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Infrastructure.Geometry;
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
            StrainMatrix prestrainMatrix = new()
            {
                Kx = (userPrestrain.Mx + autoPrestrain.Mx),
                Ky = (userPrestrain.My + autoPrestrain.My),
                EpsZ = userPrestrain.Nz + autoPrestrain.Nz
            };
            ndm.PrestrainLogic.Add(PrestrainTypes.Prestrain, prestrainMatrix);
            return ndm;
        }

        public INdm GetMockNdm(INdm ndm, IPoint2D point)
        {
            INdm newNdm = NdmTransform.GetMoqNdmAtPoint(ndm, new PointLd2D(point.X, point.Y));
            return newNdm;
        }
    }
}
