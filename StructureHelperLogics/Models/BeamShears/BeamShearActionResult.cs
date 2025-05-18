using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class BeamShearActionResult : IBeamShearActionResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string? Description { get; set; }
        /// <inheritdoc/>
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IBeamShearAction BeamShearAction { get; set; }
        public List<IBeamShearSectionLogicResult> SectionResults { get; set; } = new();
    }
}
