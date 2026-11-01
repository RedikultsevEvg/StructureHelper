using NUnit.Framework;
using StructureHelper.Windows.Shapes;
using System.ComponentModel;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class PolygonOffsetViewModelTests
    {
        [Test]
        public void Defaults_ShouldBeOutward()
        {
            var viewModel = new PolygonOffsetViewModel();

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Distance, Is.EqualTo(0.1d));
                Assert.That(viewModel.IsOutward, Is.True);
                Assert.That(viewModel.SignedDistance, Is.EqualTo(0.1d));
            });
        }

        [Test]
        public void SignedDistance_WhenInward_ShouldBeNegative()
        {
            var viewModel = new PolygonOffsetViewModel
            {
                Distance = 0.2d,
                IsOutward = false
            };

            Assert.That(viewModel.SignedDistance, Is.EqualTo(-0.2d));
        }

        [TestCase(0d)]
        [TestCase(-0.1d)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void Indexer_WhenDistanceIsNotPositiveFinite_ShouldReportError(double distance)
        {
            var viewModel = new PolygonOffsetViewModel
            {
                Distance = distance
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.That(errorInfo[nameof(PolygonOffsetViewModel.Distance)], Is.Not.Empty);
        }

        [Test]
        public void Indexer_WhenDistanceIsPositive_ShouldNotReportError()
        {
            var viewModel = new PolygonOffsetViewModel
            {
                Distance = 0.05d
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.That(errorInfo[nameof(PolygonOffsetViewModel.Distance)], Is.Empty);
        }
    }
}
