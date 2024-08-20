using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{

    [TestFixture]
    public class CrackInputDataUpdateStrategyTests
    {
        [Test]
        public void Update_WithValidInput_UpdatesTargetObject()
        {
            // Arrange
            var mockUserCrackInputDataUpdateStrategy = new Mock<IUpdateStrategy<IUserCrackInputData>>();
            var targetObject = new Mock<ICrackCalculatorInputData>();
            var sourceObject = new Mock<ICrackCalculatorInputData>();

            var forceActions = new List<IForceAction> { new Mock<IForceAction>().Object };
            var primitives = new List<INdmPrimitive> { new Mock<INdmPrimitive>().Object };
            var userCrackInputData = new Mock<IUserCrackInputData>();

            sourceObject.Setup(s => s.ForceActions).Returns(forceActions);
            sourceObject.Setup(s => s.Primitives).Returns(primitives);
            sourceObject.Setup(s => s.UserCrackInputData).Returns(userCrackInputData.Object);

            targetObject.Setup(t => t.ForceActions).Returns(new List<IForceAction>());
            targetObject.Setup(t => t.Primitives).Returns(new List<INdmPrimitive>());
            targetObject.SetupProperty(t => t.UserCrackInputData, new Mock<IUserCrackInputData>().Object);

            var strategy = new CrackInputDataUpdateStrategy(mockUserCrackInputDataUpdateStrategy.Object);

            // Act
            strategy.Update(targetObject.Object, sourceObject.Object);

            // Assert
            Assert.AreEqual(forceActions, targetObject.Object.ForceActions);
            Assert.AreEqual(primitives, targetObject.Object.Primitives);
            mockUserCrackInputDataUpdateStrategy.Verify(s => s.Update(It.IsAny<IUserCrackInputData>(), It.IsAny<IUserCrackInputData>()), Times.Once);
        }

        [Test]
        public void Update_WhenTargetAndSourceAreSame_DoesNotUpdate()
        {
            // Arrange
            var mockUserCrackInputDataUpdateStrategy = new Mock<IUpdateStrategy<IUserCrackInputData>>();
            var targetObject = new Mock<ICrackCalculatorInputData>();

            var strategy = new CrackInputDataUpdateStrategy(mockUserCrackInputDataUpdateStrategy.Object);

            // Act
            strategy.Update(targetObject.Object, targetObject.Object);

            // Assert
            mockUserCrackInputDataUpdateStrategy.Verify(s => s.Update(It.IsAny<IUserCrackInputData>(), It.IsAny<IUserCrackInputData>()), Times.Never);
        }

        [Test]
        public void Update_WithNullTarget_ThrowsStructureHelperException()
        {
            // Arrange
            var mockUserCrackInputDataUpdateStrategy = new Mock<IUpdateStrategy<IUserCrackInputData>>();
            var sourceObject = new Mock<ICrackCalculatorInputData>();

            var strategy = new CrackInputDataUpdateStrategy(mockUserCrackInputDataUpdateStrategy.Object);

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => strategy.Update(null, sourceObject.Object));
        }
                
        [Test]
        public void Update_WithNullSource_ThrowsStructureHelperException()
        {
            // Arrange
            var mockUserCrackInputDataUpdateStrategy = new Mock<IUpdateStrategy<IUserCrackInputData>>();
            var targetObject = new Mock<ICrackCalculatorInputData>();

            var strategy = new CrackInputDataUpdateStrategy(mockUserCrackInputDataUpdateStrategy.Object);

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => strategy.Update(targetObject.Object, null));
        }
    }

}
