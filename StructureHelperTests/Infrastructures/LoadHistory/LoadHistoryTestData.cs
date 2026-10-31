using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.Models.LoadHistory.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;

namespace StructureHelperTests.Infrastructures.LoadHistory
{
    /// <summary>
    /// Builds construction stages, stage results and primitives for the load history unit tests.
    /// </summary>
    internal static class LoadHistoryTestData
    {
        public static PointNdmPrimitive CreatePointPrimitive(
            double usersPrestrainNz = 0d,
            double autoPrestrainNz = 0d)
        {
            var primitive = new PointNdmPrimitive();
            primitive.NdmElement.UsersPrestrain.Nz = usersPrestrainNz;
            primitive.NdmElement.AutoPrestrain.Nz = autoPrestrainNz;
            return primitive;
        }

        public static RebarNdmPrimitive CreateRebarPrimitive(INdmPrimitive hostPrimitive)
        {
            var primitive = new RebarNdmPrimitive
            {
                HostPrimitive = hostPrimitive
            };
            return primitive;
        }

        public static (ConstructionStage Stage, StageResultStub StageResult) CreateStageWith(
            params INdmPrimitive[] primitives)
        {
            var stage = new ConstructionStage(Guid.NewGuid());
            foreach (INdmPrimitive primitive in primitives)
            {
                stage.InputData.Primitives.Add(primitive);
            }
            return (stage, new StageResultStub(stage));
        }

        public static StageResultStub CreatePreviousStageResult(IForceTuple finishStrain, bool isFinishValid = true)
        {
            var previousStage = new ConstructionStage(Guid.NewGuid());
            var previousStageResult = new StageResultStub(previousStage);
            previousStageResult.SubStagesResults.Add(new SubStageResultStub
            {
                ConstructionStage = previousStage,
                StagePosition = StagePosition.Finish,
                IsValid = isFinishValid,
                StrainTuple = finishStrain
            });
            return previousStageResult;
        }
    }
}
