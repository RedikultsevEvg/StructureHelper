using NUnit.Framework;
using StructureHelper.Services.Exports;
using StructureHelperTests.Infrastructures.LoadHistory;
using System;
using System.Linq;

namespace StructureHelperTests.UnitTests.LoadHistory
{
    [TestFixture]
    public class HistoryResultTableBuilderTests
    {
        [Test]
        public void GetRows_ShouldConvertValuesToDisplayUnits()
        {
            var stageResult = HistoryResultTestData.CreateSubStageResult();

            object?[] row = HistoryResultTableBuilder.GetRows(new[] { stageResult }).Single();

            Assert.Multiple(() =>
            {
                Assert.That(row[0], Is.EqualTo("Stage 1"));
                Assert.That(row[1], Is.EqualTo("TRUE"));
                Assert.That(row[2], Is.EqualTo("Finish"));
                Assert.That(row[3], Is.EqualTo(7));
                Assert.That(row[4], Is.EqualTo(2.5d));
                Assert.That(row[6], Is.EqualTo(90d).Within(1e-9));
                Assert.That(row[7], Is.EqualTo(19d).Within(1e-9));
                Assert.That(row[8], Is.EqualTo(9d).Within(1e-9));
                Assert.That(row[9], Is.EqualTo(1e-4d).Within(1e-15));
                Assert.That(row[10], Is.EqualTo(2e-9d).Within(1e-15));
                Assert.That(row[11], Is.EqualTo(3e-9d).Within(1e-15));
                Assert.That(row[12], Is.EqualTo(1e-6d).Within(1e-15));
                Assert.That(row[13], Is.EqualTo(12));
            });
        }

        [Test]
        public void GetRows_WhenLoaderResultsAreMissing_ShouldUseForceAndStrainTuples()
        {
            var stageResult = HistoryResultTestData.CreateSubStageResult();
            stageResult.ForcesTupleResult = null!;

            object?[] row = HistoryResultTableBuilder.GetRows(new[] { stageResult }).Single();

            Assert.Multiple(() =>
            {
                Assert.That(row[6], Is.EqualTo(90d).Within(1e-9));
                Assert.That(row[7], Is.EqualTo(19d).Within(1e-9));
                Assert.That(row[12], Is.Null);
                Assert.That(row[13], Is.Null);
            });
        }

        [Test]
        public void ToTabSeparatedText_ShouldWriteHeaderAndRows()
        {
            var stageResult = HistoryResultTestData.CreateSubStageResult();

            string text = HistoryResultTableBuilder.ToTabSeparatedText(new[] { stageResult });
            string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.Multiple(() =>
            {
                Assert.That(lines, Has.Length.EqualTo(2));
                Assert.That(lines[0], Is.EqualTo(string.Join('\t', HistoryResultTableBuilder.Headers)));
                Assert.That(lines[1], Does.StartWith("Stage 1\tTRUE\tFinish"));
            });
        }
    }
}
