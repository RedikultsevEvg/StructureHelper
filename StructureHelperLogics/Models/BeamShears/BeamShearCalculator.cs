using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class BeamShearCalculator : IBeamShearCalculator
    {
        private ICheckInputDataLogic<IBeamShearCalculatorInputData> checkInputDataLogic;
        IGetResultByInputDataLogic<IBeamShearCalculatorInputData, IBeamShearCalculatorResult> calculationLogic;
        private IBeamShearCalculatorResult result;

        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public IBeamShearCalculatorInputData InputData { get; set; } = new BeamShearCalculatorInputData(Guid.NewGuid());

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }


        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            PrepareNewResult();
            PrepareInputData();
            try
            {
                InitializeStrategies();
                if (CheckInputData() == false) { return;}
                CalculateResult();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
            }
        }

        private void PrepareInputData()
        {
            throw new NotImplementedException();
        }

        private bool CheckInputData()
        {
            var checkResult = checkInputDataLogic.Check();
            if (checkResult == false)
            {
                result.IsValid = false;
                result.Description += checkInputDataLogic.CheckResult;
                TraceLogger?.AddMessage(checkInputDataLogic.CheckResult, TraceLogStatuses.Error);
            }
            return checkResult;
        }

        private void CalculateResult()
        {
            result = calculationLogic.GetResultByInputData(InputData);
        }

        private void InitializeStrategies()
        {
            checkInputDataLogic ??= new CheckBeamShearCalculatorInputDataLogic(InputData, TraceLogger);
            calculationLogic ??= new BeamShearCalculatorLogic(TraceLogger);
        }

        private void PrepareNewResult()
        {
            result = new BeamShearCalculatorResult()
            {
                IsValid = true,
                Description = string.Empty
            };
        }
    }
}
