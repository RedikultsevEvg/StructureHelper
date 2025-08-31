using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IBeamShearCalculatorInputData : ISaveable, IInputData, IHasBeamShearActions, IHasBeamShearSections, IHasStirrups
    {
        IBeamShearDesignRangeProperty DesignRangeProperty { get; set; }
        ShearCodeTypes CodeType { get; set; }
    }
}
