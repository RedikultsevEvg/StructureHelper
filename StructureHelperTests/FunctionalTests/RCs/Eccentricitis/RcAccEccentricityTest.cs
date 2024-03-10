using Moq;
using NUnit.Framework;
using StructureHelperCommon.Models.Sections.Logics;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperTests.FunctionalTests.RCs.Eccentricitis
{
    public class RcAccEccentricityLogicTests
    {
        [Test]
        public void GetValue_ShouldCalculateCorrectly()
        {
            // Arrange
            var eccentricityAxisLogicMock = new Mock<IRcEccentricityAxisLogic>();
            eccentricityAxisLogicMock.Setup(el => el.GetValue()).Returns(3.0); // Adjust based on your expected result

            var loggerMock = new Mock<IShiftTraceLogger>();

            var rcAccEccentricityLogic = new RcAccEccentricityLogic(eccentricityAxisLogicMock.Object)
            {
                Length = 100.0,
                SizeX = 5.0,
                SizeY = 10.0,
                TraceLogger = loggerMock.Object,
            };

            // Act
            var result = rcAccEccentricityLogic.GetValue();

            // Assert
            eccentricityAxisLogicMock.Verify(el => el.GetValue(), Times.Exactly(2));
            //loggerMock.Verify(logger => logger.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.Exactly(3)); // Adjust based on your actual calls
            Assert.AreEqual(3.0, result.ex, 0.001); // Adjust based on your expected result
            Assert.AreEqual(3.0, result.ey, 0.001); // Adjust based on your expected result
                                                    // Add more assertions based on your expected behavior
        }
    }
}
