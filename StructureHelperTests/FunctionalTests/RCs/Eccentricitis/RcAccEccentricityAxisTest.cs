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
    public class RcEccentricityAxisLogicTests
    {
        [TestCase(12d, 0.2d, 0.02d)]
        [TestCase(3d, 0.9d, 0.03d)]
        [TestCase(2, 0.2d, 0.01d)]
        public void GetValue_ShouldCalculateCorrectly(double length, double size, double expectedEccentricity)
        {
            // Arrange
            var loggerMock = new Mock<IShiftTraceLogger>();

            var rcEccentricityAxisLogic = new RcEccentricityAxisLogic
            {
                Length = length,
                Size = size,
                TraceLogger = loggerMock.Object,
            };

            // Act
            var result = rcEccentricityAxisLogic.GetValue();

            // Assert
            //loggerMock.Verify(logger => logger.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.Exactly(7)); // Adjust based on your actual calls
            Assert.AreEqual(expectedEccentricity, result, 0.0001); // Adjust based on your expected result
                                                           // Add more assertions based on your expected behavior
        }
    }
}
