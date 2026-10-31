using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperLogics.Models.LoadHistory.Logics
{
    internal class HistoryCalculatorLogic : IHistoryCalculatorLogic
    {
        private const LimitStates limitState = LimitStates.ULS;
        private const CalcTerms calcTerm = CalcTerms.ShortTerm;
        private ITriangulatePrimitiveLogic triangulateLogic;

        private readonly IFirstStagePrimitivesLogic firstStagePrimitivesLogic;
        private readonly INextStagePrimitivesLogic nextStagePrimitivesLogic;

        private ForceTupleServiceLogic tupleServiceLogic = new();
        private HistoryCalculatorResult result;
        private int totalNumber = 0;
        private double totalTime = 0;
        private bool isCurrentResultValid;

        IHistoryCalculatorInputData inputData;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public HistoryCalculatorLogic()
            : this(new FirstStagePrimitivesLogic(), new NextStagePrimitivesLogic())
        {
        }

        internal HistoryCalculatorLogic(
            IFirstStagePrimitivesLogic firstStagePrimitivesLogic,
            INextStagePrimitivesLogic nextStagePrimitivesLogic)
        {
            this.firstStagePrimitivesLogic = firstStagePrimitivesLogic;
            this.nextStagePrimitivesLogic = nextStagePrimitivesLogic;
        }

        public IHistoryCalculatorResult CalculateResult(IHistoryCalculatorInputData data)
        {
            inputData = data;
            PrepareResult();
            ProcessStages();
            return result;
        }


        private void ProcessStages()
        {
            for (int stageNumber = 0; stageNumber < inputData.ConstructionStages.Count; stageNumber++)
            {
                var stage = inputData.ConstructionStages[stageNumber];
                StageResult stageResult = new(stage);
                result.StagesResults.Add(stageResult);
                IForceTuple startForceTuple;
                if (stageNumber == 0)
                {
                    startForceTuple = new ForceTuple();
                }
                else
                {
                    startForceTuple = GetForceTuple(inputData.ConstructionStages[stageNumber - 1]);
                }
                IForceTuple endForceTuple = GetForceTuple(stage);
                if (isCurrentResultValid == false || stageNumber == 0)
                {
                    firstStagePrimitivesLogic.Process(stage, stageResult);
                }
                else
                {
                    nextStagePrimitivesLogic.Process(stage, stageResult, result.StagesResults[stageNumber - 1]);
                }
                var primitives = stageResult.Primitives.Values.ToList();
                List<INdm> ndms = GetNdms(primitives);
                double timeIncrement = stage.InputData.Duration / stage.InputData.SubStepNumber;
                double localTime = 0;
                int subStagesCount = stage.InputData.SubStepNumber;
                List<IForceTuple> forceTuples = tupleServiceLogic.InterpolateTuples(startForceTuple, endForceTuple, subStagesCount);
                for (int subStageNumber = 0; subStageNumber <= subStagesCount; subStageNumber++)
                {
                    StagePosition stagePosition = GetStagePosition(subStageNumber, subStagesCount);
                    SubStageResult subStageResult = new(stage)
                    {
                        StagePosition = stagePosition,
                        LocalNumber = subStageNumber,
                        TotalNumber = totalNumber,
                        LocalTime = localTime,
                        TotalTime = totalTime,
                        Primitives = primitives,
                        ForceTuple = forceTuples[subStageNumber]
                    };
                    subStageResult.StateCalcTermPair.LimitState = limitState;
                    subStageResult.StateCalcTermPair.CalcTerm = calcTerm;
                    stageResult.SubStagesResults.Add(subStageResult);
                    totalNumber++;
                    if (subStageNumber != subStagesCount)
                    {
                        localTime += timeIncrement;
                        totalTime += timeIncrement;
                    }
                    if (isCurrentResultValid == false)
                    {
                        subStageResult.IsValid = false;
                        continue;
                    }
                    ForceTupleInputData forceInputData = new()
                    {
                        NdmCollection = ndms,
                        ForceTuple = forceTuples[subStageNumber],
                        CheckStrainLimit = inputData.CheckStrainLimit,
                        Accuracy = inputData.Accuracy,
                    };
                    ForceTupleCalculator forceTupleCalculator = new()
                    {
                        InputData = forceInputData
                    };
                    try
                    {
                        forceTupleCalculator.Run();
                        var tupleCalculatorResult = forceTupleCalculator.Result as IForceTupleCalculatorResult;
                        if (tupleCalculatorResult.IsValid == true)
                        {
                            subStageResult.ForcesTupleResult = tupleCalculatorResult;
                            subStageResult.StrainTuple.Nz = tupleCalculatorResult.LoaderResults.ForceStrainPair.StrainMatrix.EpsZ;
                            subStageResult.StrainTuple.Mx = tupleCalculatorResult.LoaderResults.ForceStrainPair.StrainMatrix.Kx;
                            subStageResult.StrainTuple.My = tupleCalculatorResult.LoaderResults.ForceStrainPair.StrainMatrix.Ky;
                        }
                        else
                        {
                            subStageResult.IsValid = false;
                            isCurrentResultValid = false;
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        subStageResult.IsValid = false;
                        subStageResult.Description += "Force tuple calculator error: " + e.Message;
                        isCurrentResultValid = false;
                    }
                }
            }
        }

        private List<INdm> GetNdms(List<INdmPrimitive> primitives)
        {
            triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = primitives,
                LimitState = limitState,
                CalcTerm = calcTerm,
                TraceLogger = TraceLogger
            };
            var ndms = triangulateLogic.GetNdms();
            return ndms;
        }

        private IForceTuple GetForceTuple(IConstructionStage stage)
        {
            var combinations = stage.InputData.ForceAction.GetCombinations();
            var designForces = combinations[0].DesignForces;
            IForceTuple forceTuple = designForces.Single(x => x.LimitState == limitState && x.CalcTerm == calcTerm).ForceTuple;
            return forceTuple;
        }

        private StagePosition GetStagePosition(int currentStepNumber, int totalStepNumber)
        {
            if (currentStepNumber == 0) { return StagePosition.Start; }
            if (currentStepNumber == totalStepNumber) { return StagePosition.Finish; }
            return StagePosition.Sub;
        }

        private void PrepareResult()
        {
            result = new(inputData);
            totalNumber = 0;
            totalTime = 0;
            isCurrentResultValid = true;
        }

    }
}
