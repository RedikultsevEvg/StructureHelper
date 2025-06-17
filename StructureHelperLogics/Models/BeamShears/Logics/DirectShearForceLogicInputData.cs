using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class DirectShearForceLogicInputData : IDirectShearForceLogicInputData
    {
        public IBeamShearAction BeamShearAction { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IInclinedSection InclinedSection { get; set; }
        public LimitStates LimitState { get; set; }
    }
}
