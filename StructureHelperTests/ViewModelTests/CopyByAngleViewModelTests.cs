using NUnit.Framework;
using StructureHelper.Windows.Services;
using System;
using System.ComponentModel;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class CopyByAngleViewModelTests
    {
        [Test]
        public void Defaults_ShouldBeUsable()
        {
            var viewModel = new CopyByAngleViewModel();

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.CenterX, Is.Zero);
                Assert.That(viewModel.CenterY, Is.Zero);
                Assert.That(viewModel.StartAngle, Is.EqualTo(30d));
                Assert.That(viewModel.AngleOfRotation, Is.EqualTo(30d));
                Assert.That(viewModel.CopyCount, Is.EqualTo(3));
                Assert.That(viewModel.IsClockwise, Is.False);
                Assert.That(viewModel.StartAngleRadians, Is.EqualTo(Math.PI / 6d).Within(1e-12));
                Assert.That(viewModel.AngleStepRadians, Is.EqualTo(Math.PI / 6d).Within(1e-12));
            });
        }

        [Test]
        public void IsClockwise_ShouldChangeAngularStepSign()
        {
            var viewModel = new CopyByAngleViewModel
            {
                AngleOfRotation = 45d,
                IsClockwise = true
            };

            Assert.That(viewModel.AngleStepRadians, Is.EqualTo(-Math.PI / 4d).Within(1e-12));
        }

        [Test]
        public void Properties_ShouldRoundTrip()
        {
            var viewModel = new CopyByAngleViewModel
            {
                CenterX = 0.1d,
                CenterY = -0.2d,
                StartAngle = 15d,
                AngleOfRotation = 60d,
                CopyCount = 5
            };

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.CenterX, Is.EqualTo(0.1d));
                Assert.That(viewModel.CenterY, Is.EqualTo(-0.2d));
                Assert.That(viewModel.StartAngle, Is.EqualTo(15d));
                Assert.That(viewModel.AngleOfRotation, Is.EqualTo(60d));
                Assert.That(viewModel.CopyCount, Is.EqualTo(5));
            });
        }

        [Test]
        public void Indexer_WhenCenterOrAngleIsNaN_ShouldReportErrors()
        {
            var viewModel = new CopyByAngleViewModel
            {
                CenterX = double.NaN,
                StartAngle = double.NaN
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.Multiple(() =>
            {
                Assert.That(errorInfo[nameof(CopyByAngleViewModel.CenterX)], Is.Not.Empty);
                Assert.That(errorInfo[nameof(CopyByAngleViewModel.StartAngle)], Is.Not.Empty);
            });
        }

        [Test]
        public void Indexer_WhenCopyCountIsNotPositive_ShouldReportError()
        {
            var viewModel = new CopyByAngleViewModel
            {
                CopyCount = 0
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.That(errorInfo[nameof(CopyByAngleViewModel.CopyCount)], Is.Not.Empty);
        }
    }
}
