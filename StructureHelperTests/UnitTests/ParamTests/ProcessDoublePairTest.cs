using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StructureHelperCommon.Models.Parameters;

namespace StructureHelperTests.UnitTests.ParamTests
{
    public class ProcessDoublePairTest
    {
        [TestCase("100mm", DigitPlace.Start, "mm", 100d)] //Without backspace
        [TestCase("100 mm", DigitPlace.Start, "mm", 100d)] //With backspace
        [TestCase("Fixed3", DigitPlace.Any, "fixed", 3d)]
        public void Run_ShouldPass(string inputString, DigitPlace digitPlace, string expectedText, double expectedValue)
        {
            //Arrange
            var logic = new ProcessDoublePairLogic() { DigitPlace = digitPlace};
            //Act
            var result = logic.GetValuePairByString(inputString);
            //Assert
            Assert.AreEqual(expectedText, result.Text);
            Assert.AreEqual(expectedValue, result.Value, 0.001d);
        }
    }
}
