using NUnit.Framework;
using StructureHelperLogics.Models.LoadHistory.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperTests.Infrastructures.LoadHistory;

namespace StructureHelperTests.UnitTests.LoadHistory
{
    [TestFixture]
    public class FirstStagePrimitivesLogicTests
    {
        [Test]
        public void Process_ShouldClonePrimitivesAndClearAutoPrestrain()
        {
            // Arrange
            var firstPrimitive = LoadHistoryTestData.CreatePointPrimitive(usersPrestrainNz: 0.001d, autoPrestrainNz: 0.005d);
            var secondPrimitive = LoadHistoryTestData.CreatePointPrimitive(usersPrestrainNz: 0.002d, autoPrestrainNz: 0.006d);
            var (stage, stageResult) = LoadHistoryTestData.CreateStageWith(firstPrimitive, secondPrimitive);
            var logic = new FirstStagePrimitivesLogic();

            // Act
            logic.Process(stage, stageResult);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(stageResult.Primitives, Has.Count.EqualTo(2));
                Assert.That(stageResult.Primitives[firstPrimitive], Is.Not.SameAs(firstPrimitive));
                Assert.That(stageResult.Primitives[secondPrimitive], Is.Not.SameAs(secondPrimitive));
                Assert.That(stageResult.Primitives[firstPrimitive].NdmElement.UsersPrestrain.Nz, Is.EqualTo(0.001d));
                Assert.That(stageResult.Primitives[firstPrimitive].NdmElement.AutoPrestrain.Nz, Is.EqualTo(0d));
                Assert.That(stageResult.Primitives[secondPrimitive].NdmElement.AutoPrestrain.Nz, Is.EqualTo(0d));
                Assert.That(firstPrimitive.NdmElement.AutoPrestrain.Nz, Is.EqualTo(0.005d),
                    "source primitive must not be changed");
                Assert.That(secondPrimitive.NdmElement.AutoPrestrain.Nz, Is.EqualTo(0.006d),
                    "source primitive must not be changed");
            });
        }

        [Test]
        public void Process_WhenHostIsNotListedInStage_ShouldAddHostCloneAndLinkHostedClone()
        {
            // Arrange
            var hostPrimitive = LoadHistoryTestData.CreatePointPrimitive(usersPrestrainNz: 0.003d, autoPrestrainNz: 0.004d);
            var hostedPrimitive = LoadHistoryTestData.CreateRebarPrimitive(hostPrimitive);
            var (stage, stageResult) = LoadHistoryTestData.CreateStageWith(hostedPrimitive);
            var logic = new FirstStagePrimitivesLogic();

            // Act
            logic.Process(stage, stageResult);

            // Assert
            var hostedClone = (IHasHostPrimitive)stageResult.Primitives[hostedPrimitive];
            INdmPrimitive hostClone = hostedClone.HostPrimitive!;
            Assert.Multiple(() =>
            {
                Assert.That(stageResult.Primitives, Has.Count.EqualTo(2));
                Assert.That(hostClone, Is.Not.SameAs(hostPrimitive));
                Assert.That(hostClone, Is.SameAs(stageResult.Primitives[hostPrimitive]));
                Assert.That(hostClone.NdmElement.UsersPrestrain.Nz, Is.EqualTo(0.003d));
                Assert.That(hostClone.NdmElement.AutoPrestrain.Nz, Is.EqualTo(0d));
                Assert.That(hostPrimitive.NdmElement.AutoPrestrain.Nz, Is.EqualTo(0.004d),
                    "source host primitive must not be changed");
            });
        }

        [Test]
        public void Process_WhenHostIsListedBeforeHostedPrimitive_ShouldLinkToPreparedHostClone()
        {
            // Arrange
            var hostPrimitive = LoadHistoryTestData.CreatePointPrimitive(usersPrestrainNz: 0.003d);
            var hostedPrimitive = LoadHistoryTestData.CreateRebarPrimitive(hostPrimitive);
            var (stage, stageResult) = LoadHistoryTestData.CreateStageWith(hostPrimitive, hostedPrimitive);
            var logic = new FirstStagePrimitivesLogic();

            // Act
            logic.Process(stage, stageResult);

            // Assert
            var hostedClone = (IHasHostPrimitive)stageResult.Primitives[hostedPrimitive];
            Assert.Multiple(() =>
            {
                Assert.That(stageResult.Primitives, Has.Count.EqualTo(2));
                Assert.That(hostedClone.HostPrimitive, Is.SameAs(stageResult.Primitives[hostPrimitive]));
            });
        }

        [Test]
        public void Process_WhenHostIsListedAfterHostedPrimitive_ShouldReuseSingleHostClone()
        {
            // Arrange
            var hostPrimitive = LoadHistoryTestData.CreatePointPrimitive(usersPrestrainNz: 0.003d);
            var hostedPrimitive = LoadHistoryTestData.CreateRebarPrimitive(hostPrimitive);
            var (stage, stageResult) = LoadHistoryTestData.CreateStageWith(hostedPrimitive, hostPrimitive);
            var logic = new FirstStagePrimitivesLogic();

            // Act
            logic.Process(stage, stageResult);

            // Assert
            var hostedClone = (IHasHostPrimitive)stageResult.Primitives[hostedPrimitive];
            Assert.Multiple(() =>
            {
                Assert.That(stageResult.Primitives, Has.Count.EqualTo(2));
                Assert.That(hostedClone.HostPrimitive, Is.SameAs(stageResult.Primitives[hostPrimitive]));
                Assert.That(stageResult.Primitives[hostPrimitive].NdmElement.UsersPrestrain.Nz, Is.EqualTo(0.003d));
            });
        }
    }
}
