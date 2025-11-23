using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureTermCalculatorInputData : IInputData, IHasPrimitives
    {
        IForceTuple ForceTuple { get; set; }
        CalcTerms LoadTerm { get; set; }
        CalcTerms CalculationTerm { get; set; }
        IDeflectionFactor DeflectionFactor { get; set; }
    }
}
