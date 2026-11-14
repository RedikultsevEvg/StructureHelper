using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{
    [TestFixture]
    public class CrackCalculatorUpdateCloningStrategyTests
    {
        private Mock<ICloningStrategy> _cloningStrategyMock;
        private Mock<ICollectionCloneStrategy<IForceAction>> _forcesCloneStrategyMock;
        private Mock<ICollectionCloneStrategy<INdmPrimitive>> _primitivesCloneStrategyMock;
        private CrackCalculatorUpdateCloningStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _cloningStrategyMock = new Mock<ICloningStrategy>();
            _forcesCloneStrategyMock = new Mock<ICollectionCloneStrategy<IForceAction>>();
            _primitivesCloneStrategyMock = new Mock<ICollectionCloneStrategy<INdmPrimitive>>();

            _strategy = new CrackCalculatorUpdateCloningStrategy(
                _cloningStrategyMock.Object,
                _forcesCloneStrategyMock.Object,
                _primitivesCloneStrategyMock.Object);
        }

        [Test]
        public void Update_WithNullSourceObject_ThrowsException()
        {
            Assert.Throws<StructureHelperException>(() => _strategy.Update(Mock.Of<ICrackCalculator>(), null));
        }

        [Test]
        public void Update_WithNullTargetObject_ThrowsException()
        {
            Assert.Throws<StructureHelperException>(() => _strategy.Update(null, Mock.Of<ICrackCalculator>()));
        }

        [Test]
        public void Update_WithSameSourceAndTarget_DoesNothing()
        {
            var sourceObject = Mock.Of<ICrackCalculator>();
            _strategy.Update(sourceObject, sourceObject);

            _forcesCloneStrategyMock.VerifyNoOtherCalls();
            _primitivesCloneStrategyMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Update_InvokesCollectionCloneStrategiesOnInputData()
        {
            var sourceActions = new List<IForceAction>();
            var targetActions = new List<IForceAction>();
            var sourcePrimitives = new List<INdmPrimitive>();
            var targetPrimitives = new List<INdmPrimitive>();

            var sourceDataMock = new Mock<ICrackCalculatorInputData>();
            sourceDataMock.SetupGet(x => x.ForceActions).Returns(sourceActions);
            sourceDataMock.SetupGet(x => x.Primitives).Returns(sourcePrimitives);
            var targetDataMock = new Mock<ICrackCalculatorInputData>();
            targetDataMock.SetupGet(x => x.ForceActions).Returns(targetActions);
            targetDataMock.SetupGet(x => x.Primitives).Returns(targetPrimitives);

            var sourceObjectMock = new Mock<ICrackCalculator>();
            sourceObjectMock.SetupGet(x => x.InputData).Returns(sourceDataMock.Object);
            var targetObjectMock = new Mock<ICrackCalculator>();
            targetObjectMock.SetupGet(x => x.InputData).Returns(targetDataMock.Object);

            _strategy.Update(targetObjectMock.Object, sourceObjectMock.Object);

            _primitivesCloneStrategyMock.Verify(
                x => x.GetClone(targetPrimitives, sourcePrimitives, CloneMode.NewEntity),
                Times.Once);
            _forcesCloneStrategyMock.Verify(
                x => x.GetClone(targetActions, sourceActions, CloneMode.NewEntity),
                Times.Once);
        }
    }
}
