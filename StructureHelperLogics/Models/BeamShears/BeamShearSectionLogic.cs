using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.BeamShears.Logics;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSectionLogic : IBeamShearSectionLogic
    {
        private BeamShearSectionLogicResult result;
        private ConcreteStrengthLogic concreteLogic;
        private StirrupStrengthLogic stirrupLogic;

        public IBeamShearSectionLogicInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public BeamShearSectionLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IResult Result => result;


        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            PrepareNewResult();
            InitializeStrategies();
            try
            {
                CalculateResult();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex.Message;
            }
        }

        private void CalculateResult()
        {
            double concreteStrength = concreteLogic.GetShearStrength();
            result.ConcreteStrength = concreteStrength;
            double stirrupStrength = stirrupLogic.GetShearStrength();
            result.StirrupStrength = stirrupStrength;
            double totalStrength = concreteStrength + stirrupStrength;
            result.TotalStrength = totalStrength;
            double actualShearForce = InputData.ForceTuple.Qy;
            if (actualShearForce > totalStrength)
            {
                result.IsValid = false;
                string message = $"Actual shear force Qa = {actualShearForce}(N), greater than bearing capacity Olim = {totalStrength}(N)";
                result.Description += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
            }
            else
            {
                string message = $"Actual shear force Qa = {actualShearForce}(N), not greater than bearing capacity Olim = {totalStrength}(N)";
                TraceLogger?.AddMessage(message);
            }
        }

        private void InitializeStrategies()
        {
            var sectionEffectiveness = SectionEffectivenessFactory.GetSheaEffectiveness(BeamShearSectionType.Rectangle);
            double longitudinalForce = InputData.ForceTuple.Nz;
            concreteLogic = new(sectionEffectiveness, InputData.InclinedSection, longitudinalForce, TraceLogger);
            stirrupLogic = new(InputData.Stirrup, InputData.InclinedSection, TraceLogger);
        }

        private void PrepareNewResult()
        {
            result = new()
            {
                IsValid = true,
                Description = string.Empty,
                InputData = InputData
            };
        }
    }
}
