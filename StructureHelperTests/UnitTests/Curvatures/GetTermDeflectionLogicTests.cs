using Moq;
using NUnit.Framework;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;

namespace StructureHelperTests.UnitTests.Curvatures
{

    [TestFixture]
    public class GetTermDeflectionLogicTests
    {
        private Mock<IGetDeflectionByCurvatureLogic> _deflectionLogicMock;
        private Mock<IForceTupleServiceLogic> _forceTupleServiceMock;
        private Mock<IShiftTraceLogger> _loggerMock;
        private Mock<IDeflectionFactor> _deflectionFactorMock;

        private Mock<IForceTuple> _longCurvatureMock;
        private Mock<IForceTuple> _shortLongCurvatureMock;
        private Mock<IForceTuple> _shortFullCurvatureMock;
        private Mock<ICurvatureTermResult> _expectedResultMock;

        [SetUp]
        public void Setup()
        {
            _deflectionLogicMock = new Mock<IGetDeflectionByCurvatureLogic>();
            _forceTupleServiceMock = new Mock<IForceTupleServiceLogic>();
            _loggerMock = new Mock<IShiftTraceLogger>();
            _deflectionFactorMock = new Mock<IDeflectionFactor>();

            _longCurvatureMock = new Mock<IForceTuple>();
            _shortLongCurvatureMock = new Mock<IForceTuple>();
            _shortFullCurvatureMock = new Mock<IForceTuple>();
            _expectedResultMock = new Mock<ICurvatureTermResult>();
        }

        // ---------------------------------------------------------
        // TEST 1 — GetLongResult should call deflection logic
        // ---------------------------------------------------------
        [Test]
        public void GetLongResult_ShouldCallDeflectionLogic_AndReturnResult()
        {
            // Arrange
            var logic = new GetTermDeflectionLogic(
                _deflectionLogicMock.Object,
                _forceTupleServiceMock.Object,
                _loggerMock.Object)
            {
                LongCurvature = _longCurvatureMock.Object,
                DeflectionFactor = _deflectionFactorMock.Object
            };

            _deflectionLogicMock
                .Setup(x => x.GetDeflection(_longCurvatureMock.Object, _deflectionFactorMock.Object))
                .Returns(_expectedResultMock.Object);

            // Act
            var result = logic.GetLongResult();

            // Assert
            Assert.That(result, Is.EqualTo(_expectedResultMock.Object));
            _deflectionLogicMock.Verify(x => x.GetDeflection(_longCurvatureMock.Object, _deflectionFactorMock.Object),
                Times.Once);

            _loggerMock.Verify(x => x.AddMessage(It.IsAny<string>()), Times.Once);
        }

        // ---------------------------------------------------------
        // TEST 2 — GetShortResult should compute curvature and call deflection logic
        // ---------------------------------------------------------
        [Test]
        public void GetShortResult_ShouldComputeShortCurvature_AndReturnDeflection()
        {
            // Arrange
            var deltaCurvatureMock = new Mock<IForceTuple>().Object;
            var shortCurvatureMock = new Mock<IForceTuple>().Object;

            var logic = new GetTermDeflectionLogic(
                _deflectionLogicMock.Object,
                _forceTupleServiceMock.Object,
                _loggerMock.Object)
            {
                LongCurvature = _longCurvatureMock.Object,
                ShortFullCurvature = _shortFullCurvatureMock.Object,
                ShortLongCurvature = _shortLongCurvatureMock.Object,
                DeflectionFactor = _deflectionFactorMock.Object
            };

            _forceTupleServiceMock
                .Setup(x => x.SumTuples(_shortFullCurvatureMock.Object, _shortLongCurvatureMock.Object, -1))
                .Returns(deltaCurvatureMock);

            _forceTupleServiceMock
                .Setup(x => x.SumTuples(_longCurvatureMock.Object, deltaCurvatureMock))
                .Returns(shortCurvatureMock);

            _deflectionLogicMock
                .Setup(x => x.GetDeflection(shortCurvatureMock, _deflectionFactorMock.Object))
                .Returns(_expectedResultMock.Object);

            // Act
            var result = logic.GetShortResult();

            // Assert
            Assert.That(result, Is.EqualTo(_expectedResultMock.Object));

            _forceTupleServiceMock.Verify(x =>
                x.SumTuples(_shortFullCurvatureMock.Object, _shortLongCurvatureMock.Object, -1), Times.Once);

            _forceTupleServiceMock.Verify(x =>
                x.SumTuples(_longCurvatureMock.Object, deltaCurvatureMock), Times.Once);

            _deflectionLogicMock.Verify(x =>
                x.GetDeflection(shortCurvatureMock, _deflectionFactorMock.Object), Times.Once);

            // Trace should run once for the final curvature
            _loggerMock.Verify(x => x.AddMessage(It.IsAny<string>()), Times.Once);
        }
    }


}
