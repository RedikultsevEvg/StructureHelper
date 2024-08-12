using NUnit.Framework;
using Moq;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperTests.UnitTests.Ndms.Cracks
{


    [TestFixture]
    public class CrackWidthCalculationLogicTests
    {
        private Mock<IRebarStressCalculator> _calculator;
        private Mock<ICrackWidthLogic> _mockCrackWidthLogic;
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private CrackWidthCalculationLogic _logic;
        private IRebarCrackCalculatorInputData _inputData;

        [SetUp]
        public void SetUp()
        {
            _calculator = new Mock<IRebarStressCalculator>();
            _mockCrackWidthLogic = new Mock<ICrackWidthLogic>();
            _mockTraceLogger = new Mock<IShiftTraceLogger>();
            _inputData = new RebarCrackCalculatorInputData
            {
                
            };

            _inputData.UserCrackInputData = new UserCrackInputData();

            _logic = new CrackWidthCalculationLogic(_calculator.Object, _mockCrackWidthLogic.Object, _mockTraceLogger.Object)
            {
                InputData = _inputData
            };
        }

        [Test]
        public void Run_ShouldPrepareNewResultAndProcessCalculations()
        {
            // Arrange
            _mockCrackWidthLogic.Setup(m => m.GetCrackWidth())
                .Returns(0.5); // Mock the GetCrackWidth method

            // Act
            _logic.Run();

            // Assert
            Assert.NotNull(_logic.Result);
            Assert.IsTrue(_logic.Result.IsValid);
            _mockTraceLogger.Verify(x => x.AddMessage(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void ProcessLongTermCalculation_ShouldReturnExpectedResult()
        {
            // Arrange
            _mockCrackWidthLogic.SetupSequence(m => m.GetCrackWidth())
                .Returns(0.5) // longTermLoadLongTermConcreteCrackWidth
                .Returns(0.2); // longTermLoadShortConcreteWidth

            // Act
            var result = _logic.ProcessLongTermCalculations();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(0.5, result.CrackWidth);
            _mockTraceLogger.Verify(x => x.AddMessage(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void ProcessShortTermCalculations_ShouldReturnExpectedResult()
        {
            // Arrange
            _mockCrackWidthLogic.SetupSequence(m => m.GetCrackWidth())
                .Returns(0.2) // longTermLoadShortConcreteWidth
                .Returns(0.3); // fullLoadShortConcreteCrackWidth

            // Act
            var result = _logic.ProcessShortTermCalculations();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(0.0, result.CrackWidth); // Assuming acrcShort would be 0 in this scenario
            _mockTraceLogger.Verify(x => x.AddMessage(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void GetRebarStressResult_ShouldReturnValidResult()
        {
            // Arrange
            var mockRebarStressCalculator = new Mock<IRebarStressCalculator>();
            var expectedResult = new RebarStressResult { IsValid = true };
            mockRebarStressCalculator.Setup(c => c.Run());
            mockRebarStressCalculator.Setup(c => c.Result).Returns(expectedResult);

            // Act
            var result = _logic.GetRebarStressResult(_inputData.ShortRebarData);

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.IsValid);
            _mockTraceLogger.Verify(x => x.AddMessage(It.IsAny<string>(), TraceLogStatuses.Error), Times.Never);
        }
    }

}
