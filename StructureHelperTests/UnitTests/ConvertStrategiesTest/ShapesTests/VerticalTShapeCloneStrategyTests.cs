using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest.ShapesTests
{
        [TestFixture]
        public class VerticalTShapeCloneStrategyTests
        {
            private Mock<IUpdateStrategy<IVerticalTShape>> updateStrategyMock;
            private Mock<IVerticalTShape> sourceMock;

            [SetUp]
            public void SetUp()
            {
                updateStrategyMock = new Mock<IUpdateStrategy<IVerticalTShape>>();
                sourceMock = new Mock<IVerticalTShape>();
            }

            [Test]
            public void GetClone_WhenSourceIsValid_ReturnsNewInstance()
            {
                // Arrange
                var sut = new VerticalTShapeCloneStrategy(updateStrategyMock.Object);

                // Act
                var clone = sut.GetClone(sourceMock.Object);

                // Assert
                Assert.That(clone, Is.Not.Null);
                Assert.That(clone, Is.Not.SameAs(sourceMock.Object));
            }

            [Test]
            public void GetClone_CallsUpdateStrategy()
            {
                // Arrange
                var sut = new VerticalTShapeCloneStrategy(updateStrategyMock.Object);

                // Act
                var clone = sut.GetClone(sourceMock.Object);

                // Assert
                updateStrategyMock.Verify(
                    x => x.Update(It.IsAny<IVerticalTShape>(), sourceMock.Object),
                    Times.Once);
            }

            [Test]
            public void GetClone_WhenSourceIsNull_Throws()
            {
                // Arrange
                var sut = new VerticalTShapeCloneStrategy(updateStrategyMock.Object);

                // Act & Assert
                Assert.Throws<StructureHelperException>(() => sut.GetClone(null));
                // If CheckObject.ThrowIfNull throws StructureHelperException,
                // replace Exception with StructureHelperException.
            }

            [Test]
            public void GetClone_WhenNoStrategyInjected_UsesDefaultStrategy()
            {
                // Arrange
                var sut = new VerticalTShapeCloneStrategy();
            var source = new Mock<IVerticalTShape>();
            source.Setup(s => s.FullHeight).Returns(0.6);
            source.Setup(s => s.WebWidth).Returns(0.2);
            source.Setup(s => s.FlangeHeight).Returns(0.2);
            source.Setup(s => s.FlangeWidth).Returns(0.4);
                var sourceObject = source.Object;

                // Act
                var clone = sut.GetClone(sourceObject);

                // Assert
                Assert.That(clone, Is.Not.Null);
                Assert.That(clone, Is.Not.SameAs(sourceObject));
            }
        }
}
