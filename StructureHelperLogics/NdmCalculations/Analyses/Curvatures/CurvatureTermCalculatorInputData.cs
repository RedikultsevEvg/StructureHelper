using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureTermCalculatorInputData : ICurvatureTermCalculatorInputData
    {
        public IForceTuple ForceTuple { get; set; }
        public CalcTerms LoadTerm { get; set; }
        public CalcTerms CalculationTerm { get; set; }

        public List<INdmPrimitive> Primitives { get; set; } = [];
        public IDeflectionFactor DeflectionFactor { get; set; }
    }
}
