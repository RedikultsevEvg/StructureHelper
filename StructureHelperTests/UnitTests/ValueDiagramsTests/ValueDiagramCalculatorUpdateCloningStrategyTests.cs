using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperTests.UnitTests.ValueDiagramsTests
{
    using Moq;
    using NUnit.Framework;
    using StructureHelperCommon.Infrastructures.Exceptions;

    [TestFixture]
    public class ValueDiagramCalculatorUpdateCloningStrategyTests
    {
        private Mock<ICloningStrategy> _cloningStrategyMock;
        private Mock<IUpdateStrategy<IHasForceActions>> _forcesUpdateMock;
        private Mock<IUpdateStrategy<IHasPrimitives>> _primitivesUpdateMock;
        private ValueDiagramCalculatorUpdateCloningStrategy _strategy;

        private Mock<IValueDiagramCalculator> _sourceMock;
        private Mock<IValueDiagramCalculator> _targetMock;
        private Mock<IValueDiagramCalculatorInputData> _inputDataMock;

        [SetUp]
        public void Setup()
        {
            _cloningStrategyMock = new Mock<ICloningStrategy>();
            _forcesUpdateMock = new Mock<IUpdateStrategy<IHasForceActions>>();
            _primitivesUpdateMock = new Mock<IUpdateStrategy<IHasPrimitives>>();

            _strategy = new ValueDiagramCalculatorUpdateCloningStrategy(
                _cloningStrategyMock.Object,
                _forcesUpdateMock.Object,
                _primitivesUpdateMock.Object
            );

            _sourceMock = new Mock<IValueDiagramCalculator>();
            _targetMock = new Mock<IValueDiagramCalculator>();
            _inputDataMock = new Mock<IValueDiagramCalculatorInputData>();

            // Both source and target share the same interface object type
            _sourceMock.Setup(s => s.InputData).Returns(_inputDataMock.Object);
            _targetMock.Setup(t => t.InputData).Returns(_inputDataMock.Object);
        }

        [Test]
        public void Update_SourceObjectIsNull_Throws()
        {
            Assert.That(
                () => _strategy.Update(_targetMock.Object, null),
                Throws.Exception.TypeOf<StructureHelperException>()
            );
        }

        [Test]
        public void Update_TargetObjectIsNull_Throws()
        {
            Assert.That(
                () => _strategy.Update(null, _sourceMock.Object),
                Throws.Exception.TypeOf<StructureHelperException>()
            );
        }

        [Test]
        public void Update_TargetAndSourceAreSame_DoesNothing()
        {
            _strategy.Update(_targetMock.Object, _targetMock.Object);

            _primitivesUpdateMock.Verify(p => p.Update(It.IsAny<IHasPrimitives>(), It.IsAny<IHasPrimitives>()), Times.Never);
            _forcesUpdateMock.Verify(f => f.Update(It.IsAny<IHasForceActions>(), It.IsAny<IHasForceActions>()), Times.Never);
        }

        [Test]
        public void Update_CallsPrimitivesAndForcesStrategies()
        {
            // Act
            _strategy.Update(_targetMock.Object, _sourceMock.Object);

            // Assert: Both strategies are called with the same InputData object
            _primitivesUpdateMock.Verify(p => p.Update(_inputDataMock.Object, _inputDataMock.Object), Times.Once);
            _forcesUpdateMock.Verify(f => f.Update(_inputDataMock.Object, _inputDataMock.Object), Times.Once);
        }

    }
}
