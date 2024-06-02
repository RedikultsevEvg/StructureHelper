using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoaderCalculator.Data.Ndms;
using NUnit.Framework;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperTests.UnitTests.Ndms.Cracks
{
    public class EquivalentDiameterLogicTest
    {
        [TestCase(0.025d, 2, 0.025d, 2, 0.025d)]
        [TestCase(0.032d, 2, 0.025d, 1, 0.029842055910607741d)]
        [TestCase(0.032d, 1, 0.025d, 2, 0.027524820186583671d)]
        public void Run_ShouldPass(double fstDiameter, int fstCount, double sndDiameter, int sndCount, double expectedDiameter)
        {
            //Arrange
            List<RebarNdm> rebar = new();
            for (int i = 0; i < fstCount; i++)
            {
                rebar.Add(new RebarNdm() { Area = 0.785d * fstDiameter * fstDiameter });
            }
            for (int i = 0; i < sndCount; i++)
            {
                rebar.Add(new RebarNdm() { Area = 0.785d * sndDiameter * sndDiameter });
            }
            var logic = new EquivalentDiameterLogic() { Rebars = rebar };
            //Act
            var eqDiametr = logic.GetAverageDiameter();
            //Assert
            Assert.AreEqual(expectedDiameter, eqDiametr, 0.0001d);
        }
    }
}
