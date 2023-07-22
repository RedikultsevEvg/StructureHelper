using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using LoaderCalculator.Logics.Geometry;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    internal class SofteningFactorLogic
    {
        public static IStressLogic stressLogic = new StressLogic();
        public IEnumerable<INdm> NdmCollection { get; set; }
        public StrainTuple StrainTuple { get; set; }
        public StrainTuple GetSofteningFactors()
        {
            var strainTuple = new StrainTuple();
            var loaderStainMatrix = StrainTupleService.ConvertToLoaderStrainMatrix(StrainTuple);
            var (MxFactor, MyFactor, NzFactor) = GeometryOperations.GetSofteningsFactors(NdmCollection, loaderStainMatrix);
            strainTuple.Mx = MxFactor;
            strainTuple.My = MyFactor;
            strainTuple.Nz = NzFactor;
            return strainTuple;
        }
    }
}
