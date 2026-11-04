using NUnit.Framework;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class CustomConcreteModelsViewModelTests
    {
        [Test]
        public void Constructor_ShouldExposeBackingCollectionAndSelectLastItem()
        {
            List<ICustomConcreteModel> collection = new()
            {
                CreateModel("First", true),
                CreateModel("Second", false)
            };

            var viewModel = new CustomConcreteModelsViewModel(collection);

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Collection, Is.SameAs(collection));
                Assert.That(viewModel.Items, Has.Count.EqualTo(2));
                Assert.That(viewModel.SelectedItem, Is.SameAs(collection[1]));
            });
        }

        [Test]
        public void CopyMethod_ShouldAddInactiveClone()
        {
            ICustomConcreteModel source = CreateModel("Model", true);
            var viewModel = new CustomConcreteModelsViewModel(new List<ICustomConcreteModel> { source })
            {
                SelectedItem = source
            };

            viewModel.CopyMethod(null);

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Collection, Has.Count.EqualTo(2));
                Assert.That(viewModel.Items, Has.Count.EqualTo(2));
                Assert.That(viewModel.Collection[1].Name, Is.EqualTo("Model copy"));
                Assert.That(viewModel.Collection[1].IsActive, Is.False);
                Assert.That(viewModel.SelectedItem, Is.SameAs(viewModel.Collection[1]));
            });
        }

        [Test]
        public void Reload_ShouldRebuildItemsAndSelectRequestedItem()
        {
            ICustomConcreteModel first = CreateModel("First", true);
            ICustomConcreteModel second = CreateModel("Second", false);
            var collection = new List<ICustomConcreteModel> { first, second };
            var viewModel = new CustomConcreteModelsViewModel(collection);

            collection.Remove(second);
            viewModel.Reload(first.Id);

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Items, Has.Count.EqualTo(1));
                Assert.That(viewModel.SelectedItem, Is.SameAs(first));
            });
        }

        private static ICustomConcreteModel CreateModel(string name, bool isActive)
        {
            var model = new CustomConcreteModel(Guid.NewGuid())
            {
                Name = name,
                IsActive = isActive,
                YoungModulus = 3e10
            };
            model.StateCalcTermPair.LimitState = LimitStates.ULS;
            model.StateCalcTermPair.CalcTerm = CalcTerms.ShortTerm;
            return model;
        }
    }
}
