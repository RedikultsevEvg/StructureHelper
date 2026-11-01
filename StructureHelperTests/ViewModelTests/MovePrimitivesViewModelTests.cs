using NUnit.Framework;
using StructureHelper.Windows.Services;
using System.ComponentModel;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class MovePrimitivesViewModelTests
    {
        [Test]
        public void Defaults_ShouldBeZero()
        {
            var viewModel = new MovePrimitivesViewModel();

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.DeltaX, Is.Zero);
                Assert.That(viewModel.DeltaY, Is.Zero);
            });
        }

        [Test]
        public void Properties_ShouldRoundTrip()
        {
            var viewModel = new MovePrimitivesViewModel
            {
                DeltaX = 0.1d,
                DeltaY = -0.2d
            };

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.DeltaX, Is.EqualTo(0.1d));
                Assert.That(viewModel.DeltaY, Is.EqualTo(-0.2d));
            });
        }

        [Test]
        public void Indexer_WhenOffsetsAreNaN_ShouldReportErrors()
        {
            var viewModel = new MovePrimitivesViewModel
            {
                DeltaX = double.NaN,
                DeltaY = double.NaN
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.Multiple(() =>
            {
                Assert.That(errorInfo[nameof(MovePrimitivesViewModel.DeltaX)], Is.Not.Empty);
                Assert.That(errorInfo[nameof(MovePrimitivesViewModel.DeltaY)], Is.Not.Empty);
            });
        }

        [Test]
        public void Indexer_WhenOffsetsAreFinite_ShouldNotReportErrors()
        {
            var viewModel = new MovePrimitivesViewModel
            {
                DeltaX = 0.1d,
                DeltaY = 0.2d
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.Multiple(() =>
            {
                Assert.That(errorInfo[nameof(MovePrimitivesViewModel.DeltaX)], Is.Empty);
                Assert.That(errorInfo[nameof(MovePrimitivesViewModel.DeltaY)], Is.Empty);
            });
        }
    }
}
