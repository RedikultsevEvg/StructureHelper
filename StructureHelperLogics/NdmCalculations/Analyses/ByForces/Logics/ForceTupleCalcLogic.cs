using LoaderCalculator.Data.Matrix;
using LoaderCalculator;
using LoaderCalculator.Data.ResultData;
using LoaderCalculator.Data.SourceData;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceTupleCalcLogic : IForceTupleCalcLogic
    {
        private IForcesTupleResult result;
        private ForceTupleTraceResultLogic forceTupleTraceResultLogic;
        private LoaderOptions loaderData;
        private Calculator calculator;
        private ILoaderResults calcResult;

        public IForceTupleInputData InputData { get; set; }

        public IForcesTupleResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public Action<IResult> ActionToOutputResults { get; set; }

        public void Calculate()
        {
            PrepareNewResult();
            CalculateResult();
        }

        private void PrepareNewResult()
        {
            result = new ForcesTupleResult()
            {
                IsValid = true,
                Description = string.Empty,
            };
        }

        private void CalculateResult()
        {
            TraceStartOfCalculation();
            try
            {
                GetLoaderResult();
                ProcessLoaderResult();

            }
            catch (Exception ex)
            {
                ProcessCalculationException(ex);
            }
        }

        private void TraceStartOfCalculation()
        {
            TraceLogger?.AddMessage(string.Intern($"Calculator type: {GetType()}"), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(string.Intern("Calculator logic based on calculating strain in plain section by elementary parts of finished size"));
            if (TraceLogger is not null)
            {
                TraceService.TraceNdmCollection(TraceLogger, InputData.NdmCollection);
            }
            TraceLogger?.AddMessage(string.Intern("Input force combination"));
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(InputData.Tuple));
            TraceLogger?.AddMessage($"Required accuracy rate {InputData.Accuracy.IterationAccuracy}");
            TraceLogger?.AddMessage($"Maximum iteration count {InputData.Accuracy.MaxIterationCount}");
        }

        private void ProcessCalculationException(Exception ex)
        {
            TraceLogger?.AddMessage($"Critical error ocured during calculation\n{ex}", TraceLogStatuses.Error);
            result.IsValid = false;
            if (ex.Message == "Calculation result is not valid: stiffness matrix is equal to zero")
            {
                TraceLogger?.AddMessage(string.Intern("Stiffness matrix is equal to zero\nProbably section was collapsed"), TraceLogStatuses.Error);
                result.Description = string.Intern("Stiffness matrix is equal to zero\nProbably section was collapsed");
            }
            else
            {
                result.Description = $"Error is appeared due to analysis. Error: {ex}";
            }
        }

        private void ProcessLoaderResult()
        {
            if (calcResult.AccuracyRate <= InputData.Accuracy.IterationAccuracy)
            {
                ProcessCorrectLoaderResult();
            }
            else
            {
                ProcessInCorrectLoaderResult();
            }
        }

        private void ProcessInCorrectLoaderResult()
        {
            TraceLogger?.AddMessage(string.Intern("Required accuracy rate has not achieved"), TraceLogStatuses.Error);
            TraceLogger?.AddMessage($"Current accuracy {calcResult.AccuracyRate}, {calcResult.IterationCounter} iteration has done", TraceLogStatuses.Warning);
            result.IsValid = false;
            result.Description = string.Intern("Required accuracy rate has not achieved");
            result.LoaderResults = calcResult;
        }

        private void ProcessCorrectLoaderResult()
        {
            result.IsValid = true;
            result.Description = LoggerStrings.CalculationHasDone;
            result.LoaderResults = calcResult;
            forceTupleTraceResultLogic = new ForceTupleTraceResultLogic(InputData.NdmCollection)
            {
                TraceLogger = TraceLogger
            };
            forceTupleTraceResultLogic.TraceResult(result);
        }

        private void GetLoaderResult()
        {
            loaderData = new LoaderOptions
            {
                Preconditions = new Preconditions
                {
                    ConditionRate = InputData.Accuracy.IterationAccuracy,
                    MaxIterationCount = InputData.Accuracy.MaxIterationCount,
                    StartForceMatrix = new ForceMatrix
                    {
                        Mx = InputData.Tuple.Mx,
                        My = InputData.Tuple.My,
                        Nz = InputData.Tuple.Nz
                    }
                },
                ActionToOutputResults = ShowResultToTrace,
                NdmCollection = InputData.NdmCollection
            };
            calculator = new Calculator();
            TraceLogger?.AddMessage(string.Intern("Calculation is started"), TraceLogStatuses.Debug);
            calculator.Run(loaderData, new CancellationToken());
            TraceLogger?.AddMessage(string.Intern("Calculation result is obtained"), TraceLogStatuses.Debug);
            calcResult = calculator.Result;
        }

        private void ShowResultToTrace(ILoaderResults result)
        {
            var strain = result.StrainMatrix;
            TraceLogger?.AddMessage($"Iteration {result.IterationCounter}, current accuracy rate {result.AccuracyRate}", TraceLogStatuses.Debug, 100);
        }
    }
}
