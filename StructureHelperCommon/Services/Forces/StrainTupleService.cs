using LoaderCalculator.Data.Matrix;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperCommon.Services.Forces
{
    public static class StrainTupleService
    {
        public static IStrainMatrix ConvertToLoaderStrainMatrix(StrainTuple strainTuple)
        {
            IStrainMatrix strainMatrix = new StrainMatrix() { Kx = strainTuple.Mx, Ky = strainTuple.My, EpsZ = strainTuple.Nz };
            return strainMatrix;
        }

        public static StrainTuple ConvertToStrainTuple(IStrainMatrix strainMatrix)
        {
            StrainTuple strainTuple = new StrainTuple() { Mx = strainMatrix.Kx, My = strainMatrix.Ky, Nz = strainMatrix.EpsZ };
            return strainTuple;
        }
    }
}
