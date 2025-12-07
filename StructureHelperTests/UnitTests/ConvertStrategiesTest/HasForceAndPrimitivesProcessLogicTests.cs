using DataAccess.DTOs;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest
{


    [TestFixture]
    public class HasForceAndPrimitivesProcessLogicTests
    {
        private Mock<IProcessLogic<IHasForceActions>> mockForcesLogic;
        private Mock<IProcessLogic<IHasPrimitives>> mockPrimitivesLogic;
        private Mock<IHasForcesAndPrimitives> mockSource;
        private Mock<IHasForcesAndPrimitives> mockTarget;
        private Mock<Dictionary<(Guid id, Type type), ISaveable>> mockDictionary;


        [SetUp]
        public void SetUp()
        {
            mockForcesLogic = new Mock<IProcessLogic<IHasForceActions>>(MockBehavior.Strict);
            mockPrimitivesLogic = new Mock<IProcessLogic<IHasPrimitives>>(MockBehavior.Strict);
            mockDictionary = new Mock<Dictionary<(Guid id, Type type), ISaveable>>();

            mockSource = new Mock<IHasForcesAndPrimitives>();
            mockTarget = new Mock<IHasForcesAndPrimitives>();
        }

        [Test]
        public void Process_WhenUsingDIInjects_ShouldCallBothInternalLogics()
        {
            // Arrange
            var sut = new HasForcesAndPrimitivesProcessLogic(
                ConvertDirection.FromDTO,
                mockForcesLogic.Object,
                mockPrimitivesLogic.Object
            )
            {
                Source = mockSource.Object,
                Target = mockTarget.Object
            };

            // Setup mocked behavior
            mockForcesLogic.SetupSet(x => x.Source = mockSource.Object);
            mockForcesLogic.SetupSet(x => x.Target = mockTarget.Object);
            mockForcesLogic.SetupSet(x => x.ReferenceDictionary = mockDictionary.Object);
            mockForcesLogic.Setup(x => x.Process());

            mockPrimitivesLogic.SetupSet(x => x.Source = mockSource.Object);
            mockPrimitivesLogic.SetupSet(x => x.Target = mockTarget.Object);
            mockPrimitivesLogic.SetupSet(x => x.ReferenceDictionary = mockDictionary.Object);
            mockPrimitivesLogic.Setup(x => x.Process());
            sut.ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>();

            // Act
            sut.Process();

            // Assert
            mockForcesLogic.Verify(x => x.Process(), Times.Once);
            mockPrimitivesLogic.Verify(x => x.Process(), Times.Once);
        }

        [Test]
        public void Process_PassesSourceAndTargetCorrectly_ToBothSubLogics()
        {
            // Arrange
            var sut = new HasForcesAndPrimitivesProcessLogic(
                ConvertDirection.ToDTO,
                mockForcesLogic.Object,
                mockPrimitivesLogic.Object)
            {
                Source = mockSource.Object,
                Target = mockTarget.Object,
            };

            mockForcesLogic.SetupSet(x => x.Source = mockSource.Object);
            mockForcesLogic.SetupSet(x => x.Target = mockTarget.Object);
            mockForcesLogic.Setup(x => x.Process());

            mockPrimitivesLogic.SetupSet(x => x.Source = mockSource.Object);
            mockPrimitivesLogic.SetupSet(x => x.Target = mockTarget.Object);
            mockPrimitivesLogic.Setup(x => x.Process());
            sut.ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>();

            // Act
            sut.Process();

            // Assert
            mockForcesLogic.VerifySet(x => x.Source = mockSource.Object);
            mockForcesLogic.VerifySet(x => x.Target = mockTarget.Object);

            mockPrimitivesLogic.VerifySet(x => x.Source = mockSource.Object);
            mockPrimitivesLogic.VerifySet(x => x.Target = mockTarget.Object);
        }
    }

}
