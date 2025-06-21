using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IGetInclinedSectionListInputData : IInputData
    {
        int StepCount { get; set; }
        double MaxInclinedSectionLegthFactor { get; set; }
        IGetInclinedSectionLogic? GetInclinedSectionLogic { get; set; }
        IBeamShearSection BeamShearSection { get; set; }
    }
}
