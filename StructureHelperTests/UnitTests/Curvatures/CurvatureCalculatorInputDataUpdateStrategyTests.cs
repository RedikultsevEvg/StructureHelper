using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Primitives;


namespace StructureHelperTests.UnitTests.Curvatures
{

    [TestFixture]
    public class CurvatureCalculatorInputDataUpdateStrategyTests
    {
        private Mock<IUpdateStrategy<IDeflectionFactor>> _deflectionUpdateMock;
        private CurvatureCalculatorInputDataUpdateStrategy _strategy;

        private Mock<ICurvatureCalculatorInputData> _targetMock;
        private Mock<ICurvatureCalculatorInputData> _sourceMock;

        private List<INdmPrimitive> _targetPrimitives;
        private List<INdmPrimitive> _sourcePrimitives;

        private List<IForceAction> _targetForces;
        private List<IForceAction> _sourceForces;

        private Mock<IDeflectionFactor> _targetDeflectionFactorMock;
        private Mock<IDeflectionFactor> _sourceDeflectionFactorMock;

        [SetUp]
        public void Setup()
        {
            _deflectionUpdateMock = new Mock<IUpdateStrategy<IDeflectionFactor>>();

            _strategy = new CurvatureCalculatorInputDataUpdateStrategy(_deflectionUpdateMock.Object);

            // Mock target and source ICurvatureCalculatorInputData
            _targetMock = new Mock<ICurvatureCalculatorInputData>();
            _sourceMock = new Mock<ICurvatureCalculatorInputData>();

            _targetPrimitives = new List<INdmPrimitive> { new Mock<INdmPrimitive>().Object };
            _sourcePrimitives = new List<INdmPrimitive> { new Mock<INdmPrimitive>().Object, new Mock<INdmPrimitive>().Object };

            _targetForces = new List<IForceAction> { new Mock<IForceAction>().Object };
            _sourceForces = new List<IForceAction> { new Mock<IForceAction>().Object, new Mock<IForceAction>().Object };

            _targetDeflectionFactorMock = new Mock<IDeflectionFactor>();
            _sourceDeflectionFactorMock = new Mock<IDeflectionFactor>();

            _targetMock.SetupGet(x => x.Primitives).Returns(_targetPrimitives);
            _targetMock.SetupGet(x => x.ForceActions).Returns(_targetForces);
            _targetMock.SetupGet(x => x.DeflectionFactor).Returns(_targetDeflectionFactorMock.Object);

            _sourceMock.SetupGet(x => x.Primitives).Returns(_sourcePrimitives);
            _sourceMock.SetupGet(x => x.ForceActions).Returns(_sourceForces);
            _sourceMock.SetupGet(x => x.DeflectionFactor).Returns(_sourceDeflectionFactorMock.Object);
        }

        // ---------------------------------------------------------
        // TEST 1 — Basic update copies collections and calls DeflectionFactor update
        // ---------------------------------------------------------
        [Test]
        public void Update_ShouldCopyCollections_AndCallDeflectionUpdate()
        {
            // Act
            _strategy.Update(_targetMock.Object, _sourceMock.Object);

            // Assert — target collections replaced by source
            Assert.That(_targetPrimitives, Is.EqualTo(_sourcePrimitives));
            Assert.That(_targetForces, Is.EqualTo(_sourceForces));

            // DeflectionFactor update called
            _deflectionUpdateMock.Verify(x =>
                x.Update(_targetDeflectionFactorMock.Object, _sourceDeflectionFactorMock.Object), Times.Once);
        }

        // ---------------------------------------------------------
        // TEST 2 — Should throw if target is null
        // ---------------------------------------------------------
        [Test]
        public void Update_ShouldThrow_WhenTargetIsNull()
        {
            Assert.Throws<StructureHelperException>(() =>
                _strategy.Update(null!, _sourceMock.Object));
        }

        // ---------------------------------------------------------
        // TEST 3 — Should throw if source is null
        // ---------------------------------------------------------
        [Test]
        public void Update_ShouldThrow_WhenSourceIsNull()
        {
            Assert.Throws<StructureHelperException>(() =>
                _strategy.Update(_targetMock.Object, null!));
        }

        // ---------------------------------------------------------
        // TEST 4 — Should not update if source and target are the same
        // ---------------------------------------------------------
        [Test]
        public void Update_ShouldNotCallDeflectionUpdate_WhenSourceAndTargetSame()
        {
            _strategy.Update(_targetMock.Object, _targetMock.Object);

            _deflectionUpdateMock.Verify(x =>
                x.Update(It.IsAny<IDeflectionFactor>(), It.IsAny<IDeflectionFactor>()), Times.Never);
        }

        // ---------------------------------------------------------
        // TEST 5 — Should skip children if UpdateChildren = false
        // ---------------------------------------------------------
        [Test]
        public void Update_ShouldSkipChildren_WhenUpdateChildrenFalse()
        {
            _strategy.UpdateChildren = false;

            _strategy.Update(_targetMock.Object, _sourceMock.Object);

            _deflectionUpdateMock.Verify(x =>
                x.Update(It.IsAny<IDeflectionFactor>(), It.IsAny<IDeflectionFactor>()), Times.Never);

            // Collections unchanged
            Assert.That(_targetPrimitives, Is.Not.EqualTo(_sourcePrimitives));
            Assert.That(_targetForces, Is.Not.EqualTo(_sourceForces));
        }
    }

}
