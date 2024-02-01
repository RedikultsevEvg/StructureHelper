using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Geometry
{
    public class GeometryCalculator : IGeometryCalculator
    {
        TextParametersLogic parametersLogic;
        IGeometryResult geometryResult;
        public string Name { get; set; }

        public IResult Result => geometryResult;

        public Action<IResult> ActionToOutputResults { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IShiftTraceLogger? TraceLogger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public GeometryCalculator(IEnumerable<INdm> ndms, IStrainMatrix strainMatrix)
        {
            parametersLogic = new TextParametersLogic(ndms, strainMatrix);
        }

        public GeometryCalculator(TextParametersLogic parametersLogic)
        {
            this.parametersLogic = parametersLogic;
        }

        public void Run()
        {
            geometryResult = new GeometryResult() { IsValid = true };
            geometryResult.TextParameters = parametersLogic.GetTextParameters();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }




    }
}
