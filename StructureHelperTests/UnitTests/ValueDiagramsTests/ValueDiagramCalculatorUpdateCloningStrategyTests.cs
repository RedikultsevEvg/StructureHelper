using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.ValueDiagramsTests
{
    [TestFixture]
    public class ValueDiagramCalculatorUpdateCloningStrategyTests
    {
        private Mock<ICloningStrategy> _cloningStrategyMock;
        private Mock<ICollectionCloneStrategy<IForceAction>> _forcesCloneMock;
        private Mock<ICollectionCloneStrategy<INdmPrimitive>> _primitivesCloneMock;
        private ValueDiagramCalculatorUpdateCloningStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _cloningStrategyMock = new Mock<ICloningStrategy>();
            _forcesCloneMock = new Mock<ICollectionCloneStrategy<IForceAction>>();
            _primitivesCloneMock = new Mock<ICollectionCloneStrategy<INdmPrimitive>>();

            _strategy = new ValueDiagramCalculatorUpdateCloningStrategy(
                _cloningStrategyMock.Object,
                _forcesCloneMock.Object,
                _primitivesCloneMock.Object);
        }

        [Test]
        public void Update_SourceObjectIsNull_Throws()
        {
            Assert.That(
                () => _strategy.Update(Mock.Of<IValueDiagramCalculator>(), null),
                Throws.Exception.TypeOf<StructureHelperException>());
        }

        [Test]
        public void Update_TargetObjectIsNull_Throws()
        {
            Assert.That(
                () => _strategy.Update(null, Mock.Of<IValueDiagramCalculator>()),
                Throws.Exception.TypeOf<StructureHelperException>());
        }

        [Test]
        public void Update_TargetAndSourceAreSame_DoesNothing()
        {
            var sourceObject = Mock.Of<IValueDiagramCalculator>();
            _strategy.Update(sourceObject, sourceObject);

            _primitivesCloneMock.VerifyNoOtherCalls();
            _forcesCloneMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Update_CallsPrimitivesAndForcesStrategies()
        {
            var sourceActions = new List<IForceAction>();
            var targetActions = new List<IForceAction>();
            var sourcePrimitives = new List<INdmPrimitive>();
            var targetPrimitives = new List<INdmPrimitive>();

            var sourceDataMock = new Mock<IValueDiagramCalculatorInputData>();
            sourceDataMock.SetupGet(x => x.ForceActions).Returns(sourceActions);
            sourceDataMock.SetupGet(x => x.Primitives).Returns(sourcePrimitives);
            var targetDataMock = new Mock<IValueDiagramCalculatorInputData>();
            targetDataMock.SetupGet(x => x.ForceActions).Returns(targetActions);
            targetDataMock.SetupGet(x => x.Primitives).Returns(targetPrimitives);

            var sourceObjectMock = new Mock<IValueDiagramCalculator>();
            sourceObjectMock.SetupGet(x => x.InputData).Returns(sourceDataMock.Object);
            var targetObjectMock = new Mock<IValueDiagramCalculator>();
            targetObjectMock.SetupGet(x => x.InputData).Returns(targetDataMock.Object);

            _strategy.Update(targetObjectMock.Object, sourceObjectMock.Object);

            _primitivesCloneMock.Verify(
                x => x.GetClone(targetPrimitives, sourcePrimitives, CloningMode.NewEntity),
                Times.Once);
            _forcesCloneMock.Verify(
                x => x.GetClone(targetActions, sourceActions, CloningMode.NewEntity),
                Times.Once);
        }
    }
}
