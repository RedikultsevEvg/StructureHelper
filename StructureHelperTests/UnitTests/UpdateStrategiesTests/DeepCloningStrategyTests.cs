using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{
    [TestFixture]
    public class DeepCloningStrategyTests
    {
        private DeepCloningStrategy _strategy;

        public sealed class TestCloneable : ICloneStrategy<TestCloneable>
        {
            public TestCloneable? CloneResult { get; set; }

            public TestCloneable GetClone(
                TestCloneable sourceObject,
                CloneMode cloneMode = CloneMode.NewEntity,
                CloneDepth cloneDepth = CloneDepth.Deep)
                => CloneResult ?? sourceObject;
        }

        [SetUp]
        public void SetUp()
        {
            _strategy = new DeepCloningStrategy();
        }

        [Test]
        public void Clone_WithNullOriginal_ReturnsNull()
        {
            var result = _strategy.Clone<object>(null);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Clone_WhenObjectAlreadyCloned_ReturnsExistingClone()
        {
            var original = new TestCloneable();
            var expectedClone = new TestCloneable();

            var clonedObjects = new Dictionary<object, object> { { original, expectedClone } };
            var internalField = typeof(DeepCloningStrategy).GetField("_clonedObjects", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            internalField.SetValue(_strategy, clonedObjects);

            var result = _strategy.Clone(original);

            Assert.That(result, Is.SameAs(expectedClone));
        }

        [Test]
        public void Clone_WithCustomCloneStrategy_UsesCloneStrategyToCloneObject()
        {
            var original = new TestCloneable();
            var expectedClone = new TestCloneable();

            var cloneStrategyMock = new Mock<ICloneStrategy<TestCloneable>>();
            cloneStrategyMock.Setup(cs => cs.GetClone(original, CloneMode.NewEntity)).Returns(expectedClone);

            var result = _strategy.Clone(original, cloneStrategyMock.Object);

            Assert.That(result, Is.SameAs(expectedClone));
            cloneStrategyMock.Verify(cs => cs.GetClone(original, CloneMode.NewEntity), Times.Once);
        }

        [Test]
        public void Clone_WhenObjectImplementsSelfStrategy_UsesSelfStrategy()
        {
            var original = new TestCloneable();
            var expectedClone = new TestCloneable { CloneResult = new TestCloneable() };
            original.CloneResult = expectedClone;

            var result = _strategy.Clone(original);

            Assert.That(result, Is.SameAs(expectedClone));
        }

        [Test]
        public void Clone_WhenObjectNotCloneableAndNoCloneStrategy_ThrowsStructureHelperException()
        {
            var original = new object();

            var exception = Assert.Throws<StructureHelperException>(() => _strategy.Clone(original));
            Assert.That(exception.Message, Does.Contain("not IClonable"));
        }

        [Test]
        public void Clone_AddsClonedObjectToDictionary()
        {
            var original = new TestCloneable();
            var expectedClone = new TestCloneable();
            original.CloneResult = expectedClone;

            var result = _strategy.Clone(original);

            var internalField = typeof(DeepCloningStrategy).GetField("_clonedObjects", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var clonedObjects = (Dictionary<object, object>)internalField.GetValue(_strategy);

            Assert.That(clonedObjects[original], Is.SameAs(expectedClone));
        }
    }
}
