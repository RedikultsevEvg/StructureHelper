using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Calculations.CalculationsResults;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve.Factories;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurvesCalculator : ISaveable, ICalculator, IHasActionByResult
    {
        private LimitCurvesResult result;
        private int curvesIterationCount;
        private LimitCurvesCalculatorUpdateStrategy updateStrategy => new();

        public Guid Id { get; }
        public string Name { get; set; }
        public LimitCurveInputData InputData { get; set; }
        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public LimitCurvesCalculator()
        {
            Name = "New calculator";
            InputData = new();
        }
        public void Run()
        {
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLoggerStatuses.Service);
            TraceLogger?.AddMessage($"Start solution in calculator {Name}");
            GetNewResult();
            try
            {
                var calculators = GetCalulators();
                curvesIterationCount = 0;
                foreach (var item in calculators)
                {
                    item.Run();
                    var locResult = item.Result as LimitCurveResult;
                    result.LimitCurveResults.Add(locResult);
                    if (locResult.IsValid == false)
                    {
                        result.Description += locResult.Description;
                    }
                    result.IterationNumber = curvesIterationCount * InputData.PointCount + locResult.IterationNumber;
                    ActionToOutputResults?.Invoke(result);
                    curvesIterationCount++;
                }
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage($"Calculation result is not valid: {ex.Message}", TraceLoggerStatuses.Error);
                result.IsValid = false;
                result.Description += ex;
            }
        }

        private void GetNewResult()
        {
            result = new()
            {
                IsValid = true
            };
        }

        private List<ILimitCurveCalculator> GetCalulators()
        {
            List<ILimitCurveCalculator> calculators = new();
            foreach (var primitiveSeries in InputData.PrimitiveSeries)
            {
                foreach (var limitState in InputData.LimitStates)
                {
                    foreach (var calcTerm in InputData.CalcTerms)
                    {
                        var ndms = NdmPrimitivesService.GetNdms(primitiveSeries.Collection, limitState, calcTerm);
                        foreach (var predicateEntry in InputData.PredicateEntries)
                        {
                            string calcName = $"{primitiveSeries.Name}_{predicateEntry.Name}_{limitState}_{calcTerm}";
                            LimitCurveCalculator calculator = GetCalculator(ndms, predicateEntry.PredicateType, calcName);
                            if (TraceLogger is not null)
                            {
                                calculator.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
                            }
                            calculators.Add(calculator);
                        }
                    }
                }
            }
            return calculators;
        }

        private LimitCurveCalculator GetCalculator(List<INdm> ndms, PredicateTypes predicateType, string calcName)
        {
            var getPredicateLogic = new GetPredicateLogic()
            {
                Name = calcName,
                Ndms = ndms,
                ConvertLogic = InputData.SurroundData.ConvertLogicEntity.ConvertLogic,
                PredicateType = predicateType
            };
            if (TraceLogger is not null) { getPredicateLogic.TraceLogger = TraceLogger; }
            var logic = new LimitCurveLogic(getPredicateLogic);
            var calculator = new LimitCurveCalculator(logic)
            {
                Name = calcName,
                SurroundData = InputData.SurroundData,
                PointCount = InputData.PointCount,
                ActionToOutputResults = SetCurveCount
            };
            return calculator;
        }

        public object Clone()
        {
            var newItem = new LimitCurvesCalculator();
            updateStrategy.Update(newItem, this);
            return newItem;
        }

        private void SetCurveCount(IResult locResult)
        {
            var curveResult = locResult as IiterationResult;
            result.IterationNumber = curvesIterationCount * InputData.PointCount + curveResult.IterationNumber;
            ActionToOutputResults?.Invoke(result);
        }

    }
}
