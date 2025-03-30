using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelperTests.UnitTests.BeamShearTests
{
        [TestFixture]
        public class SumConcentratedForceLogicTests
        {
            private Mock<IShiftTraceLogger> _mockLogger;
            private Mock<ICoordinateByLevelLogic> _mockCoordinateByLevelLogic;
            private SumConcentratedForceLogic _logic;

            [SetUp]
            public void Setup()
            {
                _mockLogger = new Mock<IShiftTraceLogger>();
                _mockCoordinateByLevelLogic = new Mock<ICoordinateByLevelLogic>();
                _logic = new SumConcentratedForceLogic(_mockCoordinateByLevelLogic.Object, _mockLogger.Object);
            }

            [Test]
            public void GetSumShearForce_ShouldReturnCorrectForce_ForValidConcentratedForce()
            {
                // Arrange
                var mockConcentratedForce = new Mock<IConcentratedForce>();
                mockConcentratedForce.Setup(f => f.ForceCoordinate).Returns(3.0);
                mockConcentratedForce.Setup(f => f.ForceValue).Returns(new ForceTuple() { Qy = 100.0 });
                mockConcentratedForce.Setup(f => f.LoadRatio).Returns(0.8);
                mockConcentratedForce.Setup(f => f.RelativeLoadLevel).Returns(0.5);

                _mockCoordinateByLevelLogic.Setup(c => c.GetCoordinate(2.0, 5.0, 0.5)).Returns(3.5);

                // Act
                double result = _logic.GetSumShearForce(mockConcentratedForce.Object, 2.0, 5.0).Qy;

                // Assert
                Assert.That(result, Is.EqualTo(80.0));
                _mockLogger.Verify(l => l.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.AtLeastOnce);
            }

            [Test]
            public void GetSumShearForce_ShouldReturnZero_WhenForceCoordinateExceedsEndCoord()
            {
                // Arrange
                var mockConcentratedForce = new Mock<IConcentratedForce>();
                mockConcentratedForce.Setup(f => f.ForceCoordinate).Returns(6.0);

                // Act
                double result = _logic.GetSumShearForce(mockConcentratedForce.Object, 2.0, 5.0).Qy;

                // Assert
                Assert.That(result, Is.EqualTo(0.0));
            }

            [Test]
            public void GetSumShearForce_ShouldThrowException_ForInvalidShearLoad()
            {
                // Arrange
                var mockInvalidShearLoad = new Mock<IBeamSpanLoad>();

                // Act & Assert
                Assert.Throws<StructureHelperException>(() => _logic.GetSumShearForce(mockInvalidShearLoad.Object, 2.0, 5.0));
            }
        }
    }
