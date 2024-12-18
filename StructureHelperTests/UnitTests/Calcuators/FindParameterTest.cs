using NUnit.Framework;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperTests.UnitTests.Calcuators
{
    internal class FindParameterTest
    {
        [TestCase(0d, 1d, 0.5d, 0.5d)]
        [TestCase(0d, 10d, 5d, 5d)]
        public void Run_ShouldPass_Valid(double startValue, double endValue, double predicateValue, double expectedValue)
        {
            //Arrange
            var calculator = new FindParameterCalculator();
            calculator.InputData.StartValue = startValue;
            calculator.InputData.EndValue = endValue; ;
            calculator.InputData.Predicate = x => x > predicateValue;

            //Act
            calculator.Run();
            var result = calculator.Result as FindParameterResult;
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(expectedValue, Is.EqualTo(predicateValue).Within(0.001d));
        }
        [TestCase(0d, 1d, 5d, 5d, false)]
        [TestCase(0d, 10d, 15d, 15d, false)]
        public void Run_ShouldPass_NotValid(double startValue, double endValue, double predicateValue, double expectedValue, bool isValid)
        {
            //Arrange
            var calculator = new FindParameterCalculator();
            calculator.InputData.StartValue = startValue;
            calculator.InputData.EndValue = endValue; ;
            calculator.InputData.Predicate = x => x > predicateValue;
            //Act
            calculator.Run();
            var result = calculator.Result as FindParameterResult;
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid == isValid, Is.True);
        }
    }
}
