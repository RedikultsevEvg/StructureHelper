using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelperTests.UnitTests.BeamShearTests
{


    [TestFixture]
    public class ConcreteShearStrengthLogicTests
    {
        private Mock<ISectionEffectiveness> sectionEffectivenessMock;
        private Mock<IInclinedSection> inclinedSectionMock;

        [SetUp]
        public void SetUp()
        {
            sectionEffectivenessMock = new Mock<ISectionEffectiveness>();
            inclinedSectionMock = new Mock<IInclinedSection>();
        }

        [Test]
        public void CalculateShearStrength_NormalCase_ReturnsExpectedValue()
        {
            // Arrange
            sectionEffectivenessMock.SetupGet(s => s.BaseShapeFactor).Returns(0.9);
            sectionEffectivenessMock.SetupGet(s => s.ShapeFactor).Returns(1.1);
            sectionEffectivenessMock.SetupGet(s => s.MaxCrackLengthRatio).Returns(2.0);
            sectionEffectivenessMock.SetupGet(s => s.MinCrackLengthRatio).Returns(0.1);

            inclinedSectionMock.SetupGet(s => s.ConcreteTensionStrength).Returns(2.5);
            inclinedSectionMock.SetupGet(s => s.WebWidth).Returns(0.3);
            inclinedSectionMock.SetupGet(s => s.EffectiveDepth).Returns(0.5);
            inclinedSectionMock.SetupGet(s => s.StartCoord).Returns(0.0);
            inclinedSectionMock.SetupGet(s => s.EndCoord).Returns(0.8); // crack length = 0.8m

            var logic = new ConcreteShearStrengthLogic(
                sectionEffectivenessMock.Object,
                inclinedSectionMock.Object,
                null
            );

            // Expected shear strength
            double concreteMoment = 0.9 * 1.1 * 2.5 * 0.3 * 0.5 * 0.5;
            double expectedShear = concreteMoment / 0.8;

            // Act
            double actual = logic.CalculateShearStrength();

            // Assert
            Assert.That(actual, Is.EqualTo(expectedShear).Within(1e-9));
        }

        [Test]
        public void CalculateShearStrength_CrackLengthAboveMax_IsCapped()
        {
            // Arrange
            sectionEffectivenessMock.SetupGet(s => s.BaseShapeFactor).Returns(1.0);
            sectionEffectivenessMock.SetupGet(s => s.ShapeFactor).Returns(1.0);
            sectionEffectivenessMock.SetupGet(s => s.MaxCrackLengthRatio).Returns(1.0); // max length = 0.5m
            sectionEffectivenessMock.SetupGet(s => s.MinCrackLengthRatio).Returns(0.1);

            inclinedSectionMock.SetupGet(s => s.ConcreteTensionStrength).Returns(1.0);
            inclinedSectionMock.SetupGet(s => s.WebWidth).Returns(1.0);
            inclinedSectionMock.SetupGet(s => s.EffectiveDepth).Returns(0.5);
            inclinedSectionMock.SetupGet(s => s.StartCoord).Returns(0.0);
            inclinedSectionMock.SetupGet(s => s.EndCoord).Returns(2.0); // crack length = 2.0m > max

            var logic = new ConcreteShearStrengthLogic(
                sectionEffectivenessMock.Object,
                inclinedSectionMock.Object,
                null
            );

            // Expected crack length should be capped to 0.5m
            double expectedShear = (1.0 * 1.0 * 1.0 * 1.0 * 0.5 * 0.5) / 0.5;

            // Act
            double actual = logic.CalculateShearStrength();

            // Assert
            Assert.That(actual, Is.EqualTo(expectedShear).Within(1e-9));
        }

        [Test]
        public void CalculateShearStrength_CrackLengthBelowMin_IsRaised()
        {
            // Arrange
            sectionEffectivenessMock.SetupGet(s => s.BaseShapeFactor).Returns(1.0);
            sectionEffectivenessMock.SetupGet(s => s.ShapeFactor).Returns(1.0);
            sectionEffectivenessMock.SetupGet(s => s.MaxCrackLengthRatio).Returns(2.0);
            sectionEffectivenessMock.SetupGet(s => s.MinCrackLengthRatio).Returns(0.5); // min length = 0.25m

            inclinedSectionMock.SetupGet(s => s.ConcreteTensionStrength).Returns(1.0);
            inclinedSectionMock.SetupGet(s => s.WebWidth).Returns(1.0);
            inclinedSectionMock.SetupGet(s => s.EffectiveDepth).Returns(0.5);
            inclinedSectionMock.SetupGet(s => s.StartCoord).Returns(0.0);
            inclinedSectionMock.SetupGet(s => s.EndCoord).Returns(0.1); // crack length = 0.1m < min

            var logic = new ConcreteShearStrengthLogic(
                sectionEffectivenessMock.Object,
                inclinedSectionMock.Object,
                null
            );

            // Expected crack length should be raised to 0.25m
            double expectedShear = (1.0 * 1.0 * 1.0 * 1.0 * 0.5 * 0.5) / 0.25;

            // Act
            double actual = logic.CalculateShearStrength();

            // Assert
            Assert.That(actual, Is.EqualTo(expectedShear).Within(1e-9));
        }

        [Test]
        public void CalculateShearStrength_InvalidEffectiveDepth_Throws()
        {
            // Arrange
            inclinedSectionMock.SetupGet(s => s.EffectiveDepth).Returns(0.0);

            var logic = new ConcreteShearStrengthLogic(
                sectionEffectivenessMock.Object,
                inclinedSectionMock.Object
            );

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => logic.CalculateShearStrength());
        }
    }

}
