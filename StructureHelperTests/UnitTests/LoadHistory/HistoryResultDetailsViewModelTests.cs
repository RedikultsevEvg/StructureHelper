using NUnit.Framework;
using StructureHelper.Windows.LoadHistory;
using StructureHelperTests.Infrastructures.LoadHistory;

namespace StructureHelperTests.UnitTests.LoadHistory
{
    [TestFixture]
    public class HistoryResultDetailsViewModelTests
    {
        [Test]
        public void Constructor_ShouldReadAccuracyIterationsForcesAndCurvatures()
        {
            var details = new HistoryResultDetailsViewModel(HistoryResultTestData.CreateSubStageResult());

            Assert.Multiple(() =>
            {
                Assert.That(details.StageName, Is.EqualTo("Stage 1"));
                Assert.That(details.Position, Is.EqualTo("Finish"));
                Assert.That(details.StepNumber, Is.EqualTo(7));
                Assert.That(details.AchievedAccuracy!.Value, Is.EqualTo(1e-6d).Within(1e-15));
                Assert.That(details.RequiredAccuracy!.Value, Is.EqualTo(1e-5d).Within(1e-15));
                Assert.That(details.IterationCount, Is.EqualTo(12));
                Assert.That(details.MaxIterationCount, Is.EqualTo(100));
                Assert.That(details.NzKilonewtons, Is.EqualTo(100d).Within(1e-9));
                Assert.That(details.MxKilonewtonMeters, Is.EqualTo(20d).Within(1e-9));
                Assert.That(details.MyKilonewtonMeters, Is.EqualTo(10d).Within(1e-9));
                Assert.That(details.EpsZ, Is.EqualTo(1e-4d).Within(1e-15));
                Assert.That(details.KxPerMillimeter, Is.EqualTo(2e-9d).Within(1e-15));
                Assert.That(details.KyPerMillimeter, Is.EqualTo(3e-9d).Within(1e-15));
            });
        }

        [Test]
        public void Constructor_WhenLoaderResultsAreMissing_ShouldFallBackToTuples()
        {
            var stageResult = HistoryResultTestData.CreateSubStageResult();
            stageResult.ForcesTupleResult = null!;

            var details = new HistoryResultDetailsViewModel(stageResult);

            Assert.Multiple(() =>
            {
                Assert.That(details.AchievedAccuracy, Is.Null);
                Assert.That(details.IterationCount, Is.Null);
                Assert.That(details.NzKilonewtons, Is.EqualTo(90d).Within(1e-9));
                Assert.That(details.EpsZ, Is.EqualTo(9e-5d).Within(1e-15));
            });
        }
    }
}
