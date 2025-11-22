using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureTermCalcualtorInputData : IInputData, IHasPrimitives
    {
        IForceTuple DesignForceTuple { get; set; }
        CalcTerms LoadTerm { get; set; }
        CalcTerms CalculationTerm { get; set; }
    }
}
