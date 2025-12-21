using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperTests.UnitTests.MaterialTests
{

    [TestFixture]
    public class ReinforcementLibUpdateStrategyTests
    {
        private Mock<IUpdateStrategy<ILibMaterial>> libUpdateStrategyMock;
        private ReinforcementLibUpdateStrategy strategy;

        [SetUp]
        public void SetUp()
        {
            libUpdateStrategyMock = new Mock<IUpdateStrategy<ILibMaterial>>();
            strategy = new ReinforcementLibUpdateStrategy(libUpdateStrategyMock.Object);
        }

        [Test]
        public void Update_SourceIsNull_Throws()
        {
            // Arrange
            var target = new Mock<IReinforcementLibMaterial>().Object;

            // Act & Assert
            Assert.Throws<StructureHelperException>(() =>
                strategy.Update(target, null));
        }

        [Test]
        public void Update_TargetIsNull_Throws()
        {
            // Arrange
            var source = new Mock<IReinforcementLibMaterial>().Object;

            // Act & Assert
            Assert.Throws<StructureHelperException>(() =>
                strategy.Update(null, source));
        }

        [Test]
        public void Update_SourceAndTargetAreSameInstance_DoesNothing()
        {
            // Arrange
            var materialMock = new Mock<IReinforcementLibMaterial>();

            // Act
            strategy.Update(materialMock.Object, materialMock.Object);

            // Assert
            libUpdateStrategyMock.Verify(
                x => x.Update(It.IsAny<ILibMaterial>(), It.IsAny<ILibMaterial>()),
                Times.Never);
        }

        [Test]
        public void Update_ValidObjects_DelegatesToLibMaterialUpdateStrategy()
        {
            // Arrange
            var sourceMock = new Mock<IReinforcementLibMaterial>();
            var targetMock = new Mock<IReinforcementLibMaterial>();

            // Act
            strategy.Update(targetMock.Object, sourceMock.Object);

            // Assert
            libUpdateStrategyMock.Verify(
                x => x.Update(targetMock.Object, sourceMock.Object),
                Times.Once);
        }
    }

}
