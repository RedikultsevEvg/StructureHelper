using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System.Collections.Generic;
namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{


    [TestFixture]
    public class HasForceActionUpdateCloningStrategyTests
    {
        private Mock<ICloningStrategy> _cloningStrategyMock;
        private HasForceActionUpdateCloningStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _cloningStrategyMock = new Mock<ICloningStrategy>();
            _strategy = new HasForceActionUpdateCloningStrategy(_cloningStrategyMock.Object);
        }

        [Test]
        public void Update_WithNullCloningStrategy_ThrowsStructureHelperException()
        {
            var targetObjectMock = new Mock<IHasForceActions>();
            var sourceObjectMock = new Mock<IHasForceActions>();
            Assert.Throws<StructureHelperException>(() =>
            {
                new HasForceActionUpdateCloningStrategy(null).Update(targetObjectMock.Object, sourceObjectMock.Object);
            });
        }

        [Test]
        public void Update_WithNullSourceObject_ThrowsStructureHelperException()
        {
            var targetObjectMock = new Mock<IHasForceActions>();
            Assert.Throws<StructureHelperException>(() =>
            {
                _strategy.Update(targetObjectMock.Object, null);
            });
        }

        [Test]
        public void Update_WithNullTargetObject_ThrowsStructureHelperException()
        {
            var sourceObjectMock = new Mock<IHasForceActions>();
            Assert.Throws<StructureHelperException>(() =>
            {
                _strategy.Update(null, sourceObjectMock.Object);
            });
        }

        [Test]
        public void Update_WithSameObjects_DoesNotPerformCloning()
        {
            var sourceObjectMock = new Mock<IHasForceActions>();
            _strategy.Update(sourceObjectMock.Object, sourceObjectMock.Object);

            _cloningStrategyMock.Verify(cs => cs.Clone(It.IsAny<object>(), null), Times.Never);
        }

        [Test]
        public void Update_ClonesEachForceActionAndAddsToTargetObject()
        {
            // Arrange
            var targetForceActions = new List<IForceAction>();
            var targetObjectMock = new Mock<IHasForceActions>();
            var sourceObjectMock = new Mock<IHasForceActions>();

            var sourceForceActions = new List<IForceAction> { new ForceFactoredList(), new ForceFactoredList() };
            var clonedForceActions = new List<IForceAction> { new ForceFactoredList(), new ForceFactoredList() };

            sourceObjectMock.SetupGet(s => s.ForceActions).Returns(sourceForceActions);
            targetObjectMock.SetupGet(t => t.ForceActions).Returns(targetForceActions);

            _cloningStrategyMock.Setup(cs => cs.Clone(sourceForceActions[0], null))
                .Returns(clonedForceActions[0]);
            _cloningStrategyMock.Setup(cs => cs.Clone(sourceForceActions[1], null))
                .Returns(clonedForceActions[1]);

            // Act
            _strategy.Update(targetObjectMock.Object, sourceObjectMock.Object);

            // Assert
            Assert.That(targetForceActions, Has.Count.EqualTo(2));
            Assert.That(targetForceActions, Is.EquivalentTo(clonedForceActions));

            _cloningStrategyMock.Verify(cs => cs.Clone(sourceForceActions[0], null), Times.Once);
            _cloningStrategyMock.Verify(cs => cs.Clone(sourceForceActions[1], null), Times.Once);
        }
    }

}
