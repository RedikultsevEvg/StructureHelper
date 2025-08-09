using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class BeamShearSectionLogicInputData : IBeamShearSectionLogicInputData
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public IInclinedSection InclinedSection { get; set; }
        /// <inheritdoc/>
        public IInclinedSection InclinedCrack { get; set; }
        /// <inheritdoc/>
        public IStirrup Stirrup { get; set; }
        /// <inheritdoc/>
        public LimitStates LimitState { get; set; }
        /// <inheritdoc/>
        public CalcTerms CalcTerm { get; set; }
        /// <inheritdoc/>
        public IForceTuple ForceTuple { get; set; }
        /// <inheritdoc/>
        public IBeamShearAction BeamShearAction { get; set; }
        /// <inheritdoc/>
        public IBeamShearSection BeamShearSection { get; set; }

        public BeamShearSectionLogicInputData(Guid id)
        {
            Id = id;
        }

    }
}
