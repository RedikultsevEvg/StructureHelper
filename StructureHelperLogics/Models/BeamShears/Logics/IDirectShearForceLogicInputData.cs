using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implements input data fore calculating forces at the and of inclined section
    /// </summary>
    public interface IDirectShearForceLogicInputData : IInputData
    {
        IBeamShearAction BeamShearAction { get; set; }
        CalcTerms CalcTerm { get; set; }
        IInclinedSection InclinedSection { get; set; }
        LimitStates LimitState { get; set; }
    }
}