using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.Models.LoadHistory.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperTests.Infrastructures.LoadHistory;

namespace StructureHelperTests.UnitTests.LoadHistory
{
    [TestFixture]
    public class NextStagePrimitivesLogicTests
    {
        [Test]
        public void Process_WhenHostIsListedBeforeHostedPrimitive_ShouldCarryOverPrestrainAndLinkHostClone()
        {
            // Arrange
            var hostPrimitive = LoadHistoryTestData.CreatePointPrimitive();
            var hostedPrimitive = LoadHistoryTestData.CreateRebarPrimitive(hostPrimitive);
            var previousStageResult = CreatePreviousStageResult(hostPrimitive, hostedPrimitive);
            var (stage, stageResult) = LoadHistoryTestData.CreateStageWith(hostPrimitive, hostedPrimitive);
            var logic = new NextStagePrimitivesLogic();

            // Act
            logic.Process(stage, stageResult, previousStageResult);

            // Assert
            var hostedClone = (IHasHostPrimitive)stageResult.Primitives[hostedPrimitive];
            Assert.Multiple(() =>
            {
                Assert.That(stageResult.Primitives, Has.Count.EqualTo(2));
                Assert.That(stageResult.Primitives[hostPrimitive].NdmElement.UsersPrestrain.Nz, Is.EqualTo(0.10d).Within(1e-12));
                Assert.That(stageResult.Primitives[hostPrimitive].NdmElement.UsersPrestrain.Mx, Is.EqualTo(0.01d).Within(1e-12));
                Assert.That(stageResult.Primitives[hostedPrimitive].NdmElement.UsersPrestrain.Nz, Is.EqualTo(0.20d).Within(1e-12));
                Assert.That(stageResult.Primitives[hostedPrimitive].NdmElement.UsersPrestrain.Mx, Is.EqualTo(0.02d).Within(1e-12));
                Assert.That(stageResult.Primitives[hostPrimitive].NdmElement.AutoPrestrain.Nz, Is.EqualTo(0d));
                Assert.That(stageResult.Primitives[hostedPrimitive].NdmElement.AutoPrestrain.Nz, Is.EqualTo(0d));
                Assert.That(hostedClone.HostPrimitive, Is.SameAs(stageResult.Primitives[hostPrimitive]));
                Assert.That(hostedClone.HostPrimitive, Is.Not.SameAs(previousStageResult.Primitives[hostPrimitive]));
            });
        }

        [Test]
        public void Process_WhenHostedPrimitiveIsListedFirst_ShouldCreateHostCloneAndCarryOverPrestrain()
        {
            // Arrange
            var hostPrimitive = LoadHistoryTestData.CreatePointPrimitive();
            var hostedPrimitive = LoadHistoryTestData.CreateRebarPrimitive(hostPrimitive);
            var previousStageResult = CreatePreviousStageResult(hostPrimitive, hostedPrimitive);
            var (stage, stageResult) = LoadHistoryTestData.CreateStageWith(hostedPrimitive, hostPrimitive);
            var logic = new NextStagePrimitivesLogic();

            // Act
            logic.Process(stage, stageResult, previousStageResult);

            // Assert
            var hostedClone = (IHasHostPrimitive)stageResult.Primitives[hostedPrimitive];
            Assert.Multiple(() =>
            {
                Assert.That(stageResult.Primitives, Has.Count.EqualTo(2));
                Assert.That(stageResult.Primitives[hostPrimitive].NdmElement.HistoryPrestrain.Nz, Is.EqualTo(0.05d).Within(1e-12));
                Assert.That(stageResult.Primitives[hostPrimitive].NdmElement.HistoryPrestrain.Mx, Is.EqualTo(0.005d).Within(1e-12));
                Assert.That(stageResult.Primitives[hostedPrimitive].NdmElement.UsersPrestrain.Nz, Is.EqualTo(0.2d).Within(1e-12));
                Assert.That(hostedClone.HostPrimitive, Is.SameAs(stageResult.Primitives[hostPrimitive]));
                Assert.That(stageResult.Primitives[hostPrimitive].NdmElement.AutoPrestrain.Nz, Is.EqualTo(0d));
            });
        }

        [Test]
        public void Process_WhenPrimitiveIsAbsentInPreviousStage_ShouldApplyNegativeFinishStrain()
        {
            // Arrange
            var newPrimitive = LoadHistoryTestData.CreatePointPrimitive(usersPrestrainNz: 0.30d);
            var previousStageResult = LoadHistoryTestData.CreatePreviousStageResult(new ForceTuple { Nz = 0.05d });
            var (stage, stageResult) = LoadHistoryTestData.CreateStageWith(newPrimitive);
            var logic = new NextStagePrimitivesLogic();

            // Act
            logic.Process(stage, stageResult, previousStageResult);

            // Assert
            Assert.That(stageResult.Primitives[newPrimitive].NdmElement.HistoryPrestrain.Nz,
                Is.EqualTo(-0.05d).Within(1e-12));
        }

