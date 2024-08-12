using NUnit.Framework;
using Moq;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperTests.UnitTests.Ndms.Cracks.InputDataTests
{

    [TestFixture]
    public class CrackWidthCalculationLogicTests
    {
        private Mock<IRebarStressCalculator> _calculator;
        private Mock<ICrackWidthLogic> mockCrackWidthLogic;
        private Mock<IShiftTraceLogger> mockTraceLogger;
        private Mock<IRebarCrackCalculatorInputData> mockInputData;
        private Mock<IUserCrackInputData> mockUserCrackInputData;
        private Mock<IRebarPrimitive> mockRebarPrimitive;
        private Mock<IRebarStressCalculator> mockRebarStressCalculator;
        private CrackWidthCalculationLogic crackWidthCalculationLogic;
        private Mock<IRebarCrackInputData> mockRebarCrackInputData;

        [SetUp]
        public void SetUp()
        {
            _calculator = new Mock<IRebarStressCalculator>();
            mockCrackWidthLogic = new Mock<ICrackWidthLogic>();
            mockTraceLogger = new Mock<IShiftTraceLogger>();
            mockInputData = new Mock<IRebarCrackCalculatorInputData>();
            mockUserCrackInputData = new Mock<IUserCrackInputData>();
            mockRebarPrimitive = new Mock<IRebarPrimitive>();
            mockRebarStressCalculator = new Mock<IRebarStressCalculator>();
            mockRebarCrackInputData = new Mock<IRebarCrackInputData>();

            mockInputData.Setup(x => x.UserCrackInputData).Returns(mockUserCrackInputData.Object);
            mockInputData.Setup(x => x.LongRebarData).Returns(mockRebarCrackInputData.Object);
            mockInputData.Setup(x => x.ShortRebarData).Returns(mockRebarCrackInputData.Object);
            mockInputData.Setup(x => x.RebarPrimitive).Returns(mockRebarPrimitive.Object);

            mockRebarCrackInputData.Setup(x => x.ForceTuple).Returns(new ForceTuple());

            crackWidthCalculationLogic = new CrackWidthCalculationLogic(_calculator.Object, mockCrackWidthLogic.Object, mockTraceLogger.Object)
            {
                InputData = mockInputData.Object
            };
        }

        [Test]
        public void Run_ShouldPrepareNewResultAndProcessCalculations()
        {
            // Arrange
            mockCrackWidthLogic.Setup(x => x.GetCrackWidth()).Returns(0.5);

            // Act
            crackWidthCalculationLogic.Run();

            // Assert
            mockCrackWidthLogic.Verify(x => x.GetCrackWidth(), Times.AtLeastOnce);
            Assert.IsNotNull(crackWidthCalculationLogic.Result);
            Assert.IsTrue(crackWidthCalculationLogic.Result.IsValid);
        }

        [Test]
        public void ProcessShortTermCalculations_ShouldCalculateCorrectShortTermCrackWidth()
        {
            // Arrange
            mockUserCrackInputData.Setup(x => x.UltimateShortCrackWidth).Returns(1.0);
            mockCrackWidthLogic.SetupSequence(x => x.GetCrackWidth())
                .Returns(0.6) // longTermLoadShortConcreteWidth
                .Returns(0.8); // fullLoadShortConcreteCrackWidth

            crackWidthCalculationLogic.InputData = mockInputData.Object;

            // Act
            var shortTermResult = crackWidthCalculationLogic.ProcessShortTermCalculations();

            // Assert
            Assert.AreEqual(1.2, shortTermResult.CrackWidth);
            Assert.AreEqual(1.0, shortTermResult.UltimateCrackWidth);
        }

        [Test]
        public void ProcessLongTermCalculations_ShouldCalculateCorrectLongTermCrackWidth()
        {
            // Arrange
            mockUserCrackInputData.Setup(x => x.UltimateLongCrackWidth).Returns(1.2);
            mockCrackWidthLogic.Setup(x => x.GetCrackWidth()).Returns(0.9);

            // Act
            var longTermResult = crackWidthCalculationLogic.ProcessLongTermCalculations();

            // Assert
            Assert.AreEqual(0.9, longTermResult.CrackWidth);
            Assert.AreEqual(1.2, longTermResult.UltimateCrackWidth);
        }
    }


}
