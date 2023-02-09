using LoaderCalculator.Data.Matrix;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperCommon.Services.Forces
{
    public static class StrainTupleService
    {
        public static void CopyProperties (IStrainTuple source, IStrainTuple target, double factor = 1 )
        {
            target.Kx = source.Kx * factor;
            target.Ky = source.Ky * factor;
            target.EpsZ = source.EpsZ * factor;
            target.Gx = source.Gx * factor;
            target.Gy = source.Gy * factor;
            target.Gz = source.Gz * factor;
        }

        public static IStrainMatrix ConvertToLoaderStrainMatrix(IStrainTuple strainTuple)
        {
            IStrainMatrix strainMatrix = new StrainMatrix() { Kx = strainTuple.EpsZ, Ky = strainTuple.Ky, EpsZ = strainTuple.EpsZ };
            return strainMatrix;
        }

        public static IStrainTuple ConvertToStrainTuple(IStrainMatrix strainMatrix)
        {
            IStrainTuple strainTuple = new StrainTuple() { Kx = strainMatrix.Kx, Ky = strainMatrix.Ky, EpsZ = strainMatrix.EpsZ };
            return strainTuple;
        }
    }
}
