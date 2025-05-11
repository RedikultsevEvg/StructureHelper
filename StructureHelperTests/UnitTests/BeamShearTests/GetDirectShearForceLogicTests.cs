using Moq;
using NUnit.Framework;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears.Logics;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelperTests.UnitTests.BeamShearTests
{


    namespace YourNamespace.Tests
    {
        [TestFixture]
        public class GetDirectShearForceLogicTests
        {
            private Mock<IShiftTraceLogger> _mockLogger;
            private Mock<ISumForceByShearLoadLogic> _mockSummaryForceLogic;
            private GetDirectShearForceLogic _logic;

            [SetUp]
            public void Setup()
            {
                _mockLogger = new Mock<IShiftTraceLogger>();
                _mockSummaryForceLogic = new Mock<ISumForceByShearLoadLogic>();
                var mockAxisAction = new Mock<IBeamShearAxisAction>();
                var mockInclinedSection = new Mock<IInclinedSection>();
                var mockShearLoad = new Mock<IBeamSpanLoad>();

                mockAxisAction.Setup(a => a.SupportForce.ForceTuple.Qx).Returns(100.0);
                mockAxisAction.Setup(a => a.ShearLoads).Returns(new List<IBeamSpanLoad> { mockShearLoad.Object });

                mockInclinedSection.Setup(i => i.StartCoord).Returns(2.0);
                mockInclinedSection.Setup(i => i.EndCoord).Returns(5.0);

                _mockSummaryForceLogic.Setup(s => s.GetSumShearForce(mockShearLoad.Object, 2.0, 5.0)).Returns(new ForceTuple() { Qy = 50.0});
                _logic = new GetDirectShearForceLogic(mockAxisAction.Object, mockInclinedSection.Object, _mockLogger.Object, _mockSummaryForceLogic.Object);
            }

            [Test]
            public void GetShearForce_ShouldReturnCorrectShearForce()
            {
                // Arrange

                // Act
                double result = _logic.CalculateShearForceTuple().Qy;

                // Assert
                Assert.That(result, Is.EqualTo(150.0));
                _mockLogger.Verify(l => l.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.AtLeastOnce);
            }
        }
    }

}
