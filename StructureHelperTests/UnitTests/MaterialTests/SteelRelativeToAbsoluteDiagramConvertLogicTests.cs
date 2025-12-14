using NUnit.Framework;
using NUnit.Framework.Legacy;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials.Libraries;

namespace StructureHelperTests.UnitTests.MaterialTests
{


    [TestFixture]
    public class SteelRelativeToAbsoluteDiagramConvertLogicTests
    {
        private const double YoungsModulus = 200e9;   // 200 GPa
        private const double BaseStrength = 400e6;    // 400 MPa

        private SteelRelativeToAbsoluteDiagramConvertLogic logic;

        [SetUp]
        public void SetUp()
        {
            logic = new SteelRelativeToAbsoluteDiagramConvertLogic(
                initialYoungsModulus: YoungsModulus,
                baseStrength: BaseStrength);
        }

        [Test]
        public void Convert_ValidRelativeDiagram_ReturnsCorrectAbsoluteDiagram()
        {
            // Arrange
            var source = new SteelDiagramRelativeProperty
            {
                StrainOfProportionality = 0.5,
                StrainOfStartOfYielding = 1.0,
                StrainOfEndOfYielding = 1.2,
                StrainOfUltimateStrength = 2.0,
                StrainOfFracture = 3.0,
                StressOfUltimateStrength = 1.1,
                StressOfFracture = 0.8
            };

            double expectedYieldingStrain = BaseStrength / YoungsModulus;

            // Act
            var result = logic.Convert(source);

            // Assert
            Assert.That(result.InitialYoungsModulus, Is.EqualTo(YoungsModulus));
            Assert.That(result.BaseStrength, Is.EqualTo(BaseStrength));

            Assert.That(result.StressOfProportionality,
                Is.EqualTo(BaseStrength * source.StrainOfProportionality));

            Assert.That(result.StrainOfProportionality,
                Is.EqualTo(expectedYieldingStrain * source.StrainOfProportionality));

            Assert.That(result.StrainOfStartOfYielding,
                Is.EqualTo(expectedYieldingStrain * source.StrainOfStartOfYielding));

            Assert.That(result.StrainOfEndOfYielding,
                Is.EqualTo(expectedYieldingStrain * source.StrainOfEndOfYielding));

            Assert.That(result.StrainOfUltimateStrength,
                Is.EqualTo(expectedYieldingStrain * source.StrainOfUltimateStrength));

            Assert.That(result.StrainOfFracture,
                Is.EqualTo(expectedYieldingStrain * source.StrainOfFracture));

            Assert.That(result.StressOfUltimateStrength,
                Is.EqualTo(BaseStrength * source.StressOfUltimateStrength));

            Assert.That(result.StressOfFracture,
                Is.EqualTo(BaseStrength * source.StressOfFracture));
        }

        [Test]
        public void Convert_InitialYoungsModulusIsZero_ThrowsException()
        {
            // Arrange
            var invalidLogic = new SteelRelativeToAbsoluteDiagramConvertLogic(
                initialYoungsModulus: 0.0,
                baseStrength: BaseStrength);

            var source = CreateValidSource();

            // Act + Assert
            var ex = Assert.Throws<StructureHelperException>(
                () => invalidLogic.Convert(source));

            StringAssert.Contains("Initial modulus", ex.Message);
        }

        [Test]
        public void Convert_BaseStrengthIsNegative_ThrowsException()
        {
            // Arrange
            var invalidLogic = new SteelRelativeToAbsoluteDiagramConvertLogic(
                initialYoungsModulus: YoungsModulus,
                baseStrength: -1.0);

            var source = CreateValidSource();

            // Act + Assert
            var ex = Assert.Throws<StructureHelperException>(
                () => invalidLogic.Convert(source));

            StringAssert.Contains("Stress of yelding", ex.Message);
        }

        private static SteelDiagramRelativeProperty CreateValidSource()
        {
            return new SteelDiagramRelativeProperty
            {
                StrainOfProportionality = 0.5,
                StrainOfStartOfYielding = 1.0,
                StrainOfEndOfYielding = 1.2,
                StrainOfUltimateStrength = 2.0,
                StrainOfFracture = 3.0,
                StressOfUltimateStrength = 1.1,
                StressOfFracture = 0.8
            };
        }
    }

}
