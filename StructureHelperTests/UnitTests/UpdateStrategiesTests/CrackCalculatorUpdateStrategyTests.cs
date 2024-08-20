using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{


    [TestFixture]
    public class CrackCalculatorUpdateStrategyTests
    {
        private Mock<IUpdateStrategy<ICrackCalculatorInputData>> _inputDataUpdateStrategyMock;
        private CrackCalculatorUpdateStrategy _crackCalculatorUpdateStrategy;

        [SetUp]
        public void SetUp()
        {
            _inputDataUpdateStrategyMock = new Mock<IUpdateStrategy<ICrackCalculatorInputData>>();
            _crackCalculatorUpdateStrategy = new CrackCalculatorUpdateStrategy(_inputDataUpdateStrategyMock.Object);
        }

        [Test]
        public void Update_WhenTargetAndSourceAreSameObject_DoesNotCallInputDataUpdateStrategy()
        {
            // Arrange
            var calculator = new Mock<ICrackCalculator>().Object;

            // Act
            _crackCalculatorUpdateStrategy.Update(calculator, calculator);

            // Assert
            _inputDataUpdateStrategyMock.Verify(s => s.Update(It.IsAny<ICrackCalculatorInputData>(), It.IsAny<ICrackCalculatorInputData>()), Times.Never);
        }

        [Test]
        public void Update_WhenSourceObjectIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var targetObject = new Mock<ICrackCalculator>().Object;

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => _crackCalculatorUpdateStrategy.Update(targetObject, null));
        }

        [Test]
        public void Update_WhenTargetObjectIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var sourceObject = new Mock<ICrackCalculator>().Object;

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => _crackCalculatorUpdateStrategy.Update(null, sourceObject));
        }

        [Test]
        public void Update_UpdatesTargetObjectNameAndCallsInputDataUpdateStrategy()
        {
            // Arrange
            var targetObject = new Mock<ICrackCalculator>();
            var sourceObject = new Mock<ICrackCalculator>();

            var targetInputData = new Mock<ICrackCalculatorInputData>().Object;
            var sourceInputData = new Mock<ICrackCalculatorInputData>().Object;

            targetObject.SetupAllProperties();
            targetObject.Object.InputData = targetInputData;
            sourceObject.SetupAllProperties();
            sourceObject.Object.InputData = sourceInputData;
            sourceObject.Setup(x => x.Name).Returns("Source Name");

            // Act
            _crackCalculatorUpdateStrategy.Update(targetObject.Object, sourceObject.Object);

            // Assert
            Assert.AreEqual("Source Name", targetObject.Object.Name);
            _inputDataUpdateStrategyMock.Verify(s => s.Update(targetInputData, sourceInputData), Times.Once);
        }

        [Test]
        public void Update_WhenTargetInputDataIsNull_CreatesNewInputDataAndUpdatesIt()
        {
            // Arrange
            var targetObject = new Mock<ICrackCalculator>();
            var sourceObject = new Mock<ICrackCalculator>();

            var sourceInputData = new Mock<ICrackCalculatorInputData>().Object;

            targetObject.SetupAllProperties();
            targetObject.Object.InputData = null;
            sourceObject.SetupAllProperties();
            sourceObject.Object.InputData = sourceInputData;

            // Act
            _crackCalculatorUpdateStrategy.Update(targetObject.Object, sourceObject.Object);

            // Assert
            Assert.IsNotNull(targetObject.Object.InputData);
            _inputDataUpdateStrategyMock.Verify(s => s.Update(targetObject.Object.InputData, sourceInputData), Times.Once);
        }
    }

}
