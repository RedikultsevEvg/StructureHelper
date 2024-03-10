using Moq;
using NUnit.Framework;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections.Logics;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperTests.FunctionalTests.RCs.Eccentricitis
{
    public class RcEccentricityLogicTests
    {
        [TestCase(30, 1.0, 2.0, -60, -30)]
        [TestCase(30, 2.0, 1.0, -30, -60)]
        public void GetValue_ShouldCalculateCorrectly(double nz, double ex, double ey, double expectedMx, double expectedMy)
        {
            // Arrange
            var inputForceTuple = new ForceTuple
            {
                Mx = 10,
                My = 20,
                Nz = nz,
                Qx = 40.0,
                Qy = 50.0,
                Mz = 60.0,
            };

            var eccentricityLogicMock = new Mock<IRcAccEccentricityLogic>();
            eccentricityLogicMock.Setup(el => el.GetValue()).Returns((ex, ey));

            var loggerMock = new Mock<IShiftTraceLogger>();

            var rcEccentricityLogic = new RcEccentricityLogic(eccentricityLogicMock.Object)
            {
                Length = 100.0,
                SizeX = 5.0,
                SizeY = 10.0,
                InputForceTuple = inputForceTuple,
                TraceLogger = loggerMock.Object,
            };

            // Act
            var result = rcEccentricityLogic.GetValue();

            // Assert
            eccentricityLogicMock.Verify(el => el.GetValue(), Times.Once);
            //loggerMock.Verify(logger => logger.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.Exactly(6)); // Adjust based on your actual calls
            //loggerMock.Verify(logger => logger.AddEntry(It.IsAny<string>()), Times.Once); // Adjust based on your actual calls
            Assert.AreEqual(expectedMx, result.Mx, 0.001); // Adjust based on your expected result
            Assert.AreEqual(expectedMy, result.My, 0.001); // Adjust based on your expected result
                                                        // Add more assertions based on your expected behavior
        }
    }
}
