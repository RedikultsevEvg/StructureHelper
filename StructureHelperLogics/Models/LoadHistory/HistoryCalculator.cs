using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.LoadHistory.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class HistoryCalculator : IHistoryCalculator
    {
        private const LimitStates limitState = LimitStates.ULS;

        private const CalcTerms calcTerm = CalcTerms.ShortTerm;
        private ITriangulatePrimitiveLogic triangulateLogic;

        private ForceTupleServiceLogic tupleServiceLogic = new();
        private HistoryCalculatorResult result;
        private int totalNumber = 0;
        private double totalTime = 0;
        private bool isCurrentResultValid;
        private ISubStageResult lastFinishResult;
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public bool ShowTraceData { get; set; }
        public List<IConstructionStage> ConstructionStages { get; } = [];
        public IShiftTraceLogger? TraceLogger { get; set; }

        public IResult Result => result;


        public HistoryCalculator(Guid id, IShiftTraceLogger traceLogger = null)
        {
            Id = id;
            TraceLogger = traceLogger;
        }
        public void Run()
        {
            PrepareResult();
            ProcessStages();
        }

        private void ProcessStages()
        {
            for (int stageNumber = 0; stageNumber < ConstructionStages.Count; stageNumber++)
            {
                var stage = ConstructionStages[stageNumber];
                IForceTuple startForceTuple;
                if (stageNumber == 0)
                {
                    startForceTuple = new ForceTuple();
                }
                else
                {
                    startForceTuple = GetForceTuple(ConstructionStages[stageNumber-1]);
                }
                IForceTuple endForceTuple = GetForceTuple(stage);
                List<INdmPrimitive> primitives = [];
                if (isCurrentResultValid == false || stageNumber == 0)
                {
                    primitives.AddRange(stage.Primitives);
                }
                else
                {
                    primitives = GetPrimitives(stageNumber);
                }
                List<INdm> ndms = GetNdms(primitives);
                double timeIncrement = stage.Duration / stage.SubStepNumber;
                double localTime = 0;
                int subStagesCount = stage.SubStepNumber;
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
                    result.StagesResults.Add(subStageResult);
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
                    ForceTupleInputData inputData = new()
                    {
                        NdmCollection = ndms,
                        ForceTuple = forceTuples[subStageNumber]
                    };
                    ForceTupleCalculator forceTupleCalculator = new() { InputData = inputData};
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
                            subStageResult.Description +="Force tuple calculator error: " + e.Message;
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

        private List<INdmPrimitive> GetPrimitives(int stageNumber)
        {
            
            IConstructionStage prevStage = ConstructionStages[stageNumber - 1];
            IConstructionStage curStage = ConstructionStages[stageNumber];
            var prevStageFinishResult = result
                .StagesResults
                .OrderByDescending(x => x.TotalNumber)
                .First(x => x.ConstructionStage == prevStage);
            List<INdmPrimitive> resultNdmList = [];
            foreach (var ndmPrimitive in curStage.Primitives)
            {
                var newPrimitive = ndmPrimitive.Clone() as INdmPrimitive;
                newPrimitive.NdmElement.AutoPrestrain.Clear();
                if (prevStage.Primitives.Contains(ndmPrimitive) && isCurrentResultValid == true)
                {
                    newPrimitive.NdmElement.UsersPrestrain.Nz += prevStageFinishResult.StrainTuple.Nz;
                    newPrimitive.NdmElement.UsersPrestrain.Mx += prevStageFinishResult.StrainTuple.Mx;
                    newPrimitive.NdmElement.UsersPrestrain.My += prevStageFinishResult.StrainTuple.My;
                }
                resultNdmList.Add(newPrimitive);
            }
            return resultNdmList;
        }

        private IForceTuple GetForceTuple(IConstructionStage stage)
        {
            var combinations = stage.ForceAction.GetCombinations();
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
            result = new(this);
            totalNumber = 0;
            totalTime = 0;
            isCurrentResultValid = true;
        }

        public object Clone()
        {
            HistoryCalculatorUpdateStrategy updateStrategy = new() { UpdateChildren = true };
            HistoryCalculator newItem = new(Guid.NewGuid());
            updateStrategy.Update(newItem, this);
            return newItem;
        }

    }
}
