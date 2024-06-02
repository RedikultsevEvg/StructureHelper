using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperTests.UnitTests.Ndms.Cracks
{
    public class TensionRebarAreaByStrainLogicTests
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

            var logic = new TensionRebarAreaByStrainLogic(mockStressLogic.Object)
            {
                StrainMatrix = mockStrainMatrix.Object,
                Rebars = rebars,
                TraceLogger = mockTraceLogger.Object
            };

            // Act
            var result = logic.GetTensionRebarArea();

            // Assert
            // Expected result calculation:
            // maxStrain = 1.5
            // reducedArea for rebar 1 = 1.0 * 0.5 / 1.5 = 0.333...
            // reducedArea for rebar 2 = 2.0 * 1.0 / 1.5 = 1.333...
            // reducedArea for rebar 3 = 3.0 * 1.5 / 1.5 = 3.0
            // sumArea = 0.333... + 1.333... + 3.0 = 4.666...
            Assert.AreEqual(4.666666666666667, result, 9);
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

            var logic = new TensionRebarAreaByStrainLogic(mockStressLogic.Object)
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



