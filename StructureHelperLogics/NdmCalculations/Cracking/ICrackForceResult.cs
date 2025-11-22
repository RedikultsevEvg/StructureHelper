using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackForceResult : IResult
    {
        IForceTuple CrackedStrainTuple { get; set; }
        IForceTuple EndTuple { get; set; }
        double FactorOfCrackAppearance { get; set; }
        bool IsSectionCracked { get; set; }
        IEnumerable<INdm> NdmCollection { get; set; }
        double PsiS { get; set; }
        IForceTuple ReducedStrainTuple { get; set; }
        IForceTuple SofteningFactors { get; set; }
        IForceTuple StartTuple { get; set; }
        IForceTuple TupleOfCrackAppearance { get; set; }
    }
}