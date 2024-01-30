using NUnit.Framework;
using StructureHelperCommon.Models.Soils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperTests.UnitTests.Calcuators
{
    [TestFixture]
    public class SoilAnchorCalculatorTest
    {
        [TestCase(6d, 0.483d, 7d, 0d, -2.188d, 25d, 30e3d, 2005767.1007324921d, 1671472.5839437435d)]
        [TestCase(6d, 0.483d, 7d, 0d, -2.188d, 30d, 0d, 1437198.1109237692d, 1197665.0924364743d)]
        public void Run_ShouldPass(double rootLength, double rootDiameter, double freeLength, double graundLevel, double headLevel, double fi, double c, double expectedCharBearingCapacity, double expectedDesignBearingCapacity)
        {
            //Arrange
            var anchor = new SoilAnchor()
            {
                RootLength = rootLength,
                RootDiameter = rootDiameter,
                FreeLength = freeLength,
                GroundLevel = graundLevel,
                HeadLevel = headLevel,
            };
            var soil = new AnchorSoilProperties()
            {
                FrictionAngle = fi,
                Coheasion = c
            };
            //Act
            var calculator = new AnchorCalculator(anchor, soil);
            calculator.Run();
            var result = calculator.Result as AnchorResult;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCharBearingCapacity, result.CharBearingCapacity, expectedCharBearingCapacity * 1e-6d);
            Assert.AreEqual(expectedDesignBearingCapacity, result.DesignBearingCapacity, expectedDesignBearingCapacity * 1e-6d);
        }

        [TestCase(6d, 0.2d, 7d, 0.145d, 0.28290937500000002d, 0.13633537500000006d)]
        [TestCase(6d, 0.483, 7d, 0.145d, 1.6716908400000001d, 1.5251168399999999d)]
        public void Run_ShouldPass_Volume(double rootLength, double rootDiameter, double freeLength, double boreHoleDiameter, double expectedVolume1, double expectedVolume2)
        {
            //Arrange
            var anchor = new SoilAnchor()
            {
                RootLength = rootLength,
                RootDiameter = rootDiameter,
                FreeLength = freeLength,
                BoreHoleDiameter = boreHoleDiameter,
            };
            var soil = new AnchorSoilProperties();
            //Act
            var calculator = new AnchorCalculator(anchor, soil);
            calculator.Run();
            var result = calculator.Result as AnchorResult;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedVolume1, result.MortarVolumeFstStady, expectedVolume1 * 1e-6d);
            Assert.AreEqual(expectedVolume2, result.MortarVolumeSndStady, expectedVolume2 * 1e-6d);
        }
    }
}
