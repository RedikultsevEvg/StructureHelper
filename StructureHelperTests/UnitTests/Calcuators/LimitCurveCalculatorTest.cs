using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace StructureHelperTests.UnitTests.Calcuators
{
    [TestFixture]
    public class LimitCurveCalculatorTest
    {
        [TestCase(10d, 20d)]
        public void Run_ShouldPass(double xmax, double ymax)
        {
            //Arrange
            var calculator = new LimitCurveCalculator(new StabLimitCurveLogic())
            {
            };
            calculator.SurroundData.XMax = xmax;
            calculator.SurroundData.XMin = -xmax;
            calculator.SurroundData.YMax = ymax;
            calculator.SurroundData.YMin = -ymax;
            //Act
            calculator.Run();
            var result = calculator.Result;
            //Assert
            Assert.IsNotNull(result);
        }
    }
}
