using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCalculatorLogic : IGetResultByInputDataLogic<IBeamShearCalculatorInputData, IBeamShearCalculatorResult>
    {
        private const LimitStates CollapseLimitState = LimitStates.ULS;
        private IBeamShearCalculatorResult result;
        private IBeamShearSectionLogic beamShearSectionLogic;
        private List<IBeamShearActionResult> actionResults;
        private IBeamShearCalculatorInputData inputData;
        private readonly List<CalcTerms> calcTerms = new() { CalcTerms.LongTerm, CalcTerms.ShortTerm };

        public IShiftTraceLogger? TraceLogger { get; set; }

        public BeamShearCalculatorLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }


        public IBeamShearCalculatorResult GetResultByInputData(IBeamShearCalculatorInputData inputData)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            this.inputData = inputData;
            PrepareNewResult();
            InitializeStrategies();
            try
            {
                GetSections();
                result.ActionResults = actionResults;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                result.IsValid = false;
            }

            return result;
        }

        private IBeamShearSectionLogicResult CalculateResult(IBeamShearSectionLogicInputData sectionInputData)
        {
            beamShearSectionLogic.InputData = sectionInputData;
            beamShearSectionLogic.Run();
            var sectionResult = beamShearSectionLogic.Result as IBeamShearSectionLogicResult;
            return sectionResult;
        }

        private void InitializeStrategies()
        {
            beamShearSectionLogic ??= new BeamShearSectionLogic(TraceLogger);
        }

        private void PrepareNewResult()
        {
            result = new BeamShearCalculatorResult()
            {
                IsValid = true,
                Description = string.Empty
            };
        }

        private void GetSections()
        {
            actionResults = new();
            List<IStirrup> stirrups = inputData.Stirrups.ToList();
            if (stirrups.Any() == false)
            {
                stirrups.Add(new StirrupByDensity(Guid.NewGuid()) { StirrupDensity = 0 });
            }
            foreach (var beamShearAction in inputData.Actions)
            {
                foreach (var calcTerm in calcTerms)
                {
                    foreach (var section in inputData.Sections)
                    {
                        foreach (var stirrup in stirrups)
                        {
                            List<IInclinedSection> inclinedSections = GetInclinedSections(section);
                            List<IBeamShearSectionLogicInputData> sectionInputDatas = GetSectionInputDatas(beamShearAction, calcTerm, section, stirrup, inclinedSections);
                            List<IBeamShearSectionLogicResult> sectionResults = GetSectionResults(sectionInputDatas);
                            BeamShearActionResult actionResult = GetActionResult(beamShearAction, calcTerm, section, stirrup, sectionResults);
                            actionResults.Add(actionResult);
                        }
                    }
                }
            }
        }

        private BeamShearActionResult GetActionResult(IBeamShearAction beamShearAction, CalcTerms calcTerm, IBeamShearSection section, IStirrup stirrup, List<IBeamShearSectionLogicResult> sectionResults)
        {
            BeamShearActionResult actionResult = PrepareNewActionResult(beamShearAction, calcTerm, section, stirrup);
            if (sectionResults.Any(x => x.IsValid == false))
            {
                actionResult.IsValid = false;
                if (actionResult.Description.Length > 0)
                {
                    actionResult.Description += "\n";
                }
                actionResult.Description += $"There are {sectionResults.Count(x => x.IsValid == false)} invalid section result(s)";
            }
            actionResult.SectionResults = sectionResults;
            return actionResult;
        }

        private List<IBeamShearSectionLogicResult> GetSectionResults(List<IBeamShearSectionLogicInputData> sectionInputDatas)
        {
            List<IBeamShearSectionLogicResult> sectionResults = new();
            foreach (var item in sectionInputDatas)
            {
                IBeamShearSectionLogicResult sectionResult = CalculateResult(item);
                sectionResults.Add(sectionResult);
            }

            return sectionResults;
        }

        private List<IBeamShearSectionLogicInputData> GetSectionInputDatas(IBeamShearAction beamShearAction, CalcTerms calcTerm, IBeamShearSection section, IStirrup stirrup, List<IInclinedSection> inclinedSections)
        {
            List<IBeamShearSectionLogicInputData> sectionInputDatas = new();
            var material = section.Material;
            var strength = material.GetStrength(CollapseLimitState, calcTerm);
            foreach (var inclinedSection in inclinedSections)
            {
                inclinedSection.ConcreteCompressionStrength = strength.Compressive;
                inclinedSection.ConcreteTensionStrength = strength.Tensile;
                IForceTuple forceTuple = GetForceTupleByShearAction(beamShearAction, inclinedSection, CollapseLimitState, calcTerm);
                BeamShearSectionLogicInputData newInputData = new(Guid.NewGuid())
                {
                    InclinedSection = inclinedSection,
                    Stirrup = stirrup,
                    ForceTuple = forceTuple,
                    LimitState = CollapseLimitState,
                    CalcTerm = calcTerm
                };
                sectionInputDatas.Add(newInputData);
            }

            return sectionInputDatas;
        }

        private static BeamShearActionResult PrepareNewActionResult(IBeamShearAction beamShearAction, CalcTerms calcTerm, IBeamShearSection section, IStirrup stirrup)
        {
            return new()
            {
                IsValid = true,
                Description = string.Empty,
                BeamShearAction = beamShearAction,
                LimitState = CollapseLimitState,
                CalcTerm = calcTerm,
                Section = section,
                Stirrup = stirrup
            };
        }

        private List<IInclinedSection> GetInclinedSections(IBeamShearSection beamShearSection)
        {
            IGetInclinedSectionListInputData inclinedSectionInputDataLogic = new GetInclinedSectionListInputData(beamShearSection);
            //IGetInclinedSectionListLogic getInclinedSectionListLogic = new GetInclinedSectionListLogic(inclinedSectionInputDataLogic, TraceLogger);
            IGetInclinedSectionListLogic getInclinedSectionListLogic = new GetInclinedSectionListLogic(inclinedSectionInputDataLogic, null);
            return getInclinedSectionListLogic.GetInclinedSections();
        }

        private IForceTuple GetForceTupleByShearAction(IBeamShearAction beamShearAction, IInclinedSection inclinedSection, LimitStates limitState, CalcTerms calcTerm)
        {
            //IGetDirectShearForceLogic getDirectShearForceLogic = new GetDirectShearForceLogic(beamShearAction, inclinedSection, limitState, calcTerm, TraceLogger);
            IGetDirectShearForceLogic getDirectShearForceLogic = new GetDirectShearForceLogic(beamShearAction, inclinedSection, limitState, calcTerm, null);
            return getDirectShearForceLogic.CalculateShearForceTuple();
        }
    }
}
