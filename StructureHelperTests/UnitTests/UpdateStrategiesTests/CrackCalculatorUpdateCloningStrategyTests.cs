using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{
    [TestFixture]
    public class CrackCalculatorUpdateCloningStrategyTests
    {
        private Mock<ICloningStrategy> _cloningStrategyMock;
        private Mock<IUpdateStrategy<IHasForceActions>> _forcesUpdateStrategyMock;
        private Mock<IUpdateStrategy<IHasPrimitives>> _primitivesUpdateStrategyMock;
        private CrackCalculatorUpdateCloningStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _cloningStrategyMock = new Mock<ICloningStrategy>();
            _forcesUpdateStrategyMock = new Mock<IUpdateStrategy<IHasForceActions>>();
            _primitivesUpdateStrategyMock = new Mock<IUpdateStrategy<IHasPrimitives>>();

            _strategy = new CrackCalculatorUpdateCloningStrategy(
                _cloningStrategyMock.Object,
                _forcesUpdateStrategyMock.Object,
                _primitivesUpdateStrategyMock.Object
            );
        }

        [Test]
        public void Update_WithNullSourceObject_ThrowsException()
        {
            // Arrange
            var targetObject = Mock.Of<ICrackCalculator>();

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => _strategy.Update(targetObject, null));
        }

        [Test]
        public void Update_WithNullTargetObject_ThrowsException()
        {
            // Arrange
            var sourceObject = Mock.Of<ICrackCalculator>();

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => _strategy.Update(null, sourceObject));
        }

        [Test]
        public void Update_WithSameSourceAndTarget_DoesNothing()
        {
            // Arrange
            var sourceObject = Mock.Of<ICrackCalculator>();

            // Act
            _strategy.Update(sourceObject, sourceObject);

            // Assert
            _forcesUpdateStrategyMock.VerifyNoOtherCalls();
            _primitivesUpdateStrategyMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Update_InvokesUpdateStrategiesOnInputData()
        {
            // Arrange
            var sourceDataMock = new Mock<ICrackCalculatorInputData>();
            var targetDataMock = new Mock<ICrackCalculatorInputData>();

            var sourceObjectMock = new Mock<ICrackCalculator>();
            sourceObjectMock.Setup(s => s.InputData).Returns(sourceDataMock.Object);

            var targetObjectMock = new Mock<ICrackCalculator>();
            targetObjectMock.Setup(t => t.InputData).Returns(targetDataMock.Object);

            // Act
            _strategy.Update(targetObjectMock.Object, sourceObjectMock.Object);

            // Assert
            _primitivesUpdateStrategyMock.Verify(ps => ps.Update(targetDataMock.Object, sourceDataMock.Object), Times.Once);
            _forcesUpdateStrategyMock.Verify(fs => fs.Update(targetDataMock.Object, sourceDataMock.Object), Times.Once);
        }
    }

}
