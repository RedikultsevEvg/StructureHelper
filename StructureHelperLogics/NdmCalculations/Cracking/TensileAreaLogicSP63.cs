using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class TensileAreaLogicSP63 : ITensileAreaLogic
    {
        const double maxConcreteFactor = 0.5d;
        const double minConcreteFactor = 0.1d;
        const double minRebarFactor = 3d;
        private static IStressLogic stressLogic => new StressLogic();
        public IEnumerable<INdm> NdmCollection { get; set; }
        public IStrainMatrix StrainMatrix { get; set; }

        public double GetTensileArea()
        {
            var rebarCollection = NdmCollection
                .Where(x => x is RebarNdm);
            var rebarArea = rebarCollection.
                Sum(x => x.Area * x.StressScale);
            var concreteCollection = NdmCollection
                .Where(x => x.Material is ConcreteMaterial);
            var concreteArea = concreteCollection
                .Sum(x => x.Area * x.StressScale);
            var concreteTensileArea = concreteCollection
                .Where(x => stressLogic.GetTotalStrainWithPresrain(StrainMatrix, x) > 0d)
                .Sum(x => x.Area * x.StressScale);
            
            concreteTensileArea = Math.Max(concreteTensileArea, rebarArea * minRebarFactor);
            concreteTensileArea = Math.Max(concreteTensileArea, concreteArea * minConcreteFactor);
            concreteTensileArea = Math.Min(concreteTensileArea, concreteArea * maxConcreteFactor);

            return concreteTensileArea;
        }
    }
}
