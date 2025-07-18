using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IGetInclinedSectionListInputData : IInputData
    {
        IBeamShearDesignRangeProperty DesignRangeProperty { get; }
        IGetInclinedSectionLogic? GetInclinedSectionLogic { get; set; }
        IBeamShearSection BeamShearSection { get; set; }
    }
}
