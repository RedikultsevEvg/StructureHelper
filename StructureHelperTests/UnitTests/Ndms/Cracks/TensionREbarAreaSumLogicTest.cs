using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking;
namespace StructureHelperTests.UnitTests.Ndms.Cracks
{
    public class TensionRebarAreaSimpleSumLogicTests
    {
        [Test]
        public void GetTensionRebarArea_ShouldReturnCorrectSumArea_WhenRebarsInTension()
        {
            // Arrange
            var mockStressLogic = new Mock<IStressLogic>();
            var mockStrainMatrix = new Mock<IStrainMatrix>();
            var mockTraceLogger = new Mock<IShiftTraceLogger>();

            var rebars = new List<RebarNdm>
        {
            new RebarNdm { Area = 1.0, StressScale = 1.0 },
            new RebarNdm { Area = 2.0, StressScale = 1.0 },
            new RebarNdm { Area = 3.0, StressScale = 1.0 }
        };

            // Setup the mock to return positive strains
            mockStressLogic.Setup(s => s.GetSectionStrain(It.IsAny<IStrainMatrix>(), It.Is<RebarNdm>(r => r.Area == 1.0)))
                           .Returns(0.5);
            mockStressLogic.Setup(s => s.GetSectionStrain(It.IsAny<IStrainMatrix>(), It.Is<RebarNdm>(r => r.Area == 2.0)))
                           .Returns(1.0);
            mockStressLogic.Setup(s => s.GetSectionStrain(It.IsAny<IStrainMatrix>(), It.Is<RebarNdm>(r => r.Area == 3.0)))
                           .Returns(1.5);

            var logic = new TensionRebarAreaSimpleSumLogic(mockStressLogic.Object)
            {
                StrainMatrix = mockStrainMatrix.Object,
                Rebars = rebars,
                TraceLogger = mockTraceLogger.Object
            };

            // Act
            var result = logic.GetTensionRebarArea();

            // Assert
            // Expected result calculation:
            // rebarArea = 1.0 * 1.0 + 2.0 * 1.0 + 3.0 * 1.0 = 6.0
            Assert.AreEqual(6.0, result);

        }

        [Test]
        public void GetTensionRebarArea_ShouldThrowException_WhenNoRebarsInTension()
        {
            // Arrange
            var mockStressLogic = new Mock<IStressLogic>();
            var mockStrainMatrix = new Mock<IStrainMatrix>();
            var mockTraceLogger = new Mock<IShiftTraceLogger>();

            var rebars = new List<RebarNdm>
        {
            new RebarNdm { Area = 1.0, StressScale = 1.0 }
        };

            // Setup the mock to return non-positive strain
            mockStressLogic.Setup(s => s.GetSectionStrain(It.IsAny<IStrainMatrix>(), It.IsAny<RebarNdm>()))
                           .Returns(0.0);

            var logic = new TensionRebarAreaSimpleSumLogic(mockStressLogic.Object)
            {
                StrainMatrix = mockStrainMatrix.Object,
                Rebars = rebars,
                TraceLogger = mockTraceLogger.Object
            };

            // Act & Assert
            var ex = Assert.Throws<StructureHelperException>(() => logic.GetTensionRebarArea());
            StringAssert.Contains("Collection of rebars does not contain any tensile rebars", ex.Message);
        }
    }
}





