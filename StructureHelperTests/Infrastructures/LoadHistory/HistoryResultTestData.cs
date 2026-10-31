using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.ResultData;
using Moq;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.Models.LoadHistory.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;

namespace StructureHelperTests.Infrastructures.LoadHistory
{
    /// <summary>
    /// Builds a load history sub-stage result with mocked loader results for unit tests.
    /// </summary>
    internal static class HistoryResultTestData
    {
        public static SubStageResultStub CreateSubStageResult()
        {
            var accuracy = new Mock<IAccuracy>();
            accuracy.SetupGet(x => x.IterationAccuracy).Returns(1e-5);
            accuracy.SetupGet(x => x.MaxIterationCount).Returns(100);

            var inputData = new Mock<IForceTupleInputData>();
            inputData.SetupGet(x => x.Accuracy).Returns(accuracy.Object);

            var forceMatrix = new Mock<IForceMatrix>();
            forceMatrix.SetupGet(x => x.Nz).Returns(100e3d);
            forceMatrix.SetupGet(x => x.Mx).Returns(20e3d);
            forceMatrix.SetupGet(x => x.My).Returns(10e3d);

            var strainMatrix = new Mock<IStrainMatrix>();
            strainMatrix.SetupGet(x => x.EpsZ).Returns(1e-4d);
            strainMatrix.SetupGet(x => x.Kx).Returns(2e-6d);
            strainMatrix.SetupGet(x => x.Ky).Returns(3e-6d);

            var forceStrainPair = new Mock<IForceStrainPair>();
            forceStrainPair.SetupGet(x => x.ForceMatrix).Returns(forceMatrix.Object);
            forceStrainPair.SetupGet(x => x.StrainMatrix).Returns(strainMatrix.Object);

            var loaderResults = new Mock<ILoaderResults>();
            loaderResults.SetupGet(x => x.AccuracyRate).Returns(1e-6d);
            loaderResults.SetupGet(x => x.IterationCounter).Returns(12);
            loaderResults.SetupGet(x => x.ForceStrainPair).Returns(forceStrainPair.Object);

            var calculatorResult = new Mock<IForceTupleCalculatorResult>();
            calculatorResult.SetupGet(x => x.LoaderResults).Returns(loaderResults.Object);
            calculatorResult.SetupGet(x => x.InputData).Returns(inputData.Object);

            var stage = new ConstructionStage(Guid.NewGuid())
            {
                Name = "Stage 1"
            };

            return new SubStageResultStub
            {
                ConstructionStage = stage,
                StagePosition = StagePosition.Finish,
                TotalNumber = 7,
                TotalTime = 2.5d,
                IsValid = true,
                Description = "done",
                ForceTuple = new ForceTuple { Nz = 90e3d, Mx = 19e3d, My = 9e3d },
                StrainTuple = new ForceTuple { Nz = 9e-5d, Mx = 1e-6d, My = 2e-6d },
                ForcesTupleResult = calculatorResult.Object
            };
        }
    }
}
