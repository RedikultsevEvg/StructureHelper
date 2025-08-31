using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelperTests.UnitTests.BeamShearTests
{
    [TestFixture]
    public class SectionEffectivenessFactorySP2018Tests
    {
        private SectionEffectivenessFactorySP2018 factory;

        [SetUp]
        public void SetUp()
        {
            factory = new SectionEffectivenessFactorySP2018();
        }

        [Test]
        public void GetShearEffectiveness_WhenRectangle_ReturnsExpectedValues()
        {
            // Act
            var result = factory.GetShearEffectiveness(BeamShearSectionType.Rectangle);

            // Assert
            Assert.That(result.BaseShapeFactor, Is.EqualTo(1.5));
            Assert.That(result.ShapeFactor, Is.EqualTo(1.0));
            Assert.That(result.MaxCrackLengthRatio, Is.EqualTo(3.0));
            Assert.That(result.MinCrackLengthRatio, Is.EqualTo(0.6));
        }

        [Test]
        public void GetShearEffectiveness_WhenUnsupportedType_Throws()
        {
            // Act & Assert
            Assert.Throws<StructureHelperException>(() =>
                factory.GetShearEffectiveness((BeamShearSectionType)999));
        }
    }
}

