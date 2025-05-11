using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCalculatorLogic : IGetResultByInputDataLogic<IBeamShearCalculatorInputData, IBeamShearCalculatorResult>
    {
        private const LimitStates CollapsLimitState = LimitStates.ULS;
        private IBeamShearCalculatorResult result;
        private IBeamShearSectionLogic beamShearSectionLogic;
        private List<IBeamShearSectionLogicInputData> sectionInputDatas;
        private IBeamShearCalculatorInputData inputData;
        private List<CalcTerms> calcTerms = new() { CalcTerms.LongTerm, CalcTerms.ShortTerm };

        public IShiftTraceLogger? TraceLogger { get; set; }

        public BeamShearCalculatorLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }


        public IBeamShearCalculatorResult GetResultByInputData(IBeamShearCalculatorInputData inputData)
        {
            this.inputData = inputData;
            PrepareNewResult();
            InitializeStrategies();
            try
            {
                GetSectionInputDatas();
                CalculateResult();
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                result.IsValid = false;
            }

            return result;
        }

        private void CalculateResult()
        {
            foreach (var sectionInputData in sectionInputDatas)
            {
                beamShearSectionLogic.InputData = sectionInputData;
                beamShearSectionLogic.Run();
                var sectionResult = beamShearSectionLogic.Result as IBeamShearSectionLogicResult;
                result.SectionResults.Add(sectionResult);
            }
        }

        private void InitializeStrategies()
        {
            beamShearSectionLogic ??= new BeamShearSectionLogic();
        }

        private void PrepareNewResult()
        {
            result = new BeamShearCalculatorResult()
            {
                IsValid = true,
                Description = string.Empty
            };
        }

        private void GetSectionInputDatas()
        {
            sectionInputDatas = new();
            foreach (var beamShearSection in inputData.Sections)
            {
                List<IInclinedSection> inclinedSections = GetInclinedSections(beamShearSection);
                foreach (var inclinedSection in inclinedSections)
                {
                    GetSections(inclinedSection);
                }
            }
        }

        private void GetSections(IInclinedSection inclinedSection)
        {
            foreach (var stirrup in inputData.Stirrups)
            {
                foreach (var beamShearAction in inputData.Actions)
                {
                    foreach (var calcTerm in calcTerms)
                    {
                        IForceTuple forceTuple = GetForceTupleByShearAction(beamShearAction, inclinedSection, CollapsLimitState, calcTerm);
                        BeamShearSectionLogicInputData newInputData = new(Guid.NewGuid())
                        {
                            InclinedSection = inclinedSection,
                            Stirrup = stirrup,
                            ForceTuple = forceTuple,
                            LimitState = CollapsLimitState,
                            CalcTerm = calcTerm
                        };
                        sectionInputDatas.Add(newInputData);
                    }
                }
            }
        }

        private List<IInclinedSection> GetInclinedSections(IBeamShearSection beamShearSection)
        {
            IGetInclinedSectionListInputData inclinedSectionInputDataLogic = new GetInclinedSectionListInputData(beamShearSection);
            IGetInclinedSectionListLogic getInclinedSectionListLogic = new GetInclinedSectionListLogic(inclinedSectionInputDataLogic, TraceLogger);
            return getInclinedSectionListLogic.GetInclinedSections();
        }

        private IForceTuple GetForceTupleByShearAction(IBeamShearAction beamShearAction, IInclinedSection inclinedSection, LimitStates limitState, CalcTerms calcTerm)
        {
            IGetDirectShearForceLogic getDirectShearForceLogic = new GetDirectShearForceLogic(beamShearAction, inclinedSection, limitState, calcTerm, TraceLogger);
            return getDirectShearForceLogic.CalculateShearForceTuple();
        }
    }
}
