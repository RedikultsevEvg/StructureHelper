using Moq;
using NUnit.Framework;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials.Logics;
using StructureHelperLogics.Models.Materials;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{


    [TestFixture]
    public class HasMaterialsUpdateCloningStrategyTests
    {
        private Mock<ICloningStrategy> _cloningStrategyMock;
        private HasMaterialsUpdateCloningStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _cloningStrategyMock = new Mock<ICloningStrategy>();
            _strategy = new HasMaterialsUpdateCloningStrategy(_cloningStrategyMock.Object);
        }

        [Test]
        public void Update_WithNullCloningStrategy_ThrowsStructureHelperException()
        {
            var targetObjectMock = new Mock<IHasHeadMaterials>();
            var sourceObjectMock = new Mock<IHasHeadMaterials>();
            Assert.Throws<StructureHelperException>(() =>
            {
                new HasMaterialsUpdateCloningStrategy(null).Update(targetObjectMock.Object, sourceObjectMock.Object);
            });
        }

        [Test]
        public void Update_WithNullSourceObject_ThrowsStructureHelperException()
        {
            var targetObjectMock = new Mock<IHasHeadMaterials>();
            Assert.Throws<StructureHelperException>(() =>
            {
                _strategy.Update(targetObjectMock.Object, null);
            });
        }

        [Test]
        public void Update_WithNullTargetObject_ThrowsStructureHelperException()
        {
            var sourceObjectMock = new Mock<IHasHeadMaterials>();
            Assert.Throws<StructureHelperException>(() =>
            {
                _strategy.Update(null, sourceObjectMock.Object);
            });
        }

        [Test]
        public void Update_WithSameObjects_DoesNotPerformCloning()
        {
            var sourceObjectMock = new Mock<IHasHeadMaterials>();
            _strategy.Update(sourceObjectMock.Object, sourceObjectMock.Object);

            _cloningStrategyMock.Verify(cs => cs.Clone(It.IsAny<IHeadMaterial>(), null), Times.Never);
        }

        [Test]
        public void Update_ClonesAndAddsEachMaterialToTarget()
        {
            // Arrange
            var targetMaterials = new List<IHeadMaterial>();
            var targetObjectMock = new Mock<IHasHeadMaterials>();
            targetObjectMock.SetupGet(t => t.HeadMaterials).Returns(targetMaterials);

            var sourceMaterials = new List<IHeadMaterial>
        {
            Mock.Of<IHeadMaterial>(m => m.Name == "Material1"),
            Mock.Of<IHeadMaterial>(m => m.Name == "Material2")
        };
            var sourceObjectMock = new Mock<IHasHeadMaterials>();
            sourceObjectMock.SetupGet(s => s.HeadMaterials).Returns(sourceMaterials);

            var clonedMaterial1 = Mock.Of<IHeadMaterial>(m => m.Name == "ClonedMaterial1");
            var clonedMaterial2 = Mock.Of<IHeadMaterial>(m => m.Name == "ClonedMaterial2");

            _cloningStrategyMock.Setup(cs => cs.Clone(sourceMaterials[0], null)).Returns(clonedMaterial1);
            _cloningStrategyMock.Setup(cs => cs.Clone(sourceMaterials[1], null)).Returns(clonedMaterial2);

            // Act
            _strategy.Update(targetObjectMock.Object, sourceObjectMock.Object);

            // Assert
            //Assert.IsEmpty(targetMaterials, "Target materials should be cleared before adding new ones.");
            Assert.That(targetMaterials, Has.Count.EqualTo(2));
            Assert.That(targetMaterials, Contains.Item(clonedMaterial1));
            Assert.That(targetMaterials, Contains.Item(clonedMaterial2));

            _cloningStrategyMock.Verify(cs => cs.Clone(sourceMaterials[0], null), Times.Once);
            _cloningStrategyMock.Verify(cs => cs.Clone(sourceMaterials[1], null), Times.Once);
        }
    }

}
