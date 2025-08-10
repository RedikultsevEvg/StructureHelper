using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.Materials
{
    public interface IRebarSectionStrengthLogic : ILogic
    {
        IRebarSection RebarSection { get; set; }
        IShiftTraceLogger? TraceLogger { get; set; }
        CalcTerms CalcTerm { get; set; }
        LimitStates LimitState { get; set; }
        double MaxRebarStrength { get; set; }
        double RebarStrengthFactor { get; set; }

        double GetRebarMaxTensileForce();
    }
}