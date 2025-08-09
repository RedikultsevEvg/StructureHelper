using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCalculatorLogic : IGetResultByInputDataLogic<IBeamShearCalculatorInputData, IBeamShearCalculatorResult>
    {
        private const LimitStates CollapseLimitState = LimitStates.ULS;
        private readonly List<CalcTerms> calcTerms = new() { CalcTerms.LongTerm, CalcTerms.ShortTerm };
        private IBeamShearCalculatorResult result;
        private List<IBeamShearActionResult> actionResults;
        private IBeamShearCalculatorInputData inputData;
        private List<IInclinedSection> inclinedSections;
        private IBeamShearSectionLogic beamShearSectionLogic;
        private IGetBeamShearSectionIputDatasLogic getBeamShearSectionIputDatasLogic;
        private IGetInclinedSectionListLogic getInclinedSectionListLogic;

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
                GetActionResults();
                result.ActionResults = actionResults;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                result.IsValid = false;
                result.Description += "\n" + ex.Message;
            }

            return result;
        }

        private void TraceSection(IBeamShearSection section)
        {
            List<IHeadMaterial> headMaterials = new()
                {
                    new HeadMaterial(Guid.Empty)
                    {
                        Name = $"{section.Name}.Concrete",
                        HelperMaterial = section.ConcreteMaterial
                    },
                    new HeadMaterial(Guid.Empty)
                    {
                        Name = $"{section.Name}.Reinforcement",
                        HelperMaterial = section.ReinforcementMaterial
                    },
                };
            var traceLogic = new TraceMaterialsFactory()
            {
                Collection = headMaterials
            };
            traceLogic.AddEntriesToTraceLogger(TraceLogger);
        }

        private IBeamShearSectionLogicResult CalculateInclinedSectionResult(IBeamShearSectionLogicInputData sectionInputData)
        {
            beamShearSectionLogic.InputData = sectionInputData;
            beamShearSectionLogic.Run();
            var sectionResult = beamShearSectionLogic.Result as IBeamShearSectionLogicResult;
            return sectionResult;
        }

        private void InitializeStrategies()
        {
            beamShearSectionLogic ??= new BeamShearSectionLogic(TraceLogger);
            getInclinedSectionListLogic ??= new GetInclinedSectionListLogic(null);
            getBeamShearSectionIputDatasLogic ??= new GetBeamShearSectionIputDatasLogic();
        }

        private void PrepareNewResult()
        {
            result = new BeamShearCalculatorResult()
            {
                IsValid = true,
                Description = string.Empty
            };
        }

        private void GetActionResults()
        {
            actionResults = new();
            List<IStirrup> stirrups = inputData.Stirrups.ToList();
            if (stirrups.Any() == false)
            {
                stirrups.Add(new StirrupByDensity(Guid.NewGuid()) { StirrupDensity = 0 });
            }
            foreach (var beamShearAction in inputData.Actions)
            {
                getBeamShearSectionIputDatasLogic.Action = beamShearAction;
                foreach (var calcTerm in calcTerms)
                {
                    getBeamShearSectionIputDatasLogic.CalcTerm = calcTerm;
                    foreach (var section in inputData.Sections)
                    {
                        getInclinedSectionListLogic.BeamShearSection = section;
                        getInclinedSectionListLogic.DesignRangeProperty = inputData.DesignRangeProperty;
                        inclinedSections = getInclinedSectionListLogic.GetInclinedSections();
                        getBeamShearSectionIputDatasLogic.Section = section;
                        getBeamShearSectionIputDatasLogic.InclinedSectionList = inclinedSections;
                        TraceLogger?.AddMessage($"Analysis for action: {beamShearAction.Name}, section: {section.Name}, calc turm {calcTerm} has been started");
                        TraceSection(section);
                        foreach (var stirrup in stirrups)
                        {
                            getBeamShearSectionIputDatasLogic.Stirrup = stirrup;
                            List<IBeamShearSectionLogicInputData> sectionInputDatas = getBeamShearSectionIputDatasLogic.GetBeamShearSectionInputDatas();
                            List<IBeamShearSectionLogicResult> sectionResults = GetInclinedSectionResults(sectionInputDatas);
                            BeamShearActionResult actionResult = GetActionResult(beamShearAction, calcTerm, section, stirrup, sectionResults);
                            actionResults.Add(actionResult);
                        }
                        TraceLogger?.AddMessage($"Analysis for action: {beamShearAction.Name}, section: {section.Name}, calc term {calcTerm} has been finished sucessfull");
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

        private List<IBeamShearSectionLogicResult> GetInclinedSectionResults(List<IBeamShearSectionLogicInputData> sectionInputDatas)
        {
            List<IBeamShearSectionLogicResult> sectionResults = new();
            foreach (var item in sectionInputDatas)
            {
                IBeamShearSectionLogicResult sectionResult = CalculateInclinedSectionResult(item);
                sectionResults.Add(sectionResult);
            }
            return sectionResults;
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
    }
}
