using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperTests.UnitTests.Ndms.Cracks
{
    public class CrackWidthLogicSP63Test
    {
        [TestCase(1.4d, 0.001d, 0d, 0.3d, 0.00020999999999999998d)]
        [TestCase(1.4d, 0.001d, 0.001d, 0.3d, 0d)]
        public void Run_ShouldPass(double termFactor, double rebarStrain, double concreteStrain, double length, double expectedWidth)
        {
            //Arrange
            var inputData = new CrackWidthLogicInputDataSP63()
            {
                TermFactor = termFactor,
                BondFactor = 0.5d,
                StressStateFactor = 1d,
                PsiSFactor = 1d,
                RebarStrain = rebarStrain,
                ConcreteStrain = concreteStrain,
                LengthBetweenCracks = length
            };
            var logic = new CrackWidthLogicSP63() { InputData = inputData };

            //Act
            var width = logic.GetCrackWidth();
            //Assert
            Assert.AreEqual(expectedWidth, width, 0.000001d);
        }

    }
}
