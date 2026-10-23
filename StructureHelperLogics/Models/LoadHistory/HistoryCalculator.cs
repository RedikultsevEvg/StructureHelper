using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.LoadHistory.Logics;
using StructureHelperLogics.Models.Primitives;
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

        public IHistoryCalculatorInputData InputData { get; }

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
                StageResult stageResult = new(stage);
                result.StagesResults.Add(stageResult);
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
                if (isCurrentResultValid == false || stageNumber == 0)
                {
                    
                    ProcessPrimitivesByCurrentStage(stageNumber);
                }
                else
                {
                    ProcessPrimitivesByPreviousStage(stageNumber);
                }
                var primitives = stageResult.Primitives.Values.ToList();
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
                    ForceTupleInputData inputData = new()
                    {
                        NdmCollection = ndms,
                        ForceTuple = forceTuples[subStageNumber],
                        CheckStrainLimit = true
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

        private void ProcessPrimitivesByCurrentStage(int stageNumber)
        {
            IConstructionStage currentStage = ConstructionStages[stageNumber];
            IStageResult currentStageResult = result.StagesResults[stageNumber];
            Dictionary<INdmPrimitive, INdmPrimitive> primitives = currentStageResult.Primitives;
            foreach (var primitive in currentStage.Primitives)
            {
                var primitiveClone = primitive.Clone() as INdmPrimitive;
                primitiveClone.NdmElement.AutoPrestrain.Clear();
                if (primitive is IHasHostPrimitive hasHostPrimitive)
                {
                    ProcessHost(primitives, hasHostPrimitive, primitiveClone);
                }
                AddPrimitiveToDictionary(primitives, primitive, primitiveClone);
            }
        }

        private static void AddPrimitiveToDictionary(Dictionary<INdmPrimitive, INdmPrimitive> primitives, INdmPrimitive primitive, INdmPrimitive primitiveClone)
        {
            if (primitives.ContainsKey(primitive) == false)
            {
                primitives.Add(primitive, primitiveClone);
            }
        }

        private INdmPrimitive ProcessHost(Dictionary<INdmPrimitive, INdmPrimitive> primitives, IHasHostPrimitive primitive, INdmPrimitive primitiveClone, IForceTuple strainTuple = null)
        {
            INdmPrimitive hostPrimitiveClone;
            INdmPrimitive? hostPrimitive = primitive.HostPrimitive;
            if (primitives.TryGetValue(hostPrimitive, out hostPrimitiveClone) == false)
                {
                    hostPrimitiveClone = hostPrimitive.Clone() as INdmPrimitive;
                    hostPrimitiveClone.NdmElement.AutoPrestrain.Clear();
                    primitives.Add(hostPrimitive, hostPrimitiveClone);
                if (strainTuple != null)
                {
                    AddPrestrain(strainTuple, hostPrimitive, hostPrimitiveClone);
                }
                }
                (primitiveClone as IHasHostPrimitive).HostPrimitive = hostPrimitiveClone;
            return hostPrimitiveClone;
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

        private void ProcessPrimitivesByPreviousStage(int stageNumber)
        {
            
            IConstructionStage currentStage = ConstructionStages[stageNumber];
            IStageResult currentStageResult = result.StagesResults[stageNumber];
            Dictionary<INdmPrimitive, INdmPrimitive> currentPrimitives = currentStageResult.Primitives;
            IStageResult previuosStageResult = result.StagesResults[stageNumber - 1];
            ISubStageResult prevStageFinishResult = previuosStageResult.SubStagesResults.Single(x => x.StagePosition == StagePosition.Finish);
            if (prevStageFinishResult.IsValid == false)
            {
                ProcessPrimitivesByCurrentStage(stageNumber);
                return;
            }
            foreach (var primitive in currentStage.Primitives)
            {
                var primitiveClone = primitive.Clone() as INdmPrimitive;
                primitiveClone.NdmElement.AutoPrestrain.Clear();
                INdmPrimitive previousPrimitive;
                if (previuosStageResult.Primitives.TryGetValue(primitive, out previousPrimitive) == true)
                {
                    AddPrestrain(prevStageFinishResult.StrainTuple, previousPrimitive, primitiveClone);
                }
                if (primitive is IHasHostPrimitive hasHostPrimitive)
                {
#error
                    INdmPrimitive prevHost;
                    previuosStageResult.Primitives.TryGetValue(primitive, out prevHost);
                    var hostPrimitiveClone = ProcessHost(currentPrimitives, prevHost as IHasHostPrimitive, primitiveClone, prevStageFinishResult.StrainTuple);
                }
                AddPrimitiveToDictionary(currentPrimitives, primitive, primitiveClone);
            }
        }

        private static void AddPrestrain(IForceTuple strainTuple, INdmPrimitive previousPrimitive, INdmPrimitive primitiveClone)
        {
            IForceTuple previousUsersPrestrain = previousPrimitive.NdmElement.UsersPrestrain;
            primitiveClone.NdmElement.UsersPrestrain.Nz = previousUsersPrestrain.Nz + strainTuple.Nz;
            primitiveClone.NdmElement.UsersPrestrain.Mx = previousUsersPrestrain.Mx + strainTuple.Mx;
            primitiveClone.NdmElement.UsersPrestrain.My = previousUsersPrestrain.My + strainTuple.My;
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
