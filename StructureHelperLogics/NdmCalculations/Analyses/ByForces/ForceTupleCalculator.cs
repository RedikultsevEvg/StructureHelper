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

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceTupleCalculator : IForceTupleCalculator, IHasActionByResult
    {
        public IForceTupleInputData InputData { get; set; }
        public string Name { get; set; }
        public IResult Result { get; private set; }

        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public ForceTupleCalculator(IForceTupleInputData inputData)
        {
            InputData = inputData;
        }
        public ForceTupleCalculator()
        {
            
        }
        public void Run()
        {
            Result = CalculateResult();
        }

        private IForcesTupleResult CalculateResult()
        {
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Calculator logic based on calculating strain in plain section by elementary parts of finished size");
            var ndmCollection = InputData.NdmCollection;
            TraceLogger?.AddMessage($"Collection of elementary parts contains {ndmCollection.Count()} parts");
            TraceLogger?.AddMessage($"Summary area of elementary part collection = {ndmCollection.Sum(x => x.Area * x.StressScale)}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Minimum x = {ndmCollection.Min(x => x.CenterX)}", TraceLogStatuses.Debug);
            TraceLogger?.AddMessage($"Maximum x = {ndmCollection.Max(x => x.CenterX)}", TraceLogStatuses.Debug);
            TraceLogger?.AddMessage($"Minimum y = {ndmCollection.Min(x => x.CenterY)}", TraceLogStatuses.Debug);
            TraceLogger?.AddMessage($"Maximum y = {ndmCollection.Max(x => x.CenterY)}", TraceLogStatuses.Debug);
            var tuple = InputData.Tuple;
            var accuracy = InputData.Accuracy;
            TraceLogger?.AddMessage($"Input force combination");
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
                TraceLogger?.AddMessage($"Calculation is started", TraceLogStatuses.Debug);
                calculator.Run(loaderData, new CancellationToken());
                TraceLogger?.AddMessage($"Calculation result is obtained", TraceLogStatuses.Debug);
                var calcResult = calculator.Result;
                if (calcResult.AccuracyRate <= accuracy.IterationAccuracy)
                {
                    TraceGoodResult(ndmCollection, calcResult);
                    return new ForcesTupleResult()
                    {
                        IsValid = true,
                        Description = LoggerStrings.CalculationHasDone,
                        LoaderResults = calcResult
                    };
                }
                else
                {
                    TraceLogger?.AddMessage($"Required accuracy rate has not achieved", TraceLogStatuses.Error);
                    TraceLogger?.AddMessage($"Current accuracy {calcResult.AccuracyRate}, {calcResult.IterationCounter} iteration has done", TraceLogStatuses.Warning);
                    return new ForcesTupleResult()
                    {
                        IsValid = false,
                        Description = "Required accuracy rate has not achieved",
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
                    TraceLogger?.AddMessage($"Stiffness matrix is equal to zero\nProbably section was collapsed", TraceLogStatuses.Error);
                    result.Description = "Stiffness matrix is equal to zero\nProbably section was collapsed";
                }
                else
                {
                    result.Description = $"Error is appeared due to analysis. Error: {ex}";
                }
                return result;
            }
        }

        private void TraceGoodResult(IEnumerable<INdm> ndmCollection, ILoaderResults calcResult)
        {
            TraceLogger?.AddMessage($"Analysis is done succsesfully");
            TraceLogger?.AddMessage($"Current accuracy {calcResult.AccuracyRate} has achieved in {calcResult.IterationCounter} iteration", TraceLogStatuses.Debug);
            var strainMatrix = calcResult.ForceStrainPair.StrainMatrix;
            var stiffness = new StiffnessLogic().GetStiffnessMatrix(ndmCollection, strainMatrix);
            TraceLogger?.AddMessage(string.Format("Next strain were obtained kx = {0}, ky = {1}, epsz = {2}", strainMatrix.Kx, strainMatrix.Ky, strainMatrix.EpsZ));
            TraceMinMaxStrain(ndmCollection, strainMatrix);
            TraceStrainAndStiffness(strainMatrix, stiffness);
        }

        private void TraceMinMaxStrain(IEnumerable<INdm> ndmCollection, IStrainMatrix strainMatrix)
        {
            var stressLogic = new StressLogic();
            double minStrain = double.PositiveInfinity, maxStrain = double.NegativeInfinity;
            Point2D minPoint = new Point2D(), maxPoint = new Point2D();
            foreach (var item in ndmCollection)
            {
                var strain = stressLogic.GetTotalStrain(strainMatrix, item);
                if (strain < minStrain)
                {
                    minStrain = strain;
                    minPoint = new Point2D() { X = item.CenterX, Y = item.CenterY };
                }

                if (strain > maxStrain)
                {
                    maxStrain = strain;
                    maxPoint = new Point2D() { X = item.CenterX, Y = item.CenterY };
                }
            }
            TraceLogger?.AddMessage(string.Format("Max strain EpsilonMax = {0}, at point x = {1}, y = {2}", maxStrain, maxPoint.X, maxPoint.Y), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage(string.Format("Min strain EpsilonMin = {0}, at point x = {1}, y = {2}", minStrain, minPoint.X, minPoint.Y), TraceLogStatuses.Debug);
        }

        private void TraceStrainAndStiffness(IStrainMatrix strain, IStiffnessMatrix stiffness)
        {
            TraceLogger?.AddMessage("Stiffness matrix");
            TraceLogger?.AddMessage(string.Format("D11 = {0}, D12 = {1}, D13 = {2}", stiffness[0, 0], stiffness[0, 1], stiffness[0, 2]));
            TraceLogger?.AddMessage(string.Format("D21 = {0}, D22 = {1}, D23 = {2}", stiffness[1, 0], stiffness[1, 1], stiffness[1, 2]));
            TraceLogger?.AddMessage(string.Format("D31 = {0}, D32 = {1}, D33 = {2}", stiffness[2, 0], stiffness[2, 1], stiffness[2, 2]));
            TraceLogger?.AddMessage("Checking equilibrium equations");
            var exitMx = stiffness[0, 0] * strain.Kx + stiffness[0, 1] * strain.Ky + stiffness[0, 2] * strain.EpsZ;
            var exitMy = stiffness[1, 0] * strain.Kx + stiffness[1, 1] * strain.Ky + stiffness[1, 2] * strain.EpsZ;
            var exitNz = stiffness[2, 0] * strain.Kx + stiffness[2, 1] * strain.Ky + stiffness[2, 2] * strain.EpsZ;
            TraceLogger?.AddMessage(string.Format("D11 * kx + D12 * ky + D13 * epsz =\n {0} * {1} + {2} * {3} + {4} * {5} = {6}",
                stiffness[0, 0], strain.Kx,
                stiffness[0, 1], strain.Ky,
                stiffness[0, 2], strain.EpsZ,
                exitMx
                ));
            TraceLogger?.AddMessage(string.Format("D12 * kx + D22 * ky + D23 * epsz =\n {0} * {1} + {2} * {3} + {4} * {5} = {6}",
                stiffness[1, 0], strain.Kx,
                stiffness[1, 1], strain.Ky,
                stiffness[1, 2], strain.EpsZ,
                exitMy
                ));
            TraceLogger?.AddMessage(string.Format("D31 * kx + D32 * ky + D33 * epsz =\n {0} * {1} + {2} * {3} + {4} * {5} = {6}",
                stiffness[2, 0], strain.Kx,
                stiffness[2, 1], strain.Ky,
                stiffness[2, 2], strain.EpsZ,
                exitNz
                ));
            TraceLogger?.AddMessage($"Output force combination");
            var outputTuple = new ForceTuple()
            {
                Mx = exitMx,
                My = exitMy,
                Nz = exitNz
            };
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(outputTuple));
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
