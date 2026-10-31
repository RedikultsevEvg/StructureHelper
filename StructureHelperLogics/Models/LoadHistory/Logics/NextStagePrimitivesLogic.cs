using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperLogics.Models.LoadHistory.Logics
{
    /// <inheritdoc/>
    public class NextStagePrimitivesLogic : INextStagePrimitivesLogic
    {
        private readonly IFirstStagePrimitivesLogic firstStagePrimitivesLogic;

        public NextStagePrimitivesLogic() : this(new FirstStagePrimitivesLogic())
        {
        }

        public NextStagePrimitivesLogic(IFirstStagePrimitivesLogic firstStagePrimitivesLogic)
        {
            this.firstStagePrimitivesLogic = firstStagePrimitivesLogic;
        }

        /// <inheritdoc/>
        public void Process(IConstructionStage stage, IStageResult stageResult, IStageResult previousStageResult)
        {
            ISubStageResult previousStageFinishResult = previousStageResult.SubStagesResults
                .Single(x => x.StagePosition == StagePosition.Finish);
            if (previousStageFinishResult.IsValid == false)
            {
                firstStagePrimitivesLogic.Process(stage, stageResult);
                return;
            }

            Dictionary<INdmPrimitive, INdmPrimitive> currentPrimitives = stageResult.Primitives;
            Dictionary<INdmPrimitive, INdmPrimitive> previousPrimitives = previousStageResult.Primitives;

            foreach (INdmPrimitive primitive in stage.InputData.Primitives)
            {
                ProcessPrimitive(primitive, currentPrimitives, previousPrimitives, previousStageFinishResult);
            }

            foreach (INdmPrimitive primitive in currentPrimitives.Values)
            {
                StagePrimitivesLogicHelper.AddPrestrain(primitive, previousStageFinishResult.StrainTuple, -1d);
            }
        }

        /// <summary>
        /// Creates the clone of the primitive for the current stage and, if the primitive is hosted,
        /// links it to the host clone of the current stage.
        /// </summary>
        private INdmPrimitive ProcessPrimitive(
            INdmPrimitive primitive,
            Dictionary<INdmPrimitive, INdmPrimitive> currentPrimitives,
            Dictionary<INdmPrimitive, INdmPrimitive> previousPrimitives,
            ISubStageResult previousStageFinishResult)
        {
            if (currentPrimitives.TryGetValue(primitive, out INdmPrimitive? existingPrimitiveClone))
            {
                return existingPrimitiveClone!;
            }

            INdmPrimitive primitiveClone;
            if (previousPrimitives.TryGetValue(primitive, out INdmPrimitive? previousPrimitive))
            {
                primitiveClone = (INdmPrimitive)previousPrimitive.Clone();
                if (primitiveClone is IHasHostPrimitive hasHostPrimitive)
                {
                    INdmPrimitive currentHost = ((IHasHostPrimitive)primitive).HostPrimitive!;
                    if (previousPrimitives.TryGetValue(currentHost, out INdmPrimitive? previousHost) == false)
                    {
                        throw new StructureHelperException($"Host of primitive {primitive} has not been found in previousStage");
                    }

                    INdmPrimitive newHost;
                    if (currentPrimitives.TryGetValue(currentHost, out INdmPrimitive? currentHostClone) == false)
                    {
                        newHost = ProcessPrimitive(currentHost, currentPrimitives, previousPrimitives, previousStageFinishResult);
                        newHost.NdmElement.UsersPrestrain.Clear();
                        StagePrimitivesLogicHelper.AddPrestrain(newHost, previousHost.NdmElement.HistoryPrestrain);
                        StagePrimitivesLogicHelper.AddPrestrain(newHost, previousStageFinishResult.StrainTuple);
                    }
                    else
                    {
                        newHost = currentHostClone!;
                    }
                    hasHostPrimitive.HostPrimitive = newHost;
                }
                StagePrimitivesLogicHelper.AddPrestrain(primitiveClone, previousStageFinishResult.StrainTuple);
            }
            else
            {
                primitiveClone = (INdmPrimitive)primitive.Clone();
                if (primitiveClone is IHasHostPrimitive currentStageHost)
                {
                    INdmPrimitive newHost = ProcessPrimitive(
                        currentStageHost.HostPrimitive!,
                        currentPrimitives,
                        previousPrimitives,
                        previousStageFinishResult);
                    currentStageHost.HostPrimitive = newHost;
                }
            }

            primitiveClone.NdmElement.AutoPrestrain.Clear();
            StagePrimitivesLogicHelper.AddPrimitiveToDictionary(currentPrimitives, primitive, primitiveClone);
            return primitiveClone;
        }
    }
}
