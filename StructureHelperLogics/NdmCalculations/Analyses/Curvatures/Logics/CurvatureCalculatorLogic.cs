using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models.Sections.Logics;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Services.NdmPrimitives;
using System;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorLogic : ICurvatureCalcualtorLogic
    {
        private const LimitStates limitState = LimitStates.SLS;
        private CurvatureCalculatorResult result;
        private ICurvatureForceCalculator forceCalculator;
        private ICurvatureForceCalculator ForceCalculator => forceCalculator ??= new CurvatureForceCalculator();
        private IPoint2D gravityCenter;

        public ICurvatureCalculatorInputData InputData { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public void Run()
        {
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
                var combinationList = action.GetCombinations();
                foreach (var combination in combinationList)
                {
                    var longTuple = combination.DesignForces.Single(x => x.LimitState == limitState && x.CalcTerm == CalcTerms.LongTerm).ForceTuple;
                    var shortTuple = combination.DesignForces.Single(x => x.LimitState == limitState && x.CalcTerm == CalcTerms.ShortTerm).ForceTuple;
                    if (action.SetInGravityCenter == true)
                    {
                        IProcessorLogic<IForceTuple> forceLogic = new ForceTupleCopier(longTuple);
                        forceLogic = new ForceTupleMoveToPointDecorator(forceLogic) { Point2D = gravityCenter };
                        longTuple = forceLogic.GetValue();
                        forceLogic = new ForceTupleCopier(shortTuple);
                        forceLogic = new ForceTupleMoveToPointDecorator(forceLogic) { Point2D = gravityCenter };
                        shortTuple = forceLogic.GetValue();
                    }
                    CurvatureForceCalculatorInputData forceInputData = new()
                    {
                        LongTermTuple = longTuple,
                        ShortTermTuple = shortTuple,
                        Primitives = InputData.Primitives,
                        DeflectionFactor = InputData.DeflectionFactor
                    };
                    ForceCalculator.InputData = forceInputData;
                    ForceCalculator.Run();
                    result.ForceCalculatorResults.Add((ICurvatureForceCalculatorResult)ForceCalculator.Result);
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
