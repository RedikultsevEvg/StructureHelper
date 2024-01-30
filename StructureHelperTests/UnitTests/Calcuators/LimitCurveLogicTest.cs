using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve.Factories;

namespace StructureHelperTests.UnitTests.Calcuators
{
    [TestFixture]
    public class LimitCurveLogicTests
    {
        [Test]
        public void GetPoints_ValidPoints_ReturnsTransformedPoints()
        {
            // Arrange
            var getPredicateLogic = new Mock<IGetPredicateLogic>();
            getPredicateLogic.Setup(p => p.GetPredicate()).Returns(point => point.X >= 0.5d);//

            var limitCurveLogic = new LimitCurveLogic(getPredicateLogic.Object);
            

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
                Assert.AreEqual(0.5d, result[i].X, 0.01d);
                Assert.AreEqual(inputPoints[i].Y / inputPoints[i].X * 0.5d, result[i].Y, 0.01d);
            }
        }
    }
}
