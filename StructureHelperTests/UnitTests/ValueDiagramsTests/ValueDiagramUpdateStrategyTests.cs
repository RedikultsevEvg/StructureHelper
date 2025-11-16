using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace StructureHelperTests.UnitTests.ValueDiagramsTests
{


    [TestFixture]
    public class ValueDiagramUpdateStrategyTests
    {
        private Mock<IUpdateStrategy<IPoint2DRange>> _rangeUpdateMock;
        private ValueDiagramUpdateStrategy _strategy;
        private Mock<IValueDiagram> _targetMock;
        private Mock<IValueDiagram> _sourceMock;
        private Mock<IPoint2DRange> _targetRangeMock;
        private Mock<IPoint2DRange> _sourceRangeMock;

        [SetUp]
        public void Setup()
        {
            _rangeUpdateMock = new Mock<IUpdateStrategy<IPoint2DRange>>();
            _strategy = new ValueDiagramUpdateStrategy(_rangeUpdateMock.Object);

            _targetMock = new Mock<IValueDiagram>();
            _sourceMock = new Mock<IValueDiagram>();
            _targetRangeMock = new Mock<IPoint2DRange>();
            _sourceRangeMock = new Mock<IPoint2DRange>();

            _targetMock.Setup(t => t.Point2DRange).Returns(_targetRangeMock.Object);
            _sourceMock.Setup(s => s.Point2DRange).Returns(_sourceRangeMock.Object);
        }

        [Test]
        public void Update_TargetIsNull_Throws()
        {
            Assert.That(
                () => _strategy.Update(null, _sourceMock.Object),
                Throws.Exception.TypeOf<StructureHelperException>()
            );
        }

        [Test]
        public void Update_SourceIsNull_Throws()
        {
            Assert.That(
                () => _strategy.Update(_targetMock.Object, null),
                Throws.Exception.TypeOf<StructureHelperException>()
            );
        }

        [Test]
        public void Update_TargetEqualsSource_DoesNothing()
        {
            _strategy.Update(_targetMock.Object, _targetMock.Object);

            _rangeUpdateMock.Verify(r => r.Update(It.IsAny<IPoint2DRange>(), It.IsAny<IPoint2DRange>()), Times.Never);
        }

        [Test]
        public void Update_CopiesStepNumber_AndCallsRangeUpdate()
        {
            _sourceMock.Setup(s => s.StepNumber).Returns(5);

            _strategy.Update(_targetMock.Object, _sourceMock.Object);

            _targetMock.VerifySet(t => t.StepNumber = 5, Times.Once);
            _rangeUpdateMock.Verify(r => r.Update(_targetRangeMock.Object, _sourceRangeMock.Object), Times.Once);
        }

        [Test]
        public void Update_LazyInitializesRangeUpdateStrategy_WhenNotProvided()
        {
            // Use parameterless constructor
            var strategy = new ValueDiagramUpdateStrategy();
            var targetMock = new Mock<IValueDiagram>();
            var sourceMock = new Mock<IValueDiagram>();
            var targetRangeMock = new Mock<IPoint2DRange>();
            var sourceRangeMock = new Mock<IPoint2DRange>();
            var pointMoq = new Mock<IPoint2D>();

            targetMock.Setup(t => t.Point2DRange).Returns(targetRangeMock.Object);
            sourceMock.Setup(s => s.Point2DRange).Returns(sourceRangeMock.Object);
            sourceRangeMock.Setup(s => s.StartPoint).Returns(pointMoq.Object);
            sourceRangeMock.Setup(s => s.EndPoint).Returns(pointMoq.Object);
            targetRangeMock.Setup(s => s.StartPoint).Returns(pointMoq.Object);
            targetRangeMock.Setup(s => s.EndPoint).Returns(pointMoq.Object);

            // This should create Point2DRangeUpdateStrategy lazily
            Assert.DoesNotThrow(() => strategy.Update(targetMock.Object, sourceMock.Object));

            // No exception means lazy initialization worked
            // Cannot directly mock the default Point2DRangeUpdateStrategy, but at least this ensures no null reference
        }

        [Test]
        public void Update_DoesNotCallRangeUpdate_WhenUpdateChildrenIsFalse()
        {
            _strategy.UpdateChildren = false;

            _strategy.Update(_targetMock.Object, _sourceMock.Object);

            _rangeUpdateMock.Verify(r => r.Update(It.IsAny<IPoint2DRange>(), It.IsAny<IPoint2DRange>()), Times.Never);
        }
    }

}
