using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models.Sections.Logics;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Services.NdmPrimitives;
using System;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorLogic : ICurvatureCalcualtorLogic
    {
        private const LimitStates limitState = LimitStates.SLS;
        private CurvatureCalculatorResult result;
        private ICurvatureForceCalculator forceCalculator;
        private ICurvatureForceCalculator ForceCalculator => forceCalculator ??= new CurvatureForceCalculator(TraceLogger);
        private IPoint2D gravityCenter;

        public ICurvatureCalculatorInputData InputData { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public void Run()
        {
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Bending deflection is calculated by expression delta = S * k * L ^ 2");
            TraceLogger?.AddMessage($"Longitudinal deflection is calculated by expression delta = S * k * L");
            PrepareNewResult();
            SetGravityCenter();
            try
            {
                Calculate();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex.Message;
            }
        }

        private void Calculate()
        {
            foreach (var action in InputData.ForceActions)
            {
                TraceLogger?.AddMessage($"Calculation for action {action.Name} has been started");
                var combinationList = action.GetCombinations();
                foreach (var combination in combinationList)
                {
                    TraceLogger?.AddMessage($"Totally {combination.DesignForces.Count} combinations has been extracted successfully");
                    var pairList = ForceActionService.ConvertCombinationToPairs(combination).Where(x => x.LimitState == limitState).ToList();
                    foreach (var pair in pairList)
                    {
                        pair.Name = action.Name;
                        TraceLogger?.AddMessage($"Calculation for combination of force {pair.Name} has been started");
                        if (action.SetInGravityCenter == true)
                        {
                            IProcessorLogic<IForceTuple> forceLogic = new ForceTupleCopier(pair.LongForceTuple);
                            forceLogic = new ForceTupleMoveToPointDecorator(forceLogic) { Point2D = gravityCenter };
                            pair.LongForceTuple = forceLogic.GetValue();
                            forceLogic = new ForceTupleCopier(pair.FullForceTuple);
                            forceLogic = new ForceTupleMoveToPointDecorator(forceLogic) { Point2D = gravityCenter };
                            pair.FullForceTuple = forceLogic.GetValue();
                        }
                        CurvatureForceCalculatorInputData forceInputData = new()
                        {
                            ForcePair = pair,
                            Primitives = InputData.Primitives,
                            DeflectionFactor = InputData.DeflectionFactor
                        };
                        ForceCalculator.InputData = forceInputData;
                        ForceCalculator.Run();
                        ICurvatureForceCalculatorResult forceResult = (ICurvatureForceCalculatorResult)ForceCalculator.Result;
                        result.ForceCalculatorResults.Add(forceResult);
                    }

                }
            }
        }

        private void SetGravityCenter()
        {
            var triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = InputData.Primitives,
                LimitState = LimitStates.ULS,
                CalcTerm = CalcTerms.ShortTerm,
                TraceLogger = TraceLogger
            };
            var ndms = triangulateLogic.GetNdms();
            var (Cx, Cy) = LoaderCalculator.Logics.Geometry.GeometryOperations.GetGravityCenter(ndms);
            gravityCenter = new Point2D() { X = Cx, Y = Cy };
        }

        private void PrepareNewResult()
        {
            result = new CurvatureCalculatorResult()
            {
                IsValid = true,
                Description = string.Empty,
                InputData = InputData
            };
        }
    }
}