        [Test]
        public void Process_WhenPreviousStageIsInvalid_ShouldDelegateToFirstStageLogic()
        {
            // Arrange
            var firstStagePrimitivesLogic = new FirstStagePrimitivesLogicSpy();
            var logic = new NextStagePrimitivesLogic(firstStagePrimitivesLogic);
            var primitive = LoadHistoryTestData.CreatePointPrimitive(usersPrestrainNz: 0.30d, autoPrestrainNz: 0.40d);
            var previousStageResult = LoadHistoryTestData.CreatePreviousStageResult(
                new ForceTuple { Nz = 0.05d },
                isFinishValid: false);
            var (stage, stageResult) = LoadHistoryTestData.CreateStageWith(primitive);

            // Act
            logic.Process(stage, stageResult, previousStageResult);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstStagePrimitivesLogic.ProcessCallCount, Is.EqualTo(1));
                Assert.That(firstStagePrimitivesLogic.LastStage, Is.SameAs(stage));
                Assert.That(firstStagePrimitivesLogic.LastStageResult, Is.SameAs(stageResult));
                Assert.That(stageResult.Primitives[primitive].NdmElement.UsersPrestrain.Nz, Is.EqualTo(0.30d));
                Assert.That(stageResult.Primitives[primitive].NdmElement.AutoPrestrain.Nz, Is.EqualTo(0d));
            });
        }

        [Test]
        public void Process_WhenHostIsAbsentInPreviousStage_ShouldThrowStructureHelperException()
        {
            // Arrange
            var hostPrimitive = LoadHistoryTestData.CreatePointPrimitive();
            var hostedPrimitive = LoadHistoryTestData.CreateRebarPrimitive(hostPrimitive);
            var previousStageResult = LoadHistoryTestData.CreatePreviousStageResult(new ForceTuple());
            previousStageResult.Primitives.Add(hostedPrimitive, (INdmPrimitive)hostedPrimitive.Clone());
            var (stage, stageResult) = LoadHistoryTestData.CreateStageWith(hostedPrimitive);
            var logic = new NextStagePrimitivesLogic();

            // Act
            TestDelegate act = () => logic.Process(stage, stageResult, previousStageResult);

            // Assert
            Assert.That(act, Throws.TypeOf<StructureHelperException>());
        }

        private static StageResultStub CreatePreviousStageResult(
            INdmPrimitive hostPrimitive,
            INdmPrimitive hostedPrimitive)
        {
            var finishStrain = new ForceTuple { Nz = 0.05d, Mx = 0.005d, My = 0.006d };
            var previousStageResult = LoadHistoryTestData.CreatePreviousStageResult(finishStrain);

            var previousHost = (INdmPrimitive)hostPrimitive.Clone();
            previousHost.NdmElement.UsersPrestrain.Nz = 0.10d;
            previousHost.NdmElement.UsersPrestrain.Mx = 0.01d;
            previousHost.NdmElement.AutoPrestrain.Nz = 0.70d;

            var previousHosted = (INdmPrimitive)hostedPrimitive.Clone();
            previousHosted.NdmElement.UsersPrestrain.Nz = 0.20d;
            previousHosted.NdmElement.UsersPrestrain.Mx = 0.02d;
            previousHosted.NdmElement.AutoPrestrain.Nz = 0.80d;

            previousStageResult.Primitives.Add(hostPrimitive, previousHost);
            previousStageResult.Primitives.Add(hostedPrimitive, previousHosted);
            return previousStageResult;
        }

        /// <summary>
        /// Records the delegation of the next stage logic to the first stage logic.
        /// </summary>
        private sealed class FirstStagePrimitivesLogicSpy : IFirstStagePrimitivesLogic
        {
            private readonly FirstStagePrimitivesLogic innerLogic = new();

            public int ProcessCallCount { get; private set; }
            public IConstructionStage? LastStage { get; private set; }
            public IStageResult? LastStageResult { get; private set; }

            public void Process(IConstructionStage stage, IStageResult stageResult)
            {
                ProcessCallCount++;
                LastStage = stage;
                LastStageResult = stageResult;
                innerLogic.Process(stage, stageResult);
            }
        }
    }
}
