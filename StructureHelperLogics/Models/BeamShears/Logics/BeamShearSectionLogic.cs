using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears.Logics;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSectionLogic : IBeamShearSectionLogic
    {
        private BeamShearSectionLogicResult result;
        private ISectionEffectiveness sectionEffectiveness;
        private ConcreteShearStrengthLogic concreteLogic;
        private StirrupStrengthLogic stirrupLogic;
        private IGetLongitudinalForceFactorLogic getLongitudinalForceFactorLogic;
        private string sectionMessage;
        private ICheckInputDataLogic<IBeamShearSectionLogicInputData> checkInputDataLogic;
        private double concreteStrength;
        private double stirrupStrength;

        private ShiftTraceLogger? localTraceLogger { get; set; }

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
            sectionMessage = $"Inclined section: start xstart = {InputData.InclinedSection.StartCoord}(m), end xend = {InputData.InclinedSection.EndCoord}(m). ";
            PrepareNewResult();
            if (Check() == false) { return; }
            localTraceLogger?.AddMessage(sectionMessage);
            try
            {
                PrepareResultData();
                InitializeStrategies();
                CalculateResult();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex.Message;
            }
        }

        private void PrepareResultData()
        {
            InclinedSection crackSection = new();
            var updateStrategy = new InclinedSectionUpdateStrategy();
            updateStrategy.Update(crackSection, InputData.InclinedSection);
            BeamShearSectionLogicInputData resultData = new(Guid.Empty);
            var resultDataUpdateStrategy = new BeamShearSectionLogicInputDataUpdateStrategy();
            resultDataUpdateStrategy.Update(resultData, InputData);
            double crackLength = crackSection.EndCoord - crackSection.StartCoord;
            double maxCrackLength = 2 * crackSection.EffectiveDepth;
            if (crackLength > maxCrackLength)
            {
                crackSection.StartCoord = crackSection.EndCoord - maxCrackLength;
            }
            resultData.InclinedCrack = crackSection;
            result.ResultInputData = resultData;
        }

        private bool Check()
        {
            bool checkResult = true;
            checkInputDataLogic ??= new CheckSectionLogicInputDataLogic(TraceLogger);
            checkInputDataLogic.InputData = InputData;
            if (checkInputDataLogic.Check() == false)
            {
                checkResult = false;
                result.IsValid = false;
                string errorMessage = checkInputDataLogic.CheckResult;
                result.Description += errorMessage;
                localTraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
            }
            return checkResult;
        }

        private void CalculateResult()
        {
            SetLongitudinalForce();
            double factorOfLongitudinalForce = getLongitudinalForceFactorLogic.GetFactor();
            localTraceLogger?.AddMessage($"Factor of  longitudinal force = {factorOfLongitudinalForce}, (dimensionless)");
            concreteStrength = concreteLogic.CalculateShearStrength();
            stirrupStrength = stirrupLogic.CalculateShearStrength();
            if (stirrupStrength > concreteStrength)
            {      
                localTraceLogger?.AddMessage($"Shear reinforcement strength Qsw = {stirrupStrength} is greater than concrete strength for shear Qb = {concreteStrength}, shear reinforcement strength has to be restricted.");
                stirrupStrength = GetStirrupStrengthBySearch();
            }
            concreteStrength *= factorOfLongitudinalForce;
            localTraceLogger?.AddMessage($"Concrete strength Qb = {concreteStrength}(N)");
            result.ConcreteStrength = concreteStrength;
            stirrupStrength *= factorOfLongitudinalForce;
            localTraceLogger?.AddMessage($"Stirrup strength Qsw = {stirrupStrength}(N)");
            result.StirrupStrength = stirrupStrength;
            double totalStrength = concreteStrength + stirrupStrength;
            localTraceLogger?.AddMessage($"Total strength = {concreteStrength} + {stirrupStrength} = {totalStrength}(N)");
            result.TotalStrength = totalStrength;
            double actualShearForce = result.ResultInputData.ForceTuple.Qy;
            if (actualShearForce > totalStrength)
            {
                result.IsValid = false;
                string message = $"Actual shear force Qa = {actualShearForce}(N), greater than bearing capacity Olim = {totalStrength}(N)";
                result.Description += message;
                localTraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                TraceLogger?.AddMessage(sectionMessage + message, TraceLogStatuses.Error);
            }
            else
            {
                string message = $"Actual shear force Qa = {actualShearForce}(N), not greater than bearing capacity Olim = {totalStrength}(N)";
                localTraceLogger?.AddMessage(message);
                TraceLogger?.AddMessage(sectionMessage + message);
            }
        }

        private double GetStirrupStrengthBySearch()
        {
            var logic = new StirrupBySearchLogic(localTraceLogger.GetSimilarTraceLogger(100))
            {
                InputData = result.ResultInputData,
                SectionEffectiveness = sectionEffectiveness
            };
            double stirrupStrength = logic.CalculateShearStrength();
            localTraceLogger?.AddMessage($"Stirrup strength was restricted as Qsw,restricted = {stirrupStrength}(N)");
            result.ResultInputData.InclinedCrack = logic.InclinedCrack;
            return stirrupStrength;
        }

        private void InitializeStrategies()
        {
            if (result.ResultInputData.InclinedSection.BeamShearSection.Shape is IRectangleShape)
            {
                sectionEffectiveness = SectionEffectivenessFactory.GetShearEffectiveness(BeamShearSectionType.Rectangle);
            }
            else if (result.ResultInputData.InclinedSection.BeamShearSection.Shape is ICircleShape)
            {
                sectionEffectiveness = SectionEffectivenessFactory.GetShearEffectiveness(BeamShearSectionType.Circle);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(result.ResultInputData.InclinedSection.BeamShearSection.Shape));
            }
            concreteLogic = new(sectionEffectiveness, result.ResultInputData.InclinedSection, localTraceLogger);
            stirrupLogic = new(result.ResultInputData, localTraceLogger);
            getLongitudinalForceFactorLogic = new GetLongitudinalForceFactorLogic(localTraceLogger?.GetSimilarTraceLogger(100));
        }

        private void SetLongitudinalForce()
        {
            getLongitudinalForceFactorLogic.LongitudinalForce = result.ResultInputData.ForceTuple.Nz;
            getLongitudinalForceFactorLogic.InclinedSection = result.ResultInputData.InclinedSection;
        }

        private void PrepareNewResult()
        {
            localTraceLogger = new();
            result = new(localTraceLogger)
            {
                IsValid = true,
                Description = string.Empty,
                InputData = InputData,
            };
        }
    }
}
