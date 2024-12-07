using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{


    [TestFixture]
    public class HasPrimitivesUpdateCloningStrategyTests
    {
        private Mock<ICloningStrategy> _cloningStrategyMock;
        private HasPrimitivesUpdateCloningStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _cloningStrategyMock = new Mock<ICloningStrategy>();
            _strategy = new HasPrimitivesUpdateCloningStrategy(_cloningStrategyMock.Object);
        }

        [Test]
        public void Update_WithNullCloningStrategy_ThrowsStructureHelperException()
        {
            var targetObjectMock = new Mock<IHasPrimitives>();
            var sourceObjectMock = new Mock<IHasPrimitives>();
            Assert.Throws<StructureHelperException>(() =>
            {
                new HasPrimitivesUpdateCloningStrategy(null).Update(targetObjectMock.Object, sourceObjectMock.Object);
            });
        }

        [Test]
        public void Update_WithNullSourceObject_ThrowsStructureHelperException()
        {
            var targetObjectMock = new Mock<IHasPrimitives>();
            Assert.Throws<StructureHelperException>(() =>
            {
                _strategy.Update(targetObjectMock.Object, null);
            });
        }

        [Test]
        public void Update_WithNullTargetObject_ThrowsStructureHelperException()
        {
            var sourceObjectMock = new Mock<IHasPrimitives>();
            Assert.Throws<StructureHelperException>(() =>
            {
                _strategy.Update(null, sourceObjectMock.Object);
            });
        }

        [Test]
        public void Update_WithSameObjects_DoesNotPerformCloning()
        {
            var sourceObjectMock = new Mock<IHasPrimitives>();
            _strategy.Update(sourceObjectMock.Object, sourceObjectMock.Object);

            _cloningStrategyMock.Verify(cs => cs.Clone(It.IsAny<INdmPrimitive>(), null), Times.Never);
        }

        [Test]
        public void Update_ProcessesEachPrimitiveCorrectly()
        {
            // Arrange
            ICloningStrategy deepCloningStrategy = new DeepCloningStrategy();
            var targetPrimitives = new List<INdmPrimitive>();
            var targetObjectMock = new Mock<IHasPrimitives>();
            targetObjectMock.SetupGet(t => t.Primitives).Returns(targetPrimitives);

            var sourcePrimitive1 = Mock.Of<INdmPrimitive>(p => p.Name == "sp1" && p.NdmElement == new NdmElement());
            var sourceHostPrimitive = Mock.Of<INdmPrimitive>(p => p.Name == "shp" && p.NdmElement == new NdmElement());
            var sourcePrimitive2 = Mock.Of<IRebarNdmPrimitive>(p => p.Name == "sp2" && p.NdmElement == new NdmElement() && p.HostPrimitive == sourceHostPrimitive);
            var sourcePrimitives = new List<INdmPrimitive>
            { 
                sourcePrimitive1,
                sourcePrimitive2
            };
            var sourceObjectMock = new Mock<IHasPrimitives>();
            sourceObjectMock.SetupGet(s => s.Primitives).Returns(sourcePrimitives);

            var clonedPrimitive1 = Mock.Of<INdmPrimitive>(p => p.Name == "cp1" && p.NdmElement == new NdmElement());
            var clonedPrimitive2 = Mock.Of<IRebarNdmPrimitive>(p => p.Name == "cp2" && p.NdmElement == new NdmElement() && p.HostPrimitive == sourceHostPrimitive);
            var clonedHostPrimitive = Mock.Of<INdmPrimitive>(p => p.Name == "chp" && p.NdmElement == new NdmElement());

            _cloningStrategyMock.Setup(cs => cs.Clone(sourcePrimitive1, null)).Returns(clonedPrimitive1);
            _cloningStrategyMock.Setup(cs => cs.Clone(It.Is<INdmPrimitive>(p => p.Name == "sp2"), null)).Returns(clonedPrimitive2);
            //_cloningStrategyMock.Setup(cs => cs.Clone(sourcePrimitive2, null)).Returns(clonedPrimitive2);
            _cloningStrategyMock.Setup(cs => cs.Clone(sourcePrimitive2.HostPrimitive, null)).Returns(clonedHostPrimitive);

            // Act
            _strategy.Update(targetObjectMock.Object, sourceObjectMock.Object);

            // Assert
            //Assert.IsEmpty(targetPrimitives, "Target primitives should be cleared before adding new ones.");
            Assert.That(targetPrimitives, Has.Count.EqualTo(2));
            Assert.That(targetPrimitives, Contains.Item(clonedPrimitive1));
            Assert.That(targetPrimitives, Contains.Item(clonedPrimitive2));

            _cloningStrategyMock.Verify(cs => cs.Clone(sourcePrimitive1, null), Times.Once);
            _cloningStrategyMock.Verify(cs => cs.Clone(It.Is<INdmPrimitive>(p => p.Name == "sp2"), null), Times.Once);
            //_cloningStrategyMock.Verify(cs => cs.Clone(sourcePrimitive2, null), Times.Once);
            _cloningStrategyMock.Verify(cs => cs.Clone(sourcePrimitive2.HostPrimitive, null), Times.Once);
        }
    }
}

