using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperTests.UnitTests.Ndms.Cracks
{


    [TestFixture]
    public class RebarStressResultLogicTests
    {
        private Mock<IRebarStressCalculator> _mockRebarStressCalculator;
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private RebarStressResultLogic _rebarStressResultLogic;

        [SetUp]
        public void Setup()
        {
            _mockRebarStressCalculator = new Mock<IRebarStressCalculator>();
            _mockTraceLogger = new Mock<IShiftTraceLogger>();
            _rebarStressResultLogic = new RebarStressResultLogic(_mockRebarStressCalculator.Object, _mockTraceLogger.Object);
        }

        [Test]
        public void GetRebarStressResult_WhenCalculationIsValid_ReturnsRebarStressResult()
        {
            // Arrange
            var mockRebarStressResult = new RebarStressResult
            {
                IsValid = true
            };

            _mockRebarStressCalculator.Setup(x => x.Run());
            _mockRebarStressCalculator.Setup(x => x.Result).Returns(mockRebarStressResult);

            var mockRebarPrimitive = new Mock<IRebarPrimitive>();
            var mockRebarCrackInputData = new Mock<IRebarCrackInputData>();

            _rebarStressResultLogic.RebarPrimitive = mockRebarPrimitive.Object;
            _rebarStressResultLogic.RebarCrackInputData = mockRebarCrackInputData.Object;

            // Act
            var result = _rebarStressResultLogic.GetRebarStressResult();

            // Assert
            Assert.AreEqual(mockRebarStressResult, result);
            _mockRebarStressCalculator.Verify(x => x.Run(), Times.Once);
        }

        [Test]
        public void GetRebarStressResult_WhenCalculationIsInvalid_ThrowsStructureHelperException()
        {
            // Arrange
            var mockRebarStressResult = new RebarStressResult
            {
                IsValid = false,
                Description = "Error in calculation"
            };

            _mockRebarStressCalculator.Setup(x => x.Run());
            _mockRebarStressCalculator.Setup(x => x.Result).Returns(mockRebarStressResult);

            var mockRebarPrimitive = new Mock<IRebarPrimitive>();
            var mockRebarCrackInputData = new Mock<IRebarCrackInputData>();

            _rebarStressResultLogic.RebarPrimitive = mockRebarPrimitive.Object;
            _rebarStressResultLogic.RebarCrackInputData = mockRebarCrackInputData.Object;

            // Act & Assert
            var ex = Assert.Throws<StructureHelperException>(() => _rebarStressResultLogic.GetRebarStressResult());
            Assert.That(ex.Message, Is.EqualTo(LoggerStrings.CalculationError + mockRebarStressResult.Description));
            _mockTraceLogger.Verify(x => x.AddMessage(It.IsAny<string>(), TraceLogStatuses.Error), Times.Once);
        }
    }

}
