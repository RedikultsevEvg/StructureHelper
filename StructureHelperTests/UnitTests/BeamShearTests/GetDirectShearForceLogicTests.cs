using Moq;
using NUnit.Framework;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears.Logics;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperCommon.Infrastructures.Enums;

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
                var mockAction = new Mock<IBeamShearAction>();
                var mockInclinedSection = new Mock<IInclinedSection>();
                var mockShearLoad = new Mock<IBeamSpanLoad>();

                mockAction.Setup(a => a.SupportAction.SupportForce.ForceTuple.Qx).Returns(100.0);
                mockAction.Setup(a => a.SupportAction.ShearLoads).Returns(new List<IBeamSpanLoad> { mockShearLoad.Object });

                mockInclinedSection.Setup(i => i.StartCoord).Returns(2.0);
                mockInclinedSection.Setup(i => i.EndCoord).Returns(5.0);

                _mockSummaryForceLogic.Setup(s => s.GetSumShearForce(mockShearLoad.Object, 2.0, 5.0)).Returns(new ForceTuple() { Qy = 50.0});
                _logic = new GetDirectShearForceLogic(mockAction.Object, mockInclinedSection.Object, LimitStates.ULS, CalcTerms.ShortTerm, _mockLogger.Object, _mockSummaryForceLogic.Object);
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
