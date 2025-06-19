using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears.Logics;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelperTests.UnitTests.BeamShearTests
{


    [TestFixture]
    public class GetLongitudinalForceFactorLogicTests
    {
        private Mock<IInclinedSection> _mockSection;
        private Mock<ICheckEntityLogic<IInclinedSection>> _mockCheckLogic;
        private Mock<IShiftTraceLogger> _mockLogger;
        private GetLongitudinalForceFactorLogic _logic;

        [SetUp]
        public void SetUp()
        {
            _mockSection = new Mock<IInclinedSection>();
            _mockCheckLogic = new Mock<ICheckEntityLogic<IInclinedSection>>();
            _mockLogger = new Mock<IShiftTraceLogger>();

            _logic = new GetLongitudinalForceFactorLogic(_mockLogger.Object)
            {
                InclinedSection = _mockSection.Object,
                LongitudinalForce = 0
            };

            // Inject mock check logic
            typeof(GetLongitudinalForceFactorLogic)
                .GetField("checkInclinedSectionLogic", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_logic, _mockCheckLogic.Object);
        }

        [Test]
        public void GetFactor_Returns1_WhenLongitudinalForceIsZero()
        {
            // Arrange
            _mockCheckLogic.Setup(c => c.Check()).Returns(true);

            // Act
            var result = _logic.GetFactor();

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void GetFactor_ComputesCorrectly_ForTension()
        {
            // Arrange
            _logic.LongitudinalForce = 100000; // tension
            _mockSection.Setup(s => s.WebWidth).Returns(0.3);
            _mockSection.Setup(s => s.FullDepth).Returns(0.5);
            _mockSection.Setup(s => s.ConcreteTensionStrength).Returns(2000000);
            _mockCheckLogic.Setup(c => c.Check()).Returns(true);

            double area = 0.3 * 0.5;
            double stress = 100000 / area;
            double ratio = stress / 2000000;
            double expected = Math.Max(1 - 0.5 * ratio, 0);

            // Act
            var result = _logic.GetFactor();

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-6));
        }

        [Test]
        public void GetFactor_ComputesCorrectly_ForCompression_WithinFirstLimit()
        {
            // Arrange
            _logic.LongitudinalForce = -10000; // compression
            _mockSection.Setup(s => s.WebWidth).Returns(0.3);
            _mockSection.Setup(s => s.FullDepth).Returns(0.5);
            _mockSection.Setup(s => s.ConcreteCompressionStrength).Returns(25000000);
            _mockCheckLogic.Setup(c => c.Check()).Returns(true);

            double area = 0.3 * 0.5;
            double stress = 10000 / area;
            double ratio = stress / 25000000;
            double expected = 1 + ratio;

            // Act
            var result = _logic.GetFactor();

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-6));
        }

        [TestCase(-11250000 / 3.99)]
        [TestCase(-11250000 / 3)]
        [TestCase(-11250000)]
        public void GetFactor_ComputesCorrectly_ForCompression_HighRatio(double force)
        {
            // Arrange
            _logic.LongitudinalForce = force; // compression
            _mockSection.Setup(s => s.WebWidth).Returns(0.3);
            _mockSection.Setup(s => s.FullDepth).Returns(0.5);
            _mockSection.Setup(s => s.ConcreteCompressionStrength).Returns(25000000);
            _mockCheckLogic.Setup(c => c.Check()).Returns(true);

            double area = 0.3 * 0.5;
            double stress = - force / area;
            double ratio = stress / 25000000;

            Assert.That(ratio, Is.GreaterThan(0.75)); // high compression branch

            double expected = Math.Max(5 * (1 - ratio), 0);

            // Act
            var result = _logic.GetFactor();

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-6));
        }

        [Test]
        public void GetFactor_Throws_WhenCheckFails()
        {
            // Arrange
            _mockCheckLogic.Setup(c => c.Check()).Returns(false);
            _mockCheckLogic.Setup(c => c.CheckResult).Returns("Invalid section");

            // Act & Assert
            var ex = Assert.Throws<StructureHelperException>(() => _logic.GetFactor());
            Assert.That(ex.Message, Does.Contain("Invalid section"));
        }
    }

}
