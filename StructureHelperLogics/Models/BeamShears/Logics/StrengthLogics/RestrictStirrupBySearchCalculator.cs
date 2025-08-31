using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    public class RestrictStirrupBySearchCalculator : IRestrictStirrupCalculator
    {
        RestrictCalculatorResult result;
        public Guid Id => Guid.Empty;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public IResult Result => result;

        public IBeamShearSectionLogicInputData InputData { get; set; }
        public ISectionEffectiveness SectionEffectiveness { get; set; }
        public double SourceStirrupStrength { get; set; }
        public IInclinedSection SourceSection { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            result = new();
            try
            {
                var logic = new StirrupBySearchLogic(TraceLogger?.GetSimilarTraceLogger(100))
                {
                    InputData = InputData,
                    SectionEffectiveness = SectionEffectiveness
                };
                double stirrupStrength = logic.CalculateShearStrength();
                TraceLogger?.AddMessage($"Stirrup strength was restricted as Qsw,restricted = {stirrupStrength}(N)");
                result.InclinedCrack = logic.InclinedCrack;
                result.StirrupStrength = stirrupStrength;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                result.Description += ex.Message;
            }
        }
    }
}
