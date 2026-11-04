using NUnit.Framework;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Models.Materials;
using System;
using System.ComponentModel;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class CustomConcreteTensionOptionsViewModelTests
    {
        [Test]
        public void Defaults_ShouldBeValid()
        {
            var options = new CustomConcreteTensionOptions(Guid.NewGuid());
            var viewModel = new CustomConcreteTensionOptionsViewModel(options);

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.IsValid, Is.True);
            });
        }

        [Test]
        public void Properties_ShouldChangeUnderlyingOptions()
        {
            var options = new CustomConcreteTensionOptions(Guid.NewGuid());
            var viewModel = new CustomConcreteTensionOptionsViewModel(options)
            {
                Strength = 2e6,
                UltimateStrain = 0.0002
            };

            Assert.Multiple(() =>
            {
                Assert.That(options.Strength, Is.EqualTo(2e6));
                Assert.That(options.UltimateStrain, Is.EqualTo(0.0002));
            });
        }

        [Test]
        public void LimitStrain_ShouldNotBeExposed()
        {
            Assert.That(
                typeof(CustomConcreteTensionOptionsViewModel).GetProperty(nameof(CustomConcreteTensionOptions.LimitStrain)),
                Is.Null);
        }

        [Test]
        public void Strength_WhenValueIsNotPositive_ShouldReportError()
        {
            var options = new CustomConcreteTensionOptions(Guid.NewGuid());
            var viewModel = new CustomConcreteTensionOptionsViewModel(options)
            {
                Strength = 0
            };
            IDataErrorInfo errorInfo = viewModel;

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.IsValid, Is.False);
                Assert.That(errorInfo[nameof(CustomConcreteTensionOptionsViewModel.Strength)], Is.Not.Empty);
            });
        }
    }
}
