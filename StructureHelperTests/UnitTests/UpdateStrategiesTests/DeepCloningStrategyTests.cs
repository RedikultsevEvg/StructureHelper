using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{


    [TestFixture]
    public class DeepCloningStrategyTests
    {
        private DeepCloningStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _strategy = new DeepCloningStrategy();
        }

        [Test]
        public void Clone_WithNullOriginal_ReturnsNull()
        {
            // Act
            var result = _strategy.Clone<object>(null);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Clone_WhenObjectAlreadyCloned_ReturnsExistingClone()
        {
            // Arrange
            var original = new Mock<ICloneable>().Object;
            var expectedClone = new Mock<ICloneable>().Object;

            // Use reflection to populate the internal dictionary (_clonedObjects)
            var clonedObjects = new Dictionary<object, object> { { original, expectedClone } };
            var internalField = typeof(DeepCloningStrategy).GetField("_clonedObjects", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            internalField.SetValue(_strategy, clonedObjects);

            // Act
            var result = _strategy.Clone(original);

            // Assert
            Assert.That(result, Is.SameAs(expectedClone));
        }

        [Test]
        public void Clone_WithCustomCloneStrategy_UsesCloneStrategyToCloneObject()
        {
            // Arrange
            var original = new Mock<ICloneable>().Object;
            var expectedClone = new Mock<ICloneable>().Object;

            var cloneStrategyMock = new Mock<ICloneStrategy<ICloneable>>();
            cloneStrategyMock.Setup(cs => cs.GetClone(original)).Returns(expectedClone);

            // Act
            var result = _strategy.Clone(original, cloneStrategyMock.Object);

            // Assert
            Assert.That(result, Is.SameAs(expectedClone));
            cloneStrategyMock.Verify(cs => cs.GetClone(original), Times.Once);
        }

        [Test]
        public void Clone_WhenObjectImplementsICloneable_UsesCloneMethod()
        {
            // Arrange
            var originalMock = new Mock<ICloneable>();
            var expectedClone = new Mock<ICloneable>().Object;

            originalMock.Setup(o => o.Clone()).Returns(expectedClone);

            // Act
            var result = _strategy.Clone(originalMock.Object);

            // Assert
            Assert.That(result, Is.SameAs(expectedClone));
            originalMock.Verify(o => o.Clone(), Times.Once);
        }

        [Test]
        public void Clone_WhenObjectNotCloneableAndNoCloneStrategy_ThrowsStructureHelperException()
        {
            // Arrange
            var original = new object(); // Not ICloneable

            // Act & Assert
            var exception = Assert.Throws<StructureHelperException>(() => _strategy.Clone(original));
            Assert.That(exception.Message, Does.Contain("object is not IClonable and cloning strategy is null"));
        }

        [Test]
        public void Clone_AddsClonedObjectToDictionary()
        {
            // Arrange
            var originalMock = new Mock<ICloneable>();
            var expectedClone = new Mock<ICloneable>().Object;

            originalMock.Setup(o => o.Clone()).Returns(expectedClone);

            // Act
            var result = _strategy.Clone(originalMock.Object);

            // Use reflection to check the private dictionary (_clonedObjects)
            var internalField = typeof(DeepCloningStrategy).GetField("_clonedObjects", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var clonedObjects = (Dictionary<object, object>)internalField.GetValue(_strategy);

            // Assert
            Assert.That(clonedObjects[originalMock.Object], Is.SameAs(expectedClone));
        }
    }

}
