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
        private double factorOfLongitudinalForce;

        private ShiftTraceLogger? localTraceLogger { get; set; }
        public ShearCodeTypes ShearCodeType { get; set; }
        public IGetSectionEffectivenessLogic GetSectionEffectivenessLogic { get; set; }
        public IRestrictStirrupCalculator RestrictStirrupCalculator { get; set; }
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
            concreteStrength = concreteLogic.CalculateShearStrength();
            stirrupStrength = stirrupLogic.CalculateShearStrength();
            double factoredConcreteStrength = concreteStrength * factorOfLongitudinalForce;
            localTraceLogger?.AddMessage($"Concrete strength Qb = {concreteStrength} * {factorOfLongitudinalForce} = {factoredConcreteStrength}(N)");
            result.ConcreteStrength = factoredConcreteStrength;
            if (stirrupStrength > factoredConcreteStrength)
            {      
                localTraceLogger?.AddMessage($"Shear reinforcement strength Qsw = {stirrupStrength} is greater than concrete strength for shear Qb = {factoredConcreteStrength}, shear reinforcement strength has to be restricted.");
                stirrupStrength = RestrictStirrupStrength();
            }
            //stirrupStrength *= factorOfLongitudinalForce;
            localTraceLogger?.AddMessage($"Stirrup strength Qsw = {stirrupStrength}(N)");
            result.StirrupStrength = stirrupStrength;
            double totalStrength = factoredConcreteStrength + stirrupStrength;
            localTraceLogger?.AddMessage($"Total strength Qlim = Qb + Qsw = {factoredConcreteStrength} + {stirrupStrength} = {totalStrength}(N)");
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
            TraceLogger?.AddMessage($"Using factor Uf = Qa / Qlim = {actualShearForce} / {totalStrength} = {actualShearForce / totalStrength}");
        }

        private double RestrictStirrupStrength()
        {
            RestrictStirrupCalculator.TraceLogger = localTraceLogger.GetSimilarTraceLogger(100);
            RestrictStirrupCalculator.InputData = result.ResultInputData;
            RestrictStirrupCalculator.SectionEffectiveness = sectionEffectiveness;
            RestrictStirrupCalculator.SourceStirrupStrength = stirrupStrength;
            RestrictStirrupCalculator.ConcreteFactor = factorOfLongitudinalForce;
            RestrictStirrupCalculator.StirrupFactor = 1.0;
            RestrictStirrupCalculator.SourceSection = result.ResultInputData.InclinedCrack;
            RestrictStirrupCalculator.Run();
            var calculatorResult = RestrictStirrupCalculator.Result as RestrictCalculatorResult;
            if (calculatorResult.IsValid == false)
            {
                result.IsValid = false;
                result.Description += calculatorResult.Description;
                return calculatorResult.StirrupStrength;
            }
            else
            {
                result.ResultInputData.InclinedCrack = calculatorResult.InclinedCrack;
                return calculatorResult.StirrupStrength;
            }
        }

        private void InitializeStrategies()
        {
            IShape shape = result.ResultInputData.InclinedSection.BeamShearSection.Shape;
            sectionEffectiveness = GetSectionEffectivenessLogic.GetSectionEffectiveness(ShearCodeType, shape);
            concreteLogic = new(sectionEffectiveness, result.ResultInputData.InclinedSection, localTraceLogger);
            stirrupLogic = new(result.ResultInputData, localTraceLogger);
            getLongitudinalForceFactorLogic = new GetLongitudinalForceFactorLogic(localTraceLogger?.GetSimilarTraceLogger(100));
        }

        private void SetLongitudinalForce()
        {
            getLongitudinalForceFactorLogic.LongitudinalForce = result.ResultInputData.ForceTuple.Nz;
            getLongitudinalForceFactorLogic.InclinedSection = result.ResultInputData.InclinedSection;
            factorOfLongitudinalForce = getLongitudinalForceFactorLogic.GetFactor();
            localTraceLogger?.AddMessage($"Factor of  longitudinal force = {factorOfLongitudinalForce}, (dimensionless)");
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
