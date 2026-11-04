using NUnit.Framework;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class CustomConcreteModelEditViewModelTests
    {
        [Test]
        public void ValidationError_ShouldBeEmptyForValidModel()
        {
            ICustomConcreteModel model = CreateModel();
            var viewModel = new CustomConcreteModelEditViewModel(model, new List<ICustomConcreteModel> { model });

            Assert.That(viewModel.ValidationError, Is.Empty);
        }

        [Test]
        public void Properties_ShouldChangeOriginalModel()
        {
            ICustomConcreteModel model = CreateModel();
            var viewModel = new CustomConcreteModelEditViewModel(model, new List<ICustomConcreteModel> { model })
            {
                Name = "Changed",
                IsActive = false,
                YoungModulus = 2e10,
                LimitState = LimitStates.SLS,
                CalcTerm = CalcTerms.LongTerm
            };
            viewModel.CompressionOptions.PeakStress = 2e7;
            viewModel.TensionOptions.Strength = 2e6;

            Assert.Multiple(() =>
            {
                Assert.That(model.Name, Is.EqualTo("Changed"));
                Assert.That(model.IsActive, Is.False);
                Assert.That(model.YoungModulus, Is.EqualTo(2e10));
                Assert.That(model.StateCalcTermPair.LimitState, Is.EqualTo(LimitStates.SLS));
                Assert.That(model.StateCalcTermPair.CalcTerm, Is.EqualTo(CalcTerms.LongTerm));
                Assert.That(model.CompressionOptions.PeakStress, Is.EqualTo(2e7));
                Assert.That(model.TensionOptions.Strength, Is.EqualTo(2e6));
            });
        }

        [Test]
        public void RestoreOriginalValues_ShouldRestoreAllModelAndOptionValues()
        {
            ICustomConcreteModel model = CreateModel();
            var originalTensionLimitStrain = model.TensionOptions.LimitStrain;
            var viewModel = new CustomConcreteModelEditViewModel(model, new List<ICustomConcreteModel> { model })
            {
                Name = "Changed",
                IsActive = false,
                YoungModulus = 2e10,
                LimitState = LimitStates.SLS,
                CalcTerm = CalcTerms.LongTerm
            };
            viewModel.CompressionOptions.PeakStress = 2e7;
            viewModel.TensionOptions.Strength = 2e6;

            viewModel.RestoreOriginalValues();

            Assert.Multiple(() =>
            {
                Assert.That(model.Name, Is.EqualTo("Original"));
                Assert.That(model.IsActive, Is.True);
                Assert.That(model.YoungModulus, Is.EqualTo(3e10));
                Assert.That(model.StateCalcTermPair.LimitState, Is.EqualTo(LimitStates.ULS));
                Assert.That(model.StateCalcTermPair.CalcTerm, Is.EqualTo(CalcTerms.ShortTerm));
                Assert.That(model.CompressionOptions.PeakStress, Is.EqualTo(3e7));
                Assert.That(model.TensionOptions.Strength, Is.EqualTo(1e6));
                Assert.That(model.TensionOptions.LimitStrain, Is.EqualTo(originalTensionLimitStrain));
            });
        }

        [Test]
        public void ValidationError_WhenAnotherActiveModelUsesSamePair_ShouldReportError()
        {
            ICustomConcreteModel model = CreateModel();
            ICustomConcreteModel another = CreateModel();
            var viewModel = new CustomConcreteModelEditViewModel(
                model,
                new List<ICustomConcreteModel> { model, another })
            {
                IsActive = true
            };

            Assert.That(viewModel.ValidationError, Is.Not.Empty);
        }

        [Test]
        public void ValidationError_WhenAnotherModelIsInactive_ShouldBeEmpty()
        {
            ICustomConcreteModel model = CreateModel();
            ICustomConcreteModel another = CreateModel();
            another.IsActive = false;
            var viewModel = new CustomConcreteModelEditViewModel(
                model,
                new List<ICustomConcreteModel> { model, another })
            {
                IsActive = true
            };

            Assert.That(viewModel.ValidationError, Is.Empty);
        }

        [Test]
        public void ValidationError_WhenYoungModulusIsNotPositive_ShouldReportError()
        {
            ICustomConcreteModel model = CreateModel();
            var viewModel = new CustomConcreteModelEditViewModel(model, new List<ICustomConcreteModel> { model })
            {
                YoungModulus = 0
            };

            Assert.That(viewModel.ValidationError, Is.Not.Empty);
        }

        private static ICustomConcreteModel CreateModel()
        {
            var model = new CustomConcreteModel(Guid.NewGuid())
            {
                Name = "Original",
                IsActive = true,
                YoungModulus = 3e10
            };
            model.StateCalcTermPair.LimitState = LimitStates.ULS;
            model.StateCalcTermPair.CalcTerm = CalcTerms.ShortTerm;
            return model;
        }
    }
}
