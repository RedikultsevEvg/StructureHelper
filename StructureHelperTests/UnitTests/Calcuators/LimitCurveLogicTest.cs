using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace StructureHelperTests.UnitTests.Calcuators
{
    [TestFixture]
    public class LimitCurveLogicTests
    {
        [Test]
        public void GetPoints_ValidPoints_ReturnsTransformedPoints()
        {
            // Arrange
            var parameterLogicMock = new Mock<ILimitCurveParameterLogic>();
            parameterLogicMock.Setup(p => p.GetParameter()).Returns(2.0); // Mocking the GetParameter method

            var limitCurveLogic = new LimitCurveLogic(parameterLogicMock.Object);
            limitCurveLogic.LimitPredicate = point => point.X >= 0.5d; // Example predicate

            var inputPoints = new List<IPoint2D>
        {
            new Point2D { X = 1, Y = 2 },
            new Point2D { X = 3, Y = 4 }
            // Add more points as needed
        };

            // Act
            var result = limitCurveLogic.GetPoints(inputPoints);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(inputPoints.Count, result.Count);

            for (int i = 0; i < inputPoints.Count; i++)
            {
                Assert.AreEqual(inputPoints[i].X * 2.0, result[i].X);
                Assert.AreEqual(inputPoints[i].Y * 2.0, result[i].Y);
            }

            // Verify that GetParameter was called
            parameterLogicMock.Verify(p => p.GetParameter(), Times.Exactly(inputPoints.Count));
        }

        [Test]
        public void GetPoints_InvalidPredicate_ThrowsException()
        {
            // Arrange
            var parameterLogicMock = new Mock<ILimitCurveParameterLogic>();
            parameterLogicMock.Setup(p => p.GetParameter()).Returns(2.0);

            var limitCurveLogic = new LimitCurveLogic(parameterLogicMock.Object);
            limitCurveLogic.LimitPredicate = point => true; // Invalid predicate

            var inputPoints = new List<IPoint2D>
        {
            new Point2D { X = 1, Y = 2 },
            new Point2D { X = 3, Y = 4 }
        };

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => limitCurveLogic.GetPoints(inputPoints));
        }
    }
}
