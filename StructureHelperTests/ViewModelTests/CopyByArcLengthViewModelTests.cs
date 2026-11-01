using NUnit.Framework;
using StructureHelper.Windows.Services;
using System;
using System.ComponentModel;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class CopyByArcLengthViewModelTests
    {
        [Test]
        public void Defaults_ShouldBeUsable()
        {
            var viewModel = new CopyByArcLengthViewModel();

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.CenterX, Is.Zero);
                Assert.That(viewModel.CenterY, Is.Zero);
                Assert.That(viewModel.StartAngle, Is.EqualTo(30d));
                Assert.That(viewModel.ArcLength, Is.EqualTo(0.1d));
                Assert.That(viewModel.CopyCount, Is.EqualTo(3));
                Assert.That(viewModel.IsClockwise, Is.False);
                Assert.That(viewModel.StartAngleRadians, Is.EqualTo(Math.PI / 6d).Within(1e-12));
            });
        }

        [Test]
        public void Properties_ShouldRoundTrip()
        {
            var viewModel = new CopyByArcLengthViewModel
            {
                CenterX = 0.1d,
                CenterY = -0.2d,
                StartAngle = 15d,
                ArcLength = 0.25d,
                CopyCount = 5,
                IsClockwise = true
            };

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.CenterX, Is.EqualTo(0.1d));
                Assert.That(viewModel.CenterY, Is.EqualTo(-0.2d));
                Assert.That(viewModel.StartAngle, Is.EqualTo(15d));
                Assert.That(viewModel.ArcLength, Is.EqualTo(0.25d));
                Assert.That(viewModel.CopyCount, Is.EqualTo(5));
                Assert.That(viewModel.IsClockwise, Is.True);
            });
        }

        [TestCase(0d)]
        [TestCase(-0.1d)]
        [TestCase(double.NaN)]
        public void Indexer_WhenArcLengthIsNotPositiveFinite_ShouldReportError(double arcLength)
        {
            var viewModel = new CopyByArcLengthViewModel
            {
                ArcLength = arcLength
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.That(errorInfo[nameof(CopyByArcLengthViewModel.ArcLength)], Is.Not.Empty);
        }

        [Test]
        public void Indexer_WhenCenterIsNaN_ShouldReportErrors()
        {
            var viewModel = new CopyByArcLengthViewModel
            {
                CenterX = double.NaN,
                CenterY = double.NaN
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.Multiple(() =>
            {
                Assert.That(errorInfo[nameof(CopyByArcLengthViewModel.CenterX)], Is.Not.Empty);
                Assert.That(errorInfo[nameof(CopyByArcLengthViewModel.CenterY)], Is.Not.Empty);
            });
        }

        [Test]
        public void Indexer_WhenCopyCountIsNotPositive_ShouldReportError()
        {
            var viewModel = new CopyByArcLengthViewModel
            {
                CopyCount = 0
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.That(errorInfo[nameof(CopyByArcLengthViewModel.CopyCount)], Is.Not.Empty);
        }
    }
}
