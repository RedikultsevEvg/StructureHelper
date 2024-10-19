using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperTests.UnitTests.Ndms.Cracks
{
    public class TupleRebarsCrackSolverTest
    {
        [Test]
        public void Run_ShouldProcessAllCalculatorsAndSetValidResults()
        {
            // Arrange
            var mockCalculatorFactory = new Mock<IRebarCalulatorsFactory>();
            var mockCalculator = new Mock<IRebarCrackCalculator>();
            mockCalculator.Setup(c => c.Run());
            mockCalculator.Setup(c => c.Result).Returns(new RebarCrackResult { IsValid = true });

            var calculators = new List<IRebarCrackCalculator> { mockCalculator.Object };
            mockCalculatorFactory.Setup(f => f.GetCalculators()).Returns(calculators);

            var solver = new TupleRebarsCrackSolver(mockCalculatorFactory.Object)
            {
                Rebars = new List<RebarNdmPrimitive>(),
                InputData = new TupleCrackInputData(),
                LongLength = 10.0,
                ShortLength = 5.0,
                TraceLogger = null
            };

            // Act
            solver.Run();

            // Assert
            mockCalculator.Verify(c => c.Run(), Times.Once);
            Assert.True(solver.IsResultValid);
            Assert.NotNull(solver.Result);
            Assert.True(solver.Result.All(r => r.IsValid));
        }

        [Test]
        public void Run_ShouldSetInvalidResultWhenAnyCalculatorResultIsInvalid()
        {
            // Arrange
            var mockCalculatorFactory = new Mock<IRebarCalulatorsFactory>();
            var mockCalculator1 = new Mock<IRebarCrackCalculator>();
            mockCalculator1.Setup(c => c.Run());
            mockCalculator1.Setup(c => c.Result).Returns(new RebarCrackResult { IsValid = true });

            var mockCalculator2 = new Mock<IRebarCrackCalculator>();
            mockCalculator2.Setup(c => c.Run());
            mockCalculator2.Setup(c => c.Result).Returns(new RebarCrackResult { IsValid = false });

            var calculators = new List<IRebarCrackCalculator> { mockCalculator1.Object, mockCalculator2.Object };
            mockCalculatorFactory.Setup(f => f.GetCalculators()).Returns(calculators);

            var solver = new TupleRebarsCrackSolver(mockCalculatorFactory.Object)
            {
                Rebars = new List<RebarNdmPrimitive>(),
                InputData = new TupleCrackInputData(),
                LongLength = 10.0,
                ShortLength = 5.0,
                TraceLogger = null
            };

            // Act
            solver.Run();

            // Assert
            mockCalculator1.Verify(c => c.Run(), Times.Once);
            mockCalculator2.Verify(c => c.Run(), Times.Once);
            Assert.False(solver.IsResultValid);
        }
    }
}