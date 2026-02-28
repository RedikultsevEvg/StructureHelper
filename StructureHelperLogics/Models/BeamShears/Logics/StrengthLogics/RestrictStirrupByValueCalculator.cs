using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    public class RestrictStirrupByValueCalculator : IRestrictStirrupCalculator
    {
        RestrictCalculatorResult result;
        public Guid Id => Guid.Empty;
        public double RestrictionValue { get; set; } = double.PositiveInfinity;
        public IBeamShearSectionLogicInputData InputData { get; set; }
        public ISectionEffectiveness SectionEffectiveness { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public double SourceStirrupStrength { get; set; }
        public IInclinedSection SourceSection { get; set; }
        public double ConcreteFactor { get; set; }
        public double StirrupFactor { get; set; } = 1.0;

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            result = new();
            result.InclinedCrack = SourceSection;
            result.StirrupStrength = Math.Min(SourceStirrupStrength, RestrictionValue);
        }
    }
}
