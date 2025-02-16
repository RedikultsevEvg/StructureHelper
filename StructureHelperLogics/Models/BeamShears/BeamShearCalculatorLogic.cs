using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCalculatorLogic : IGetResultByInputDataLogic<IBeamShearCalculatorInputData, IBeamShearCalculatorResult>
    {
        private IBeamShearCalculatorResult result;
        private IBeamShearSectionCalculator beamShearSectionCalculator;
        private List<IBeamShearSectionCalculatorInputData> sectionInputDatas;

        public BeamShearCalculatorLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public IBeamShearCalculatorResult GetResultByInputData(IBeamShearCalculatorInputData inputData)
        {
            PrepareNewResult();
            InitializeStrategies();
            try
            {
                GetSectionInputDatas(inputData);
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
                beamShearSectionCalculator.InputData = sectionInputData;
                beamShearSectionCalculator.Run();
                var sectionResult = beamShearSectionCalculator.Result as IBeamShearSectionCalculatorResult;
                result.SectionResults.Add(sectionResult);
            }
        }

        private void InitializeStrategies()
        {
            beamShearSectionCalculator ??= new BeamShearSectionCalculator();
        }

        private void PrepareNewResult()
        {
            result = new BeamShearCalculatorResult()
            {
                IsValid = true,
                Description = string.Empty
            };
        }

        private void GetSectionInputDatas(IBeamShearCalculatorInputData inputData)
        {
            sectionInputDatas = new();
            foreach (var beamShearSection in inputData.ShearSections)
            {
                foreach (var stirrup in inputData.Stirrups)
                {
                    foreach (var beamShearAction in inputData.BeamShearActions)
                    {
                        BeamShearSectionCalculatorInputData newInputData = new(Guid.NewGuid())
                        {
                            BeamShearSection = beamShearSection,
                            Stirrup = stirrup,
                            BeamShearAction = beamShearAction
                        };
                        sectionInputDatas.Add(newInputData);
                    }
                }
            }
        }
    }
}
