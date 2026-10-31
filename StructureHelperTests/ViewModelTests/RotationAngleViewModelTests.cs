using NUnit.Framework;
using StructureHelper.Windows.Shapes;
using System;
using System.ComponentModel;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class RotationAngleViewModelTests
    {
        [TestCase(90d, Math.PI / 2d)]
        [TestCase(180d, Math.PI)]
        [TestCase(-90d, -Math.PI / 2d)]
        public void AngleRadians_ShouldConvertDegreesToRadians(double degrees, double expectedRadians)
        {
            var viewModel = new RotationAngleViewModel
            {
                AngleDegrees = degrees
            };

            Assert.That(viewModel.AngleRadians, Is.EqualTo(expectedRadians).Within(1e-12));
        }

        [Test]
        public void Indexer_WhenAngleIsNaN_ShouldReportError()
        {
            var viewModel = new RotationAngleViewModel
            {
                AngleDegrees = double.NaN
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.That(errorInfo[nameof(RotationAngleViewModel.AngleDegrees)], Is.Not.Empty);
        }

        [Test]
        public void Indexer_WhenAngleIsFinite_ShouldNotReportError()
        {
            var viewModel = new RotationAngleViewModel
            {
                AngleDegrees = 45d
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.That(errorInfo[nameof(RotationAngleViewModel.AngleDegrees)], Is.Empty);
        }
    }
}
