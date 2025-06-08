using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Geometry
{
    public class GeometryCalculator : IGeometryCalculator
    {
        IParametersLogic parametersLogic;
        IGeometryResult geometryResult;

        public IResult Result => geometryResult;

        public Action<IResult> ActionToOutputResults { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IShiftTraceLogger? TraceLogger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid Id => throw new NotImplementedException();

        public GeometryCalculator(IParametersLogic parametersLogic)
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
