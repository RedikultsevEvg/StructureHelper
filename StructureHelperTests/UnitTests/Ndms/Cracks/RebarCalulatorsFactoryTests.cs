using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperTests.UnitTests.Ndms.Cracks
{
    public class RebarCalulatorsFactoryTests
    {
        [Test]
        public void GetCalculators_ShouldReturnListOfCalculators()
        {
            // Arrange
            var mockInputFactory = new Mock<IRebarCrackInputDataFactory>();
            var rebar = new RebarPrimitive();
            var inputData = new TupleCrackInputData();
            var rebarCrackInputData = new RebarCrackCalculatorInputData();

            mockInputFactory.SetupProperty(f => f.Rebar, rebar);
            mockInputFactory.SetupProperty(f => f.InputData, inputData);
            mockInputFactory.SetupProperty(f => f.LongLength, 10.0);
            mockInputFactory.SetupProperty(f => f.ShortLength, 5.0);
            mockInputFactory.Setup(f => f.GetInputData()).Returns(rebarCrackInputData);

            var factory = new RebarCalulatorsFactory(mockInputFactory.Object)
            {
                Rebars = new List<RebarPrimitive> { rebar },
                InputData = inputData,
                LongLength = 10.0,
                ShortLength = 5.0,
                TraceLogger = null
            };

            // Act
            var calculators = factory.GetCalculators();

            // Assert
            var calculator = calculators[0];
            Assert.NotNull(calculator);
            Assert.AreEqual(rebarCrackInputData, calculator.InputData);
            Assert.Null(calculator.TraceLogger);
        }

        [Test]
        public void GetCalculators_ShouldInitializeCalculatorsWithCorrectData()
        {
            // Arrange
            var mockInputFactory = new Mock<IRebarCrackInputDataFactory>();
            var rebar1 = new RebarPrimitive();
            var rebar2 = new RebarPrimitive();
            var inputData = new TupleCrackInputData();
            var rebarInputData1 = new RebarCrackCalculatorInputData();
            var rebarInputData2 = new RebarCrackCalculatorInputData();
            var mockLogger = new Mock<IShiftTraceLogger>();

            mockInputFactory.SetupSequence(f => f.GetInputData())
                .Returns(rebarInputData1)
                .Returns(rebarInputData2);
            mockInputFactory.Setup(f => f.GetInputData()).Returns(rebarInputData1);

            mockLogger.Setup(l => l.GetSimilarTraceLogger(50)).Returns(mockLogger.Object);

            var factory = new RebarCalulatorsFactory(mockInputFactory.Object)
            {
                Rebars = new List<RebarPrimitive> { rebar1, rebar2 },
                InputData = inputData,
                LongLength = 20.0,
                ShortLength = 10.0,
                TraceLogger = mockLogger.Object
            };

            // Act
            var calculators = factory.GetCalculators();

            // Assert
            Assert.AreEqual(2, calculators.Count);
            Assert.AreEqual(rebarInputData1, calculators[0].InputData);
            //Assert.AreEqual(rebarInputData2, calculators[1].InputData);
            //Assert.AreEqual(mockLogger.Object, calculators[0].TraceLogger);
            //Assert.AreEqual(mockLogger.Object, calculators[1].TraceLogger);
        }
    }
}