using LoaderCalculator;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.ResultData;
using LoaderCalculator.Data.SourceData;
using LoaderCalculator.Logics;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceTupleCalculator : IForceTupleCalculator, IHasActionByResult
    {
        IForceTupleTraceResultLogic forceTupleTraceResultLogic;
        public IForceTupleInputData InputData { get; set; }
        public string Name { get; set; }
        public IResult Result { get; private set; }

        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public ForceTupleCalculator()
        {
            
        }
        public void Run()
        {
            Result = CalculateResult();
        }

        private IForcesTupleResult CalculateResult()
        {
            TraceLogger?.AddMessage(string.Intern($"Calculator type: {GetType()}"), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(string.Intern("Calculator logic based on calculating strain in plain section by elementary parts of finished size"));
            var ndmCollection = InputData.NdmCollection;
            if (TraceLogger is not null)
            {
                TraceService.TraceNdmCollection(TraceLogger, ndmCollection);
            }
            var tuple = InputData.Tuple;
            var accuracy = InputData.Accuracy;
            TraceLogger?.AddMessage(string.Intern("Input force combination"));
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(tuple));
            var mx = tuple.Mx;
            var my = tuple.My;
            var nz = tuple.Nz;
            TraceLogger?.AddMessage($"Required accuracy rate {accuracy.IterationAccuracy}");
            TraceLogger?.AddMessage($"Maximum iteration count {accuracy.MaxIterationCount}");
            try
            {
                var loaderData = new LoaderOptions
                {
                    Preconditions = new Preconditions
                    {
                        ConditionRate = accuracy.IterationAccuracy,
                        MaxIterationCount = accuracy.MaxIterationCount,
                        StartForceMatrix = new ForceMatrix { Mx = mx, My = my, Nz = nz }
                    },
                    ActionToOutputResults = ShowResultToTrace,
                    NdmCollection = ndmCollection
                };
                var calculator = new Calculator();
                TraceLogger?.AddMessage(string.Intern("Calculation is started"), TraceLogStatuses.Debug);
                calculator.Run(loaderData, new CancellationToken());
                TraceLogger?.AddMessage(string.Intern("Calculation result is obtained"), TraceLogStatuses.Debug);
                var calcResult = calculator.Result;
                if (calcResult.AccuracyRate <= accuracy.IterationAccuracy)
                {
                    var result = new ForcesTupleResult()
                    {
                        IsValid = true,
                        Description = LoggerStrings.CalculationHasDone,
                        LoaderResults = calcResult
                    };
                    forceTupleTraceResultLogic = new ForceTupleTraceResultLogic(ndmCollection) { TraceLogger = TraceLogger };
                    forceTupleTraceResultLogic.TraceResult(result);
                    return result;
                }
                else
                {
                    TraceLogger?.AddMessage(string.Intern("Required accuracy rate has not achieved"), TraceLogStatuses.Error);
                    TraceLogger?.AddMessage($"Current accuracy {calcResult.AccuracyRate}, {calcResult.IterationCounter} iteration has done", TraceLogStatuses.Warning);
                    return new ForcesTupleResult()
                    {
                        IsValid = false,
                        Description = string.Intern("Required accuracy rate has not achieved"),
                        LoaderResults = calcResult
                    };
                }

            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage($"Critical error ocured during calculation\n{ex}", TraceLogStatuses.Error);
                var result = new ForcesTupleResult()
                {
                    IsValid = false
                };
                if (ex.Message == "Calculation result is not valid: stiffness matrix is equal to zero")
                {
                    TraceLogger?.AddMessage(string.Intern("Stiffness matrix is equal to zero\nProbably section was collapsed"), TraceLogStatuses.Error);
                    result.Description = string.Intern("Stiffness matrix is equal to zero\nProbably section was collapsed");
                }
                else
                {
                    result.Description = $"Error is appeared due to analysis. Error: {ex}";
                }
                return result;
            }
        }

        public object Clone()
        {
            var newItem = new ForceTupleCalculator();
            return newItem;
        }

        private static void ShowResultToConsole(ILoaderResults result)
        {
            var strain = result.StrainMatrix;
            //MessageBox.Show($" Текущие результаты  в {result.IterationCounter} итерации:");
        }

        private void ShowResultToTrace(ILoaderResults result)
        {
            var strain = result.StrainMatrix;
            TraceLogger?.AddMessage($"Iteration {result.IterationCounter}, current accuracy rate {result.AccuracyRate}", TraceLogStatuses.Debug,100);
        }
    }
}
