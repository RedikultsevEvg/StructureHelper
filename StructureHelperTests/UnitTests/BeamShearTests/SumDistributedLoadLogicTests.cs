using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperTests.UnitTests.BeamShearTests
{
        [TestFixture]
        public class SumDistributedLoadLogicTests
        {
            private Mock<IShiftTraceLogger> _mockLogger;
            private Mock<ICoordinateByLevelLogic> _mockCoordinateByLevelLogic;
            private SumDistributedLoadLogic _logic;
        private Mock<IFactoredCombinationProperty> _mockCombination;

        [SetUp]
            public void Setup()
            {
                _mockLogger = new Mock<IShiftTraceLogger>();
                _mockCoordinateByLevelLogic = new Mock<ICoordinateByLevelLogic>();
            _mockCombination = new Mock<IFactoredCombinationProperty>();
            _logic = new SumDistributedLoadLogic(_mockCoordinateByLevelLogic.Object, _mockLogger.Object);
            }

            [Test]
            public void GetSumShearForce_ShouldReturnCorrectForce_ForValidDistributedLoad()
            {
            // Arrange
            _mockCombination.Setup(f => f.LimitState).Returns(LimitStates.ULS);
            _mockCombination.Setup(f => f.CalcTerm).Returns(CalcTerms.ShortTerm);
            _mockCombination.Setup(f => f.LongTermFactor).Returns(0.9);
            _mockCombination.Setup(f => f.ULSFactor).Returns(1.2);
            var mockDistributedLoad = new Mock<IDistributedLoad>();
                mockDistributedLoad.Setup(d => d.StartCoordinate).Returns(1.0);
                mockDistributedLoad.Setup(d => d.EndCoordinate).Returns(4.0);
                mockDistributedLoad.Setup(d => d.LoadValue).Returns(new ForceTuple() { Qy = 50.0 });
                mockDistributedLoad.Setup(d => d.LoadRatio).Returns(0.9);
                mockDistributedLoad.Setup(d => d.RelativeLoadLevel).Returns(0.5);
            mockDistributedLoad.Setup(d => d.CombinationProperty).Returns(_mockCombination.Object);

                _mockCoordinateByLevelLogic.Setup(c => c.GetCoordinate(2.0, 5.0, 0.5)).Returns(4.5);

                // Act
                double result = _logic.GetSumShearForce(mockDistributedLoad.Object, 2.0, 5.0).Qy;

                // Assert
                Assert.That(result, Is.EqualTo(121.50000000000001d));
                _mockLogger.Verify(l => l.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.AtLeastOnce);
            }

            [Test]
            public void GetSumShearForce_ShouldReturnZero_WhenStartCoordExceedsEndCoord()
            {
                // Arrange
                var mockDistributedLoad = new Mock<IDistributedLoad>();
                mockDistributedLoad.Setup(d => d.StartCoordinate).Returns(6.0);
            mockDistributedLoad.Setup(d => d.LoadValue.Qy).Returns(100);

                // Act
                double result = _logic.GetSumShearForce(mockDistributedLoad.Object, 2.0, 5.0).Qy;

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
