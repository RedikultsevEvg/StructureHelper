using NUnit.Framework;
using StructureHelper.Windows.Shapes;
using System;
using System.ComponentModel;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class RotationAngleViewModelTests
    {
        [TestCase(90d, true, -Math.PI / 2d)]
        [TestCase(180d, true, -Math.PI)]
        [TestCase(-90d, true, Math.PI / 2d)]
        [TestCase(90d, false, Math.PI / 2d)]
        [TestCase(180d, false, Math.PI)]
        public void AngleRadians_ShouldConvertDegreesAndApplyDirection(
            double degrees,
            bool isClockwise,
            double expectedRadians)
        {
            var viewModel = new RotationAngleViewModel
            {
                AngleDegrees = degrees,
                IsClockwise = isClockwise
            };

            Assert.That(viewModel.AngleRadians, Is.EqualTo(expectedRadians).Within(1e-12));
        }

        [Test]
        public void IsClockwise_ShouldBeTrueByDefault()
        {
            var viewModel = new RotationAngleViewModel();

            Assert.That(viewModel.IsClockwise, Is.True);
        }

        [Test]
        public void IsClockwise_WhenFalse_ShouldRotateCounterClockwise()
        {
            var viewModel = new RotationAngleViewModel
            {
                AngleDegrees = 90d,
                IsClockwise = false
            };

            Assert.That(viewModel.AngleRadians, Is.EqualTo(Math.PI / 2d).Within(1e-12));
        }

        [TestCase("15", 15d)]
        [TestCase("30", 30d)]
        [TestCase("45", 45d)]
        [TestCase("60", 60d)]
        [TestCase("75", 75d)]
        [TestCase("90", 90d)]
        public void SetAngleCommand_ShouldAssignAngleFromStringParameter(string parameter, double expectedAngle)
        {
            var viewModel = new RotationAngleViewModel();

            viewModel.SetAngleCommand.Execute(parameter);

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.AngleDegrees, Is.EqualTo(expectedAngle).Within(1e-12));
                Assert.That(viewModel.AngleRadians,
                    Is.EqualTo(-expectedAngle * Math.PI / 180d).Within(1e-12));
            });
        }

        [Test]
        public void SetAngleCommand_ShouldAssignAngleFromDoubleParameter()
        {
            var viewModel = new RotationAngleViewModel();

            viewModel.SetAngleCommand.Execute(45d);

            Assert.That(viewModel.AngleDegrees, Is.EqualTo(45d).Within(1e-12));
        }

        [Test]
        public void Center_ShouldBeZeroByDefault()
        {
            var viewModel = new RotationAngleViewModel();

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.CenterX, Is.Zero);
                Assert.That(viewModel.CenterY, Is.Zero);
            });
        }

        [Test]
        public void Constructor_ShouldSetRotationCenter()
        {
            var viewModel = new RotationAngleViewModel(0.1d, -0.2d);

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.CenterX, Is.EqualTo(0.1d));
                Assert.That(viewModel.CenterY, Is.EqualTo(-0.2d));
            });
        }

        [Test]
        public void CenterProperties_ShouldRoundTrip()
        {
            var viewModel = new RotationAngleViewModel
            {
                CenterX = 0.05d,
                CenterY = -0.1d
            };

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.CenterX, Is.EqualTo(0.05d));
                Assert.That(viewModel.CenterY, Is.EqualTo(-0.1d));
            });
        }

        [Test]
        public void Indexer_WhenCenterIsNaN_ShouldReportErrors()
        {
            var viewModel = new RotationAngleViewModel
            {
                CenterX = double.NaN,
                CenterY = double.NaN
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.Multiple(() =>
            {
                Assert.That(errorInfo[nameof(RotationAngleViewModel.CenterX)], Is.Not.Empty);
                Assert.That(errorInfo[nameof(RotationAngleViewModel.CenterY)], Is.Not.Empty);
            });
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
