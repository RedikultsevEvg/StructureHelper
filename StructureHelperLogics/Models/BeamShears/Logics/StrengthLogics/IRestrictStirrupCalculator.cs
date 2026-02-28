using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IRestrictStirrupCalculator : ILogicCalculator
    {
        IBeamShearSectionLogicInputData InputData { get; set; }
        ISectionEffectiveness SectionEffectiveness { get; set; }
        double SourceStirrupStrength { get; set; }
        IInclinedSection SourceSection { get; set; }
        double ConcreteFactor { get; set; }
        double StirrupFactor { get; set; }
    }
}