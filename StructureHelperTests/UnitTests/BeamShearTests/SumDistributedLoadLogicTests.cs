using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelperTests.UnitTests.BeamShearTests
{
        [TestFixture]
        public class SumDistributedLoadLogicTests
        {
            private Mock<IShiftTraceLogger> _mockLogger;
            private Mock<ICoordinateByLevelLogic> _mockCoordinateByLevelLogic;
            private SumDistributedLoadLogic _logic;

            [SetUp]
            public void Setup()
            {
                _mockLogger = new Mock<IShiftTraceLogger>();
                _mockCoordinateByLevelLogic = new Mock<ICoordinateByLevelLogic>();
                _logic = new SumDistributedLoadLogic(_mockCoordinateByLevelLogic.Object, _mockLogger.Object);
            }

            [Test]
            public void GetSumShearForce_ShouldReturnCorrectForce_ForValidDistributedLoad()
            {
                // Arrange
                var mockDistributedLoad = new Mock<IDistributedLoad>();
                mockDistributedLoad.Setup(d => d.StartCoordinate).Returns(1.0);
                mockDistributedLoad.Setup(d => d.EndCoordinate).Returns(4.0);
                mockDistributedLoad.Setup(d => d.LoadValue).Returns(50.0);
                mockDistributedLoad.Setup(d => d.LoadRatio).Returns(0.9);
                mockDistributedLoad.Setup(d => d.RelativeLoadLevel).Returns(0.5);

                _mockCoordinateByLevelLogic.Setup(c => c.GetCoordinate(2.0, 5.0, 0.5)).Returns(4.5);

                // Act
                double result = _logic.GetSumShearForce(mockDistributedLoad.Object, 2.0, 5.0);

                // Assert
                Assert.That(result, Is.EqualTo(135.0));
                _mockLogger.Verify(l => l.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.AtLeastOnce);
            }

            [Test]
            public void GetSumShearForce_ShouldReturnZero_WhenStartCoordExceedsEndCoord()
            {
                // Arrange
                var mockDistributedLoad = new Mock<IDistributedLoad>();
                mockDistributedLoad.Setup(d => d.StartCoordinate).Returns(6.0);

                // Act
                double result = _logic.GetSumShearForce(mockDistributedLoad.Object, 2.0, 5.0);

                // Assert
                Assert.That(result, Is.EqualTo(0.0));
            }

            [Test]
            public void GetSumShearForce_ShouldThrowException_ForInvalidShearLoad()
            {
                // Arrange
                var mockInvalidShearLoad = new Mock<IBeamShearLoad>();

                // Act & Assert
                Assert.Throws<StructureHelperException>(() => _logic.GetSumShearForce(mockInvalidShearLoad.Object, 2.0, 5.0));
            }
        }

}
