using Moq;
using NUnit.Framework;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;
namespace StructureHelperTests.UnitTests.BeamShearTests
{
        [TestFixture]
        public class GetShearForceLogicTests
        {
            private Mock<IShiftTraceLogger> _mockLogger;
            private Mock<IGetLoadFactor> _mockGetFactorLogic;
            private Mock<IGetDirectShearForceLogic> _mockGetDirectShearForceLogic;
            private Mock<IShearForceLogicInputData> _mockInputData;
            private ShearForceLogic _logic;

            [SetUp]
            public void Setup()
            {
                _mockLogger = new Mock<IShiftTraceLogger>();
                _mockGetFactorLogic = new Mock<IGetLoadFactor>();
                _mockGetDirectShearForceLogic = new Mock<IGetDirectShearForceLogic>();
                _mockInputData = new Mock<IShearForceLogicInputData>();

                _logic = new ShearForceLogic(
                    _mockInputData.Object,
                    _mockLogger.Object,
                    _mockGetFactorLogic.Object,
                    _mockGetDirectShearForceLogic.Object
                );
            }

            [Test]
            public void GetShearForce_ShouldReturnCorrectShearForce()
            {
                // Arrange
                _mockGetFactorLogic.Setup(f => f.GetFactor()).Returns(1.5);
                _mockGetDirectShearForceLogic.Setup(d => d.CalculateShearForceTuple()).Returns(new ForceTuple() { Qy = 100.0 });

                // Act
                double result = _logic.GetShearForce().Qy;

                // Assert
                Assert.That(result, Is.EqualTo(150.0));
                _mockLogger.Verify(l => l.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.AtLeastOnce);
            }
        }
}
