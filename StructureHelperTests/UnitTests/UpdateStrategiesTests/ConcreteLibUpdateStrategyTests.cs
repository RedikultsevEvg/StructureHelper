using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;
using System;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{


    namespace YourNamespace.Tests
    {
        [TestFixture]
        public class ConcreteLibUpdateStrategyTests
        {
            private Mock<IUpdateStrategy<ILibMaterial>> mockLibUpdateStrategy;
            private ConcreteLibUpdateStrategy strategy;

            [SetUp]
            public void Setup()
            {
                mockLibUpdateStrategy = new Mock<IUpdateStrategy<ILibMaterial>>();
                strategy = new ConcreteLibUpdateStrategy(mockLibUpdateStrategy.Object);
            }

            [Test]
            public void Update_ShouldUpdateTargetObject_WhenSourceAndTargetAreNotTheSame()
            {
                // Arrange
                var targetMock = new Mock<IConcreteLibMaterial>();
                var sourceMock = new Mock<IConcreteLibMaterial>();

                sourceMock.Setup(s => s.TensionForULS).Returns(false);
                sourceMock.Setup(s => s.TensionForSLS).Returns(true);
                sourceMock.Setup(s => s.RelativeHumidity).Returns(0.75);

                // Act
                strategy.Update(targetMock.Object, sourceMock.Object);

                // Assert
                mockLibUpdateStrategy.Verify(l => l.Update(targetMock.Object, sourceMock.Object), Times.Once);
                targetMock.VerifySet(t => t.TensionForULS = false);
                targetMock.VerifySet(t => t.TensionForSLS = true);
                targetMock.VerifySet(t => t.RelativeHumidity = 0.75);
            }

            [Test]
            public void Update_ShouldNotUpdate_WhenSourceAndTargetAreSame()
            {
                // Arrange
                var targetMock = new Mock<IConcreteLibMaterial>();

                // Act
                strategy.Update(targetMock.Object, targetMock.Object);

                // Assert
                mockLibUpdateStrategy.Verify(l => l.Update(It.IsAny<IConcreteLibMaterial>(), It.IsAny<IConcreteLibMaterial>()), Times.Never);
                targetMock.VerifySet(t => t.TensionForULS = It.IsAny<bool>(), Times.Never);
                targetMock.VerifySet(t => t.TensionForSLS = It.IsAny<bool>(), Times.Never);
                targetMock.VerifySet(t => t.RelativeHumidity = It.IsAny<double>(), Times.Never);
            }

            [Test]
            public void Update_ShouldThrowStructureHelperException_WhenObjectsAreOfDifferentTypes()
            {
                // Arrange
                var targetMock = new Mock<IConcreteLibMaterial>();
                var sourceMock = new Mock<ConcreteLibMaterial>();

                // Act & Assert
                Assert.Throws<StructureHelperException>(() => strategy.Update(targetMock.Object, sourceMock.Object));
            }
        }
    }

}
