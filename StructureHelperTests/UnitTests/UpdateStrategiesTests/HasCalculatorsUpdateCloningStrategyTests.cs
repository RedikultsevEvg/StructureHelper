using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{


    [TestFixture]
    public class HasCalculatorsUpdateCloningStrategyTests
    {
        private Mock<ICalculatorCloningStrategyContainer> _cloningStrategyContainerMoq;
        private Mock<ICloningStrategy> _cloningStrategyMock;
        private Mock<IUpdateStrategy<IForceCalculator>> _forceCalculatorUpdateStrategyMock;
        private Mock<IUpdateStrategy<ICrackCalculator>> _crackCalculatorUpdateStrategyMock;
        private Mock<IUpdateStrategy<ILimitCurvesCalculator>> _limitCurvesCalculatorUpdateStrategyMock;
        private Mock<IUpdateStrategy<IValueDiagramCalculator>> _valueDiagramCalculatorUpdateStrategyMock;
        private HasCalculatorsUpdateCloningStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _cloningStrategyContainerMoq = new Mock<ICalculatorCloningStrategyContainer>();
            _cloningStrategyMock = new Mock<ICloningStrategy>();
            _forceCalculatorUpdateStrategyMock = new Mock<IUpdateStrategy<IForceCalculator>>();
            _crackCalculatorUpdateStrategyMock = new Mock<IUpdateStrategy<ICrackCalculator>>();
            _limitCurvesCalculatorUpdateStrategyMock = new Mock<IUpdateStrategy<ILimitCurvesCalculator>>();
            _valueDiagramCalculatorUpdateStrategyMock = new Mock<IUpdateStrategy<IValueDiagramCalculator>>();
            _cloningStrategyContainerMoq.Setup(m => m.ForceCalculatorStrategy).Returns(_forceCalculatorUpdateStrategyMock.Object);
            _cloningStrategyContainerMoq.Setup(m => m.CrackCalculatorStrategy).Returns(_crackCalculatorUpdateStrategyMock.Object);
            _cloningStrategyContainerMoq.Setup(m => m.LimitCurvesCalculatorStrategy).Returns(_limitCurvesCalculatorUpdateStrategyMock.Object);
            _cloningStrategyContainerMoq.Setup(m => m.ValueDiagramCalculatorStrategy).Returns(_valueDiagramCalculatorUpdateStrategyMock.Object);

            _strategy = new HasCalculatorsUpdateCloningStrategy(
                _cloningStrategyMock.Object,
                _cloningStrategyContainerMoq.Object);
        }

        [Test]
        public void Update_WithNullSourceObject_ThrowsException()
        {
            // Arrange
            var targetObject = Mock.Of<IHasCalculators>();

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => _strategy.Update(targetObject, null));
        }

        [Test]
        public void Update_WithNullTargetObject_ThrowsException()
        {
            // Arrange
            var sourceObject = Mock.Of<IHasCalculators>();

            // Act & Assert
            Assert.Throws<StructureHelperException>(() => _strategy.Update(null, sourceObject));
        }

        [Test]
        public void Update_WithSameSourceAndTarget_DoesNothing()
        {
            // Arrange
            var calculators = new List<ICalculator> { Mock.Of<IForceCalculator>() };
            var sourceObject = Mock.Of<IHasCalculators>(s => s.Calculators == calculators);
            var targetObject = sourceObject;

            // Act
            _strategy.Update(targetObject, sourceObject);

            // Assert
            _cloningStrategyMock.VerifyNoOtherCalls();
            _forceCalculatorUpdateStrategyMock.VerifyNoOtherCalls();
            _crackCalculatorUpdateStrategyMock.VerifyNoOtherCalls();
            _limitCurvesCalculatorUpdateStrategyMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Update_ProcessesEachCalculatorCorrectly()
        {
            // Arrange
            var sourceForceCalculator = Mock.Of<IForceCalculator>(x => x.Name == "Force");
            var sourceCrackCalculator = Mock.Of<ICrackCalculator>(x => x.Name == "Crack");
            var sourceLimitCurvesCalculator = Mock.Of<ILimitCurvesCalculator>(x => x.Name == "Curves");
            var sourceCalculators = new List<ICalculator>
        {
            sourceForceCalculator,
            sourceCrackCalculator,
            sourceLimitCurvesCalculator
        };
            var sourceObjectMock = new Mock<IHasCalculators>();
            sourceObjectMock.Setup(s => s.Calculators).Returns(sourceCalculators);

            var targetCalculators = new List<ICalculator>();
            var targetObjectMock = new Mock<IHasCalculators>();
            targetObjectMock.Setup(t => t.Calculators).Returns(targetCalculators);

            var clonedForceCalculator = Mock.Of<IForceCalculator>();
            var clonedCrackCalculator = Mock.Of<ICrackCalculator>();
            var clonedLimitCurvesCalculator = Mock.Of<ILimitCurvesCalculator>();

            _cloningStrategyMock.Setup(cs => cs.Clone(It.Is<ICalculator>(x => x.Name == "Force"), null)).Returns(clonedForceCalculator);
            _cloningStrategyMock.Setup(cs => cs.Clone(It.Is<ICalculator>(x => x.Name == "Crack"), null)).Returns(clonedCrackCalculator);
            _cloningStrategyMock.Setup(cs => cs.Clone(It.Is<ICalculator>(x => x.Name == "Curves"), null)).Returns(clonedLimitCurvesCalculator);

            // Act
            _strategy.Update(targetObjectMock.Object, sourceObjectMock.Object);

            // Assert
            Assert.That(targetCalculators, Has.Count.EqualTo(3));
            Assert.That(targetCalculators, Contains.Item(clonedForceCalculator));
            Assert.That(targetCalculators, Contains.Item(clonedCrackCalculator));
            Assert.That(targetCalculators, Contains.Item(clonedLimitCurvesCalculator));

            _forceCalculatorUpdateStrategyMock.Verify(f => f.Update(clonedForceCalculator, sourceForceCalculator), Times.Once);
            _crackCalculatorUpdateStrategyMock.Verify(c => c.Update(clonedCrackCalculator, sourceCrackCalculator), Times.Once);
            _limitCurvesCalculatorUpdateStrategyMock.Verify(l => l.Update(clonedLimitCurvesCalculator, sourceLimitCurvesCalculator), Times.Once);
        }

        [Test]
        public void Update_WithUnknownCalculatorType_ThrowsException()
        {
            // Arrange
            var unknownCalculator = Mock.Of<ICalculator>();
            var sourceCalculators = new List<ICalculator> { unknownCalculator };
            var sourceObjectMock = new Mock<IHasCalculators>();
            sourceObjectMock.Setup(s => s.Calculators).Returns(sourceCalculators);

            var targetObjectMock = new Mock<IHasCalculators>();
            targetObjectMock.Setup(t => t.Calculators).Returns(new List<ICalculator>());

            // Act & Assert
            var exception = Assert.Throws<StructureHelperException>(() => _strategy.Update(targetObjectMock.Object, sourceObjectMock.Object));
            Assert.That(exception.Message, Does.Contain(ErrorStrings.ObjectTypeIsUnknown));
        }
    }

}
