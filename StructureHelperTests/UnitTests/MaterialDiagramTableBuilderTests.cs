using LiveCharts;
using LiveCharts.Wpf;
using NUnit.Framework;
using StructureHelper.Services.Exports;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class MaterialDiagramTableBuilderTests
    {
        [Test]
        public void GetHeaders_ShouldUseSeriesTitles()
        {
            var series = new List<LineSeries>
            {
                new() { Title = "Concrete", Values = new ChartValues<double> { 1d, 2d } },
                new() { Title = string.Empty, Values = new ChartValues<double> { 3d } }
            };

            IReadOnlyList<string> headers = MaterialDiagramTableBuilder.GetHeaders(series);

            Assert.Multiple(() =>
            {
                Assert.That(headers, Has.Count.EqualTo(3));
                Assert.That(headers[0], Is.EqualTo("Strain"));
                Assert.That(headers[1], Is.EqualTo("Concrete"));
                Assert.That(headers[2], Is.EqualTo("Diagram 2"));
            });
        }

        [Test]
        public void GetRows_ShouldAlignLabelsAndSeriesValues()
        {
            var series = new List<LineSeries>
            {
                new() { Title = "A", Values = new ChartValues<double> { 10d, 20d } },
                new() { Title = "B", Values = new ChartValues<double> { 30d, 40d } }
            };
            var labels = new List<string> { "0", "0.001" };

            List<IReadOnlyList<object?>> rows = MaterialDiagramTableBuilder
                .GetRows(labels, series)
                .ToList();

            Assert.Multiple(() =>
            {
                Assert.That(rows, Has.Count.EqualTo(2));
                Assert.That(rows[0][0], Is.EqualTo(0d));
                Assert.That(rows[0][1], Is.EqualTo(10d));
                Assert.That(rows[0][2], Is.EqualTo(30d));
                Assert.That(rows[1][0], Is.EqualTo(0.001d));
                Assert.That(rows[1][1], Is.EqualTo(20d));
                Assert.That(rows[1][2], Is.EqualTo(40d));
            });
        }

        [Test]
        public void GetRows_WhenLabelsContainRepeatedBlocks_ShouldUseOneBlock()
        {
            var series = new List<LineSeries>
            {
                new() { Title = "A", Values = new ChartValues<double> { 10d, 20d } },
                new() { Title = "B", Values = new ChartValues<double> { 30d, 40d } }
            };
            var labels = new List<string> { "0", "0.001", "0", "0.001" };

            List<IReadOnlyList<object?>> rows = MaterialDiagramTableBuilder
                .GetRows(labels, series)
                .ToList();

            Assert.Multiple(() =>
            {
                Assert.That(rows, Has.Count.EqualTo(2));
                Assert.That(rows[0][0], Is.EqualTo(0d));
                Assert.That(rows[1][0], Is.EqualTo(0.001d));
            });
        }

        [Test]
        public void GetRows_WhenSeriesHasFewerValues_ShouldUseNulls()
        {
            var series = new List<LineSeries>
            {
                new() { Title = "A", Values = new ChartValues<double> { 10d, 20d } },
                new() { Title = "B", Values = new ChartValues<double> { 30d } }
            };
            var labels = new List<string> { "0", "0.001" };

            List<IReadOnlyList<object?>> rows = MaterialDiagramTableBuilder
                .GetRows(labels, series)
                .ToList();

            Assert.Multiple(() =>
            {
                Assert.That(rows, Has.Count.EqualTo(2));
                Assert.That(rows[1][1], Is.EqualTo(20d));
                Assert.That(rows[1][2], Is.Null);
            });
        }
    }
}
