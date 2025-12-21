using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperTests.UnitTests.MaterialTests
{


    [TestFixture]
    public class ConcreteLibUpdateStrategyTests
    {
        private Mock<IUpdateStrategy<ILibMaterial>> libUpdateStrategyMock;
        private ConcreteLibUpdateStrategy strategy;

        [SetUp]
        public void SetUp()
        {
            libUpdateStrategyMock = new Mock<IUpdateStrategy<ILibMaterial>>();
            strategy = new ConcreteLibUpdateStrategy(libUpdateStrategyMock.Object);
        }

        [Test]
        public void Update_SourceIsNull_Throws()
        {
            // Arrange
            var target = new Mock<IConcreteLibMaterial>().Object;

            // Act & Assert
            Assert.Throws<StructureHelperException>(() =>
                strategy.Update(target, null));
        }

        [Test]
        public void Update_TargetIsNull_Throws()
        {
            // Arrange
            var source = new Mock<IConcreteLibMaterial>().Object;

            // Act & Assert
            Assert.Throws<StructureHelperException>(() =>
                strategy.Update(null, source));
        }

        [Test]
        public void Update_SourceAndTargetAreSameInstance_DoesNothing()
        {
            // Arrange
            var materialMock = new Mock<IConcreteLibMaterial>();

            // Act
            strategy.Update(materialMock.Object, materialMock.Object);

            // Assert
            libUpdateStrategyMock.Verify(
                x => x.Update(It.IsAny<ILibMaterial>(), It.IsAny<ILibMaterial>()),
                Times.Never);
        }

        [Test]
        public void Update_ValidObjects_UpdatesLibMaterialAndConcreteProperties()
        {
            // Arrange
            var sourceMock = new Mock<IConcreteLibMaterial>();
            var targetMock = new Mock<IConcreteLibMaterial>();

            sourceMock.SetupGet(x => x.TensionForULS).Returns(false);
            sourceMock.SetupGet(x => x.TensionForSLS).Returns(true);
            sourceMock.SetupGet(x => x.RelativeHumidity).Returns(0.65);
            sourceMock.SetupGet(x => x.MinAge).Returns(7);
            sourceMock.SetupGet(x => x.MaxAge).Returns(365);

            // Act
            strategy.Update(targetMock.Object, sourceMock.Object);

            // Assert — lib material delegation
            libUpdateStrategyMock.Verify(
                x => x.Update(targetMock.Object, sourceMock.Object),
                Times.Once);

            // Assert — concrete-specific properties copied
            targetMock.VerifySet(x => x.TensionForULS = false, Times.Once);
            targetMock.VerifySet(x => x.TensionForSLS = true, Times.Once);
            targetMock.VerifySet(x => x.RelativeHumidity = 0.65, Times.Once);
            targetMock.VerifySet(x => x.MinAge = 7, Times.Once);
            targetMock.VerifySet(x => x.MaxAge = 365, Times.Once);
        }
    }

}
