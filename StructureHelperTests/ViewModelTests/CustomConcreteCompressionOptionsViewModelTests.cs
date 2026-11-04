using NUnit.Framework;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Models.Materials;
using System;
using System.ComponentModel;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class CustomConcreteCompressionOptionsViewModelTests
    {
        [Test]
        public void Defaults_ShouldBeValid()
        {
            var options = new CustomConcreteCompressionOptions(Guid.NewGuid());
            var viewModel = new CustomConcreteCompressionOptionsViewModel(options);

            Assert.That(viewModel.IsValid, Is.True);
        }

        [Test]
        public void Properties_ShouldChangeUnderlyingOptions()
        {
            var options = new CustomConcreteCompressionOptions(Guid.NewGuid());
            var viewModel = new CustomConcreteCompressionOptionsViewModel(options)
            {
                PeakStress = 2e7,
                PeakStrain = 0.0015,
                LimitStrain = 0.003,
                DescendingScaleFactor = 0.8
            };

            Assert.Multiple(() =>
            {
                Assert.That(options.PeakStress, Is.EqualTo(2e7));
                Assert.That(options.PeakStrain, Is.EqualTo(0.0015));
                Assert.That(options.LimitStrain, Is.EqualTo(0.003));
                Assert.That(options.DescendingScaleFactor, Is.EqualTo(0.8));
            });
        }

        [TestCase(0d)]
        [TestCase(-1d)]
        [TestCase(double.NaN)]
        public void PeakStress_WhenNotPositive_ShouldReportError(double value)
        {
            var options = new CustomConcreteCompressionOptions(Guid.NewGuid());
            var viewModel = new CustomConcreteCompressionOptionsViewModel(options)
            {
                PeakStress = value
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.IsValid, Is.False);
                Assert.That(errorInfo[nameof(CustomConcreteCompressionOptionsViewModel.PeakStress)], Is.Not.Empty);
            });
        }

        [Test]
        public void LimitStrain_WhenNotGreaterThanPeakStrain_ShouldReportError()
        {
            var options = new CustomConcreteCompressionOptions(Guid.NewGuid());
            var viewModel = new CustomConcreteCompressionOptionsViewModel(options)
            {
                PeakStrain = 0.002,
                LimitStrain = 0.002
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.That(errorInfo[nameof(CustomConcreteCompressionOptionsViewModel.LimitStrain)], Is.Not.Empty);
        }
    }
}
