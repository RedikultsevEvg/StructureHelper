using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Ndms.Transformations;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.Design.AxImporter;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    internal static class TriangulationService
    {
        public static void SetPrestrain(IEnumerable<INdm> ndmCollection, IStrainTuple strainTuple)
        {
            NdmTransform.SetPrestrain(ndmCollection, new StrainMatrix() { Kx = strainTuple.Kx, Ky = strainTuple.Kx, EpsZ = strainTuple.Kx });
        }

        public static void CommonTransform(IEnumerable<INdm> ndmCollection, IShapeTriangulationLogicOptions options)
        {
            double dX = options.Center.X;
            double dY = options.Center.Y;
            NdmTransform.Move(ndmCollection, dX, dY);
        }
    }
}
