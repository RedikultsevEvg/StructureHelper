using NUnit.Framework;
using Moq;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{


    [TestFixture]
    public class LibMaterialUpdateStrategyTests
    {
        private Mock<ILibMaterial> mockTargetMaterial;
        private Mock<ILibMaterial> mockSourceMaterial;
        private LibMaterialUpdateStrategy updateStrategy;

        [SetUp]
        public void Setup()
        {
            mockTargetMaterial = new Mock<ILibMaterial>();
            mockSourceMaterial = new Mock<ILibMaterial>();
            updateStrategy = new LibMaterialUpdateStrategy();

            var targetSafetyFactors = new List<IMaterialSafetyFactor>();
            mockTargetMaterial.Setup(t => t.SafetyFactors).Returns(targetSafetyFactors);
        }

        [Test]
        public void Update_ShouldThrowException_WhenTargetAndSourceAreDifferentTypes()
        {
            // Arrange
            var mockTarget1 = new ConcreteLibMaterial();
            var mockTarget2 = new ReinforcementLibMaterial();

            // Act & Assert
            Assert.Throws<StructureHelperException>(() =>  updateStrategy.Update(mockTarget1, mockTarget2));
        }

        [Test]
        public void Update_ShouldCopyMaterialEntityAndMaterialLogic()
        {
            // Arrange
            var materialEntity = new Mock<ILibMaterialEntity>().Object;
            var materialLogic = new Mock<IMaterialLogic>().Object;

            mockSourceMaterial.Setup(s => s.MaterialEntity).Returns(materialEntity);
            mockSourceMaterial.Setup(s => s.MaterialLogic).Returns(materialLogic);

            // Act
            updateStrategy.Update(mockTargetMaterial.Object, mockSourceMaterial.Object);

            // Assert
            mockTargetMaterial.VerifySet(t => t.MaterialEntity = materialEntity, Times.Once);
            mockTargetMaterial.VerifySet(t => t.MaterialLogic = materialLogic, Times.Once);
        }

        //[Test]
        public void Update_ShouldClearAndCopySafetyFactors()
        {
            // Arrange
            var mockSafetyFactor1 = new Mock<IMaterialSafetyFactor>();
            var mockSafetyFactor2 = new Mock<IMaterialSafetyFactor>();
            var safetyFactors = new List<IMaterialSafetyFactor> { mockSafetyFactor1.Object, mockSafetyFactor2.Object };
            var targetSafetyFactors = new List<IMaterialSafetyFactor>();

            mockSourceMaterial.Setup(s => s.SafetyFactors).Returns(safetyFactors);
            mockSafetyFactor1.Setup(f => f.Clone()).Returns(mockSafetyFactor1.Object);
            mockSafetyFactor2.Setup(f => f.Clone()).Returns(mockSafetyFactor2.Object);

            mockTargetMaterial.Setup(t => t.SafetyFactors).Returns(targetSafetyFactors);

            // Act
            updateStrategy.Update(mockTargetMaterial.Object, mockSourceMaterial.Object);

            // Assert
            //mockTargetMaterial.Verify(t => t.SafetyFactors.Clear(), Times.Once);
            //mockTargetMaterial.Verify(t => t.SafetyFactors.Add(mockSafetyFactor1.Object), Times.Once);
            //mockTargetMaterial.Verify(t => t.SafetyFactors.Add(mockSafetyFactor2.Object), Times.Once);
        }

        [Test]
        public void Update_ShouldNotPerformUpdate_WhenObjectsAreReferenceEqual()
        {
            //Arrange

            // Act
            updateStrategy.Update(mockTargetMaterial.Object, mockTargetMaterial.Object);

            // Assert
            mockTargetMaterial.VerifySet(t => t.MaterialEntity = It.IsAny<ILibMaterialEntity>(), Times.Never);
            //mockTargetMaterial.Verify(t => t.SafetyFactors.Clear(), Times.Never);
            mockTargetMaterial.VerifySet(t => t.MaterialLogic = It.IsAny<IMaterialLogic>(), Times.Never);
        }
    }

}
