using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    internal static class TriangulationService
    {
        public static void SetPrestrain(IEnumerable<INdm> ndmCollection, StrainTuple strainTuple, PrestrainTypes prestrainType)
        {
            StrainMatrix prestrainMatrix = new() { Kx = strainTuple.Mx, Ky = strainTuple.My, EpsZ = strainTuple.Nz };
            NdmTransform.SetPrestrain(ndmCollection, prestrainMatrix, prestrainType);
        }

        public static void SetPrestrain(INdm ndm, StrainTuple strainTuple, PrestrainTypes prestrainType)
        {
            StrainMatrix prestrainMatrix = new() { Kx = strainTuple.Mx, Ky = strainTuple.My, EpsZ = strainTuple.Nz };
            NdmTransform.SetPrestrain(ndm, prestrainMatrix, prestrainType);
        }

        public static void CommonTransform(IEnumerable<INdm> ndmCollection, IShapeTriangulationLogicOptions options)
        {
            double dX = options.Center.X;
            double dY = options.Center.Y;
            NdmTransform.Move(ndmCollection, dX, dY);
        }
    }
}
