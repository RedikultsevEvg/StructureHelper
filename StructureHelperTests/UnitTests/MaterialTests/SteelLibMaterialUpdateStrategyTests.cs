using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperTests.UnitTests.MaterialTests
{


    [TestFixture]
    public class SteelLibMaterialUpdateStrategyTests
    {
        private Mock<IUpdateStrategy<ILibMaterial>> libUpdateStrategyMock;
        private SteelLibMaterialUpdateStrategy strategy;

        [SetUp]
        public void SetUp()
        {
            libUpdateStrategyMock = new Mock<IUpdateStrategy<ILibMaterial>>();
            strategy = new SteelLibMaterialUpdateStrategy(libUpdateStrategyMock.Object);
        }

        [Test]
        public void Update_SourceIsNull_Throws()
        {
            // Arrange
            var target = new Mock<ISteelLibMaterial>().Object;

            // Act & Assert
            Assert.Throws<StructureHelperException>(() =>
                strategy.Update(target, null));
        }

        [Test]
        public void Update_TargetIsNull_Throws()
        {
            // Arrange
            var source = new Mock<ISteelLibMaterial>().Object;

            // Act & Assert
            Assert.Throws<StructureHelperException>(() =>
                strategy.Update(null, source));
        }

        [Test]
        public void Update_SourceAndTargetAreSameInstance_DoesNothing()
        {
            // Arrange
            var materialMock = new Mock<ISteelLibMaterial>();

            // Act
            strategy.Update(materialMock.Object, materialMock.Object);

            // Assert
            libUpdateStrategyMock.Verify(
                x => x.Update(It.IsAny<ILibMaterial>(), It.IsAny<ILibMaterial>()),
                Times.Never);

            materialMock.VerifySet(x => x.MaxPlasticStrainRatio = It.IsAny<double>(), Times.Never);
            materialMock.VerifySet(x => x.UlsFactor = It.IsAny<double>(), Times.Never);
            materialMock.VerifySet(x => x.ThicknessFactor = It.IsAny<double>(), Times.Never);
            materialMock.VerifySet(x => x.WorkConditionFactor = It.IsAny<double>(), Times.Never);
        }

        [Test]
        public void Update_ValidObjects_UpdatesLibMaterialAndSteelProperties()
        {
            // Arrange
            var sourceMock = new Mock<ISteelLibMaterial>();
            var targetMock = new Mock<ISteelLibMaterial>();

            sourceMock.SetupGet(x => x.MaxPlasticStrainRatio).Returns(1.1);
            sourceMock.SetupGet(x => x.UlsFactor).Returns(1.2);
            sourceMock.SetupGet(x => x.ThicknessFactor).Returns(1.3);
            sourceMock.SetupGet(x => x.WorkConditionFactor).Returns(1.4);

            // Act
            strategy.Update(targetMock.Object, sourceMock.Object);

            // Assert — lib material update
            libUpdateStrategyMock.Verify(
                x => x.Update(targetMock.Object, sourceMock.Object),
                Times.Once);

            // Assert — steel-specific properties
            targetMock.VerifySet(x => x.MaxPlasticStrainRatio = 1.1, Times.Once);
            targetMock.VerifySet(x => x.UlsFactor = 1.2, Times.Once);
            targetMock.VerifySet(x => x.ThicknessFactor = 1.3, Times.Once);
            targetMock.VerifySet(x => x.WorkConditionFactor = 1.4, Times.Once);
        }
    }

}
