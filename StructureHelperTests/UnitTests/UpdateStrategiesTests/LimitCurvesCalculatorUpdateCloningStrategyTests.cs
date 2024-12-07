using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{



    [TestFixture]
    public class LimitCurvesCalculatorUpdateCloningStrategyTests
    {
        private Mock<ICloningStrategy> _cloningStrategyMock;
        private Mock<IUpdateStrategy<ILimitCurvesCalculatorInputData>> _inputDataUpdateStrategyMock;
        private LimitCurvesCalculatorUpdateCloningStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _cloningStrategyMock = new Mock<ICloningStrategy>();
            _inputDataUpdateStrategyMock = new Mock<IUpdateStrategy<ILimitCurvesCalculatorInputData>>();
            _strategy = new LimitCurvesCalculatorUpdateCloningStrategy(
                _cloningStrategyMock.Object,
                _inputDataUpdateStrategyMock.Object);
        }

        [Test]
        public void Update_WithNullSourceObject_ThrowsException()
        {
            // Arrange
            var targetObject = Mock.Of<ILimitCurvesCalculator>();

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => _strategy.Update(targetObject, null));
        }

        [Test]
        public void Update_WithNullTargetObject_ThrowsException()
        {
            // Arrange
            var sourceObject = Mock.Of<ILimitCurvesCalculator>();

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => _strategy.Update(null, sourceObject));
        }

        [Test]
        public void Update_WithSameSourceAndTarget_DoesNothing()
        {
            // Arrange
            var sourceObject = Mock.Of<ILimitCurvesCalculator>();

            // Act
            _strategy.Update(sourceObject, sourceObject);

            // Assert
            _cloningStrategyMock.VerifyNoOtherCalls();
            _inputDataUpdateStrategyMock.VerifyNoOtherCalls();
        }

        //[Test]
        public void Update_UpdatesInputDataAndProcessesEachPrimitiveSeries()
        {
            // Arrange
            var targetSeries = new NamedCollection<INdmPrimitive> { Collection = new List<INdmPrimitive>() };
            var targetData = Mock.Of<ILimitCurvesCalculatorInputData>(d => d.PrimitiveSeries == new List<NamedCollection<INdmPrimitive>> { targetSeries });

            var targetObjectMock = new Mock<ILimitCurvesCalculator>();
            targetObjectMock.Setup(t => t.InputData).Returns(targetData);

            var sourcePrimitive1 = Mock.Of<INdmPrimitive>(p => p.Name == "sp1");
            var sourcePrimitive2 = Mock.Of<INdmPrimitive>(p => p.Name == "sp2");
            var sourceSeries = new NamedCollection<INdmPrimitive>
            {
                Collection = new List<INdmPrimitive> { sourcePrimitive1, sourcePrimitive2 }
            };
            var sourceData = Mock.Of<ILimitCurvesCalculatorInputData>(d => d.PrimitiveSeries == new List<NamedCollection<INdmPrimitive>> { sourceSeries });

            var sourceObjectMock = new Mock<ILimitCurvesCalculator>();
            sourceObjectMock.Setup(s => s.InputData).Returns(sourceData);

            var clonedPrimitive1 = Mock.Of<INdmPrimitive>(p => p.Name == "cp1");
            var clonedPrimitive2 = Mock.Of<INdmPrimitive>(p => p.Name == "cp2");

            _cloningStrategyMock.Setup(cs => cs.Clone(It.Is<INdmPrimitive>(p => p.Name == "sp1"), null)).Returns(clonedPrimitive1);
            _cloningStrategyMock.Setup(cs => cs.Clone(It.Is<INdmPrimitive>(p => p.Name == "sp2"), null)).Returns(clonedPrimitive2);

            // Act
            _strategy.Update(targetObjectMock.Object, sourceObjectMock.Object);

            // Assert
            _inputDataUpdateStrategyMock.Verify(
                us => us.Update(targetData, sourceData),
                Times.Once);

            Assert.That(targetSeries.Collection, Has.Count.EqualTo(2));
            Assert.That(targetSeries.Collection, Contains.Item(clonedPrimitive1));
            Assert.That(targetSeries.Collection, Contains.Item(clonedPrimitive2));

            _cloningStrategyMock.Verify(cs => cs.Clone(sourcePrimitive1, null), Times.Once);
            _cloningStrategyMock.Verify(cs => cs.Clone(sourcePrimitive2, null), Times.Once);
        }
    }


}
