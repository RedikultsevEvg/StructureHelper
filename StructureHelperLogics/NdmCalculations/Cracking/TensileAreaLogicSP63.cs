using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
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
        const double minRebarFactor = 9d;
        private static IStressLogic stressLogic => new StressLogic();
        public IEnumerable<INdm> NdmCollection { get; set; }
        public IStrainMatrix StrainMatrix { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetTensileArea()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);

            var rebarCollection = NdmCollection
                .Where(x => x is RebarNdm & stressLogic.GetTotalStrain(StrainMatrix, x) > 0d);
            var rebarArea = rebarCollection.
                Sum(x => x.Area * x.StressScale);
            TraceLogger?.AddMessage($"Summary rebar area As = {rebarArea}");
            var concreteCollection = NdmCollection
                .Where(x => x.Material is ConcreteMaterial);
            var concreteArea = concreteCollection
                .Sum(x => x.Area * x.StressScale);
            TraceLogger?.AddMessage($"Concrete area Ac = {concreteArea}");
            var concreteTensileArea = concreteCollection
                .Where(x => stressLogic.GetTotalStrainWithPrestrain(StrainMatrix, x) > 0d)
                .Sum(x => x.Area * x.StressScale);
            TraceLogger?.AddMessage($"Concrete tensile area Ac,t = {concreteTensileArea}");

            double areaByRebar = rebarArea * minRebarFactor;
            TraceLogger?.AddMessage($"Concrete area is considered not less than {minRebarFactor} of area of rebars");
            TraceLogger?.AddMessage($"Minimum concrete effective area from area of rebar Ac,eff = {rebarArea} * {minRebarFactor} = {areaByRebar}");
            concreteTensileArea = Math.Max(concreteTensileArea, areaByRebar);
            double areaByMinConcreteFactor = concreteArea * minConcreteFactor;
            TraceLogger?.AddMessage($"Concrete area is considered not less than {minConcreteFactor} of area of concrete");
            TraceLogger?.AddMessage($"Minimum concrete effective area Ac,eff = {concreteArea} * {minConcreteFactor} = {areaByMinConcreteFactor}");
            concreteTensileArea = Math.Max(concreteTensileArea, areaByMinConcreteFactor);
            double areaByMaxConcreteFactor = concreteArea * maxConcreteFactor;
            TraceLogger?.AddMessage($"Concrete area is considered not greater than {maxConcreteFactor} of area of concrete");
            TraceLogger?.AddMessage($"Maximum concrete effective area Ac,eff = {concreteArea} * {maxConcreteFactor} = {areaByMaxConcreteFactor}");
            concreteTensileArea = Math.Min(concreteTensileArea, areaByMaxConcreteFactor);
            TraceLogger?.AddMessage($"Concrete effective area Ac,eff = {concreteTensileArea}");
            return concreteTensileArea;
        }
    }
}
