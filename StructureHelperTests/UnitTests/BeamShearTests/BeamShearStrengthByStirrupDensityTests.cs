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

        [Test]
        public void GetShearStrength_CalculatesCorrectly()
        {
            // Arrange
            _mockInclinedSection.Setup(s => s.StartCoord).Returns(1.0);
            _mockInclinedSection.Setup(s => s.EndCoord).Returns(3.0);
            _mockInclinedSection.Setup(s => s.EffectiveDepth).Returns(2.0);

            _mockStirrupEffectiveness.Setup(s => s.MaxCrackLengthRatio).Returns(0.5);
            _mockStirrupEffectiveness.Setup(s => s.StirrupShapeFactor).Returns(1.2);
            _mockStirrupEffectiveness.Setup(s => s.StirrupPlacementFactor).Returns(1.1);

            _mockStirrupByDensity.Setup(s => s.StirrupDensity).Returns(4.0);

            double expectedStrength = 1.2 * 1.1 * 1.0 * 4.0; // Min(2.0, 1.0) = 1.0

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
