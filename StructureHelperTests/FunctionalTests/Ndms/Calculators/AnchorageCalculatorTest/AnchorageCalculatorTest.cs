using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StructureHelperLogics.NdmCalculations.Analyses.RC;

namespace StructureHelperTests.FunctionalTests.Ndms.Calculators.AnchorageCalculatorTest
{
    public class AnchorageCalculatorTest
    {
        [TestCase(0.012d, 0d, 0.416d)]
        [TestCase(0.025d, 0d, 0.867d)]
        [TestCase(0.032d, 0d, 1.110d)]
        [TestCase(0.036d, 0d, 1.388d)]
        public void Run_ShouldPass(double diameter, double stress, double expectedBaseDevLength)
        {
            //Arrange
            var inputData = new AnchorageInputData();
            inputData.ConcreteStrength = 1e6; //Pa
            inputData.ReinforcementStrength = 347e6; //Pa
            inputData.FactorEta1 = 2.5d;
            inputData.CrossSectionArea = Math.PI * diameter * diameter / 4d;
            inputData.CrossSectionPerimeter = Math.PI * diameter;
            inputData.ReinforcementStress = stress;
            var calculator = new AnchorageCalculator(inputData);
            //Act
            var baseLength = calculator.GetBaseDevLength();
            //Assert
            Assert.AreEqual(expectedBaseDevLength, baseLength, 0.001d);
        }
    }
}
