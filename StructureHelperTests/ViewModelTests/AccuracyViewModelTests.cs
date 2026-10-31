using NUnit.Framework;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Calculators;
using System.ComponentModel;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class AccuracyViewModelTests
    {
        [Test]
        public void Constructor_ShouldEditCopyAndLeaveTargetUntouched()
        {
            Accuracy target = CreateTarget();
            var viewModel = new AccuracyViewModel(target);

            viewModel.IterationAccuracy = 0.0001d;
            viewModel.MaxIterationCount = 200;

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.IterationAccuracy, Is.EqualTo(0.0001d));
                Assert.That(viewModel.MaxIterationCount, Is.EqualTo(200));
                Assert.That(target.IterationAccuracy, Is.EqualTo(0.001d));
                Assert.That(target.MaxIterationCount, Is.EqualTo(100));
            });
        }

        [Test]
        public void TryApply_ShouldCopyValuesToTarget()
        {
            Accuracy target = CreateTarget();
            var viewModel = new AccuracyViewModel(target)
            {
                IterationAccuracy = 1e-5d,
                MaxIterationCount = 50
            };

            bool applied = viewModel.TryApply();

            Assert.Multiple(() =>
            {
                Assert.That(applied, Is.True);
                Assert.That(viewModel.ValidationMessage, Is.Empty);
                Assert.That(target.IterationAccuracy, Is.EqualTo(1e-5d));
                Assert.That(target.MaxIterationCount, Is.EqualTo(50));
            });
        }

        [TestCase(0d)]
        [TestCase(-0.001d)]
        public void TryApply_WhenAccuracyIsNotPositive_ShouldFailAndKeepTarget(double value)
        {
            Accuracy target = CreateTarget();
            var viewModel = new AccuracyViewModel(target)
            {
                IterationAccuracy = value
            };

            bool applied = viewModel.TryApply();

            Assert.Multiple(() =>
            {
                Assert.That(applied, Is.False);
                Assert.That(viewModel.ValidationMessage, Is.Not.Empty);
                Assert.That(target.IterationAccuracy, Is.EqualTo(0.001d));
            });
        }

        [Test]
        public void TryApply_WhenAccuracyIsNaN_ShouldFailAndKeepTarget()
        {
            Accuracy target = CreateTarget();
            var viewModel = new AccuracyViewModel(target)
            {
                IterationAccuracy = double.NaN
            };

            bool applied = viewModel.TryApply();

            Assert.Multiple(() =>
            {
                Assert.That(applied, Is.False);
                Assert.That(viewModel.ValidationMessage, Is.Not.Empty);
                Assert.That(target.IterationAccuracy, Is.EqualTo(0.001d));
            });
        }

        [Test]
        public void TryApply_WhenMaxIterationCountIsNotPositive_ShouldFailAndKeepTarget()
        {
            Accuracy target = CreateTarget();
            var viewModel = new AccuracyViewModel(target)
            {
                MaxIterationCount = 0
            };

            bool applied = viewModel.TryApply();

            Assert.Multiple(() =>
            {
                Assert.That(applied, Is.False);
                Assert.That(viewModel.ValidationMessage, Is.Not.Empty);
                Assert.That(target.MaxIterationCount, Is.EqualTo(100));
            });
        }

        [Test]
        public void Indexer_ShouldReportInvalidValues()
        {
            var viewModel = new AccuracyViewModel(CreateTarget())
            {
                IterationAccuracy = 0d,
                MaxIterationCount = 0
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.Multiple(() =>
            {
                Assert.That(errorInfo[nameof(AccuracyViewModel.IterationAccuracy)], Is.Not.Empty);
                Assert.That(errorInfo[nameof(AccuracyViewModel.MaxIterationCount)], Is.Not.Empty);
            });
        }

        private static Accuracy CreateTarget()
        {
            return new Accuracy
            {
                IterationAccuracy = 0.001d,
                MaxIterationCount = 100
            };
        }
    }
}
