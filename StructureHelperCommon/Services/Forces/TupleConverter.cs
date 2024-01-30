using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace StructureHelperCommon.Services.Forces
{
    public static class TupleConverter
    {
        static readonly IStressLogic stressLogic = new StressLogic();
        public static StrainMatrix ConvertToLoaderStrainMatrix(StrainTuple strainTuple)
        {
            var strainMatrix = new StrainMatrix() { Kx = strainTuple.Mx, Ky = strainTuple.My, EpsZ = strainTuple.Nz };
            return strainMatrix;
        }

        public static StrainTuple ConvertToStrainTuple(IStrainMatrix strainMatrix)
        {
            StrainTuple strainTuple = new StrainTuple() { Mx = strainMatrix.Kx, My = strainMatrix.Ky, Nz = strainMatrix.EpsZ };
            return strainTuple;
        }

        public static ForceTuple ConvertToForceTuple(IEnumerable<INdm> ndms, StrainTuple strainTuple)
        {
            var strainMatrix = ConvertToLoaderStrainMatrix(strainTuple);
            var forceMatrix = stressLogic.GetForceMatrix(ndms, strainMatrix);
            var forceTuple = ConvertToForceTuple(forceMatrix);
            return forceTuple;
        }

        public static ForceTuple ConvertToForceTuple(IForceMatrix forceMatrix)
        {
            ForceTuple forceTuple = new()
            {
                Mx = forceMatrix.Mx,
                My = forceMatrix.My,
                Nz = forceMatrix.Nz
            };
            return forceTuple;
        }
    }
}
