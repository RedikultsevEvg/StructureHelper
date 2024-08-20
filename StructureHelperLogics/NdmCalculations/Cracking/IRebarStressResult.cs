using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IRebarStressResult : IResult
    {
        /// <summary>
        /// Strain tuple which stress and strain is obtained for
        /// </summary>
        StrainTuple StrainTuple { get; set; }
        /// <summary>
        /// Strain in fake concrete ndm-part which rounds rebas and locatade at axis of rebar (refrence strain in concrete)
        /// </summary>
        double ConcreteStrain { get; set; }
        /// <summary>
        /// Strain in rebar, dimensionless
        /// </summary>
        double RebarStrain { get; set; }
        /// <summary>
        /// Stress in rebar, Pa
        /// </summary>
        double RebarStress { get; set; }
    }
}