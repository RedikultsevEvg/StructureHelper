using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    public class ActionResultListLogic : IActionResultListLogic
    {
        private const LimitStates CollapseLimitState = LimitStates.ULS;
        private readonly List<CalcTerms> calcTerms = new() { CalcTerms.LongTerm, CalcTerms.ShortTerm };
        private readonly IBeamShearCalculatorInputData inputData;
        private readonly IGetBeamShearSectionIputDatasLogic getBeamShearSectionIputDatasLogic;
        private readonly IGetInclinedSectionListLogic getInclinedSectionListLogic;
        private readonly IInclinedSectionResultListLogic inclinedSectionResultListLogic;
        private readonly ITraceSectionLogic traceSectionLogic;
        private List<IInclinedSection> inclinedSections;
        List<IBeamShearActionResult> actionResults;
        List<IStirrup> stirrups;

        public ActionResultListLogic(
            IBeamShearCalculatorInputData inputData,
            IGetBeamShearSectionIputDatasLogic getBeamShearSectionIputDatasLogic,
            IGetInclinedSectionListLogic getInclinedSectionListLogic,
            IInclinedSectionResultListLogic inclinedSectionResultListLogic,
            ITraceSectionLogic traceSectionLogic,
            IShiftTraceLogger? traceLogger
            )
        {
            this.inputData = inputData;
            this.getBeamShearSectionIputDatasLogic = getBeamShearSectionIputDatasLogic;
            this.getInclinedSectionListLogic = getInclinedSectionListLogic;
            this.inclinedSectionResultListLogic = inclinedSectionResultListLogic;
            this.traceSectionLogic = traceSectionLogic;
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public List<IBeamShearActionResult> GetActionResults()
        {
            PrepareNewResult();
            foreach (var beamShearAction in inputData.Actions)
            {
                getBeamShearSectionIputDatasLogic.Action = beamShearAction;
                foreach (var calcTerm in calcTerms)
                {
                    getBeamShearSectionIputDatasLogic.CalcTerm = calcTerm;
                    foreach (var section in inputData.Sections)
                    {
                        PrepareInclinedSectionList(section);
                        TraceLogger?.AddMessage($"Analysis for action: {beamShearAction.Name}, section: {section.Name}, calc turm {calcTerm} has been started");
                        traceSectionLogic.TraceSection(section);
                        foreach (var stirrup in stirrups)
                        {
                            List<IBeamShearSectionLogicResult> sectionResults = PrepareSectionResult(stirrup);
                            BeamShearActionResult actionResult = GetActionResult(beamShearAction, calcTerm, section, stirrup, sectionResults);
                            actionResults.Add(actionResult);
                        }
                        TraceLogger?.AddMessage($"Analysis for action: {beamShearAction.Name}, section: {section.Name}, calc term {calcTerm} has been finished sucessfull");
                    }
                }
            }
            return actionResults;
        }

        private List<IBeamShearSectionLogicResult> PrepareSectionResult(IStirrup stirrup)
        {
            getBeamShearSectionIputDatasLogic.Stirrup = stirrup;
            List<IBeamShearSectionLogicInputData> sectionInputDatas = getBeamShearSectionIputDatasLogic.GetBeamShearSectionInputDatas();
            List<IBeamShearSectionLogicResult> sectionResults = inclinedSectionResultListLogic.GetInclinedSectionResults(sectionInputDatas);
            return sectionResults;
        }

        private void PrepareInclinedSectionList(IBeamShearSection section)
        {
            getInclinedSectionListLogic.BeamShearSection = section;
            getInclinedSectionListLogic.DesignRangeProperty = inputData.DesignRangeProperty;
            inclinedSections = getInclinedSectionListLogic.GetInclinedSections();
            getBeamShearSectionIputDatasLogic.Section = section;
            getBeamShearSectionIputDatasLogic.InclinedSectionList = inclinedSections;
        }

        private void PrepareNewResult()
        {
            actionResults = new();
            stirrups = inputData.Stirrups.ToList();
            if (stirrups.Any() == false)
            {
                stirrups.Add(new StirrupByDensity(Guid.NewGuid()) { StirrupDensity = 0 });
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
