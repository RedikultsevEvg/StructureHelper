using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelperTests.UnitTests.BeamShearTests
{


    [TestFixture]
    public class BeamShearStrengthByStirrupDensityTests
    {
        private Mock<IStirrupEffectiveness> _mockStirrupEffectiveness;
        private Mock<IStirrupByDensity> _mockStirrupByDensity;
        private Mock<IInclinedSection> _mockInclinedSection;
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private StirrupByDensityStrengthLogic _beamShearStrength;

        [SetUp]
        public void Setup()
        {
            _mockStirrupEffectiveness = new Mock<IStirrupEffectiveness>();
            _mockStirrupByDensity = new Mock<IStirrupByDensity>();
            _mockInclinedSection = new Mock<IInclinedSection>();
            _mockTraceLogger = new Mock<IShiftTraceLogger>();

            _beamShearStrength = new StirrupByDensityStrengthLogic(
                _mockStirrupEffectiveness.Object,
                _mockStirrupByDensity.Object,
                _mockInclinedSection.Object,
                _mockTraceLogger.Object
            );
        }

        [TestCase(0.2, 0.48)]
        [TestCase(0.5, 1.2)]
        [TestCase(1.1, 2.64)]
        [TestCase(2.2, 2.64)]
        public void GetShearStrength_CalculatesCorrectly_differentStirrupEnd(double stirrupEnd, double expectedStrength)
        {
            // Arrange
            _mockInclinedSection.Setup(s => s.StartCoord).Returns(0);
            _mockInclinedSection.Setup(s => s.EndCoord).Returns(100);
            _mockInclinedSection.Setup(s => s.EffectiveDepth).Returns(0.55);

            _mockStirrupEffectiveness.Setup(s => s.MaxCrackLengthRatio).Returns(2.0);
            _mockStirrupEffectiveness.Setup(s => s.StirrupShapeFactor).Returns(0.8);
            _mockStirrupEffectiveness.Setup(s => s.StirrupPlacementFactor).Returns(0.75);
            _mockStirrupEffectiveness.Setup(s => s.MinimumStirrupRatio).Returns(0.25);

            _mockStirrupByDensity.Setup(s => s.StirrupDensity).Returns(4.0);
            _mockStirrupByDensity.Setup(s => s.StartCoordinate).Returns(0);
            _mockStirrupByDensity.Setup(s => s.EndCoordinate).Returns(stirrupEnd);

            // Act
            double result = _beamShearStrength.GetShearStrength();

            // Assert
            Assert.That(result, Is.EqualTo(expectedStrength).Within(1e-6));
            _mockTraceLogger.Verify(t => t.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.AtLeastOnce);
        }

        [TestCase(0.20, 0.48)]
        [TestCase(0.55, 1.32)]
        [TestCase(1.1, 2.64)]
        [TestCase(2.2, 2.64)]
        public void GetShearStrength_CalculatesCorrectly_differentInclinedSectionEnd(double inclinedSectionEnd, double expectedStrength)
        {
            // Arrange
            _mockInclinedSection.Setup(s => s.StartCoord).Returns(0);
            _mockInclinedSection.Setup(s => s.EndCoord).Returns(inclinedSectionEnd);
            _mockInclinedSection.Setup(s => s.EffectiveDepth).Returns(0.55);

            _mockStirrupEffectiveness.Setup(s => s.MaxCrackLengthRatio).Returns(2.0);
            _mockStirrupEffectiveness.Setup(s => s.StirrupShapeFactor).Returns(0.8);
            _mockStirrupEffectiveness.Setup(s => s.StirrupPlacementFactor).Returns(0.75);
            _mockStirrupEffectiveness.Setup(s => s.MinimumStirrupRatio).Returns(0.25);

            _mockStirrupByDensity.Setup(s => s.StirrupDensity).Returns(4.0);
            _mockStirrupByDensity.Setup(s => s.StartCoordinate).Returns(0);
            _mockStirrupByDensity.Setup(s => s.EndCoordinate).Returns(100);

            // Act
            double result = _beamShearStrength.GetShearStrength();

            // Assert
            Assert.That(result, Is.EqualTo(expectedStrength).Within(1e-6));
            _mockTraceLogger.Verify(t => t.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.AtLeastOnce);
        }

        [TestCase(0.2, 0.66)]
        [TestCase(0.5, 1.65)]
        [TestCase(0.8, 2.64)]
        public void GetShearStrength_CalculatesCorrectly_differentShapeFactor(double shapeFactor, double expectedStrength)
        {
            // Arrange
            _mockInclinedSection.Setup(s => s.StartCoord).Returns(0);
            _mockInclinedSection.Setup(s => s.EndCoord).Returns(1.1);
            _mockInclinedSection.Setup(s => s.EffectiveDepth).Returns(0.55);

            _mockStirrupEffectiveness.Setup(s => s.MaxCrackLengthRatio).Returns(2.0);
            _mockStirrupEffectiveness.Setup(s => s.StirrupShapeFactor).Returns(shapeFactor);
            _mockStirrupEffectiveness.Setup(s => s.StirrupPlacementFactor).Returns(0.75);
            _mockStirrupEffectiveness.Setup(s => s.MinimumStirrupRatio).Returns(0.25);

            _mockStirrupByDensity.Setup(s => s.StirrupDensity).Returns(4.0);
            _mockStirrupByDensity.Setup(s => s.StartCoordinate).Returns(0);
            _mockStirrupByDensity.Setup(s => s.EndCoordinate).Returns(100);

            // Act
            double result = _beamShearStrength.GetShearStrength();

            // Assert
            Assert.That(result, Is.EqualTo(expectedStrength).Within(1e-6));
            _mockTraceLogger.Verify(t => t.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.AtLeastOnce);
        }

        [TestCase(0.2, 0.704)]
        [TestCase(0.5, 1.76)]
        [TestCase(0.8, 2.816)]
        public void GetShearStrength_CalculatesCorrectly_differentPlacementFactor(double placementFactor, double expectedStrength)
        {
            // Arrange
            _mockInclinedSection.Setup(s => s.StartCoord).Returns(0);
            _mockInclinedSection.Setup(s => s.EndCoord).Returns(1.1);
            _mockInclinedSection.Setup(s => s.EffectiveDepth).Returns(0.55);

            _mockStirrupEffectiveness.Setup(s => s.MaxCrackLengthRatio).Returns(2.0);
            _mockStirrupEffectiveness.Setup(s => s.StirrupShapeFactor).Returns(0.8);
            _mockStirrupEffectiveness.Setup(s => s.StirrupPlacementFactor).Returns(placementFactor);
            _mockStirrupEffectiveness.Setup(s => s.MinimumStirrupRatio).Returns(0.25);

            _mockStirrupByDensity.Setup(s => s.StirrupDensity).Returns(4.0);
            _mockStirrupByDensity.Setup(s => s.StartCoordinate).Returns(0);
            _mockStirrupByDensity.Setup(s => s.EndCoordinate).Returns(100);

            // Act
            double result = _beamShearStrength.GetShearStrength();

            // Assert
            Assert.That(result, Is.EqualTo(expectedStrength).Within(1e-6));
            _mockTraceLogger.Verify(t => t.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.AtLeastOnce);
        }

        [Test]
        public void GetShearStrength_ThrowsException_WhenDependenciesAreNull()
        {
            // Arrange
            var invalidInstance = new StirrupByDensityStrengthLogic(
                _mockStirrupEffectiveness.Object,
                null, // Invalid
                _mockInclinedSection.Object,
                _mockTraceLogger.Object
            );

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => invalidInstance.GetShearStrength());
        }
    }

}
