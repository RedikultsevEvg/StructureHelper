using Moq;
using NUnit.Framework;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using System;
using System.Linq;
using System.Windows.Media;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class CustomConcreteMaterialViewModelTests
    {
        [Test]
        public void Constructor_ShouldLoadModelsAndSelectFirst()
        {
            (Mock<IHeadMaterial> head, ICustomConcreteHelperMaterial helper) = CreateHeadMaterial();
            var viewModel = new CustomConcreteMaterialViewModel(head.Object);

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Models.Items, Has.Count.EqualTo(1));
                Assert.That(viewModel.Models.SelectedItem, Is.SameAs(viewModel.Models.Items[0]));
                Assert.That(viewModel.Models.SelectedItem, Is.SameAs(helper.MaterialModels[0]));
                Assert.That(viewModel.ValidationError, Is.Empty);
            });
        }

        [Test]
        public void CopyModelCommand_ShouldAddInactiveCopy()
        {
            (Mock<IHeadMaterial> head, ICustomConcreteHelperMaterial helper) = CreateHeadMaterial();
            var viewModel = new CustomConcreteMaterialViewModel(head.Object);

            viewModel.Models.CopyMethod(null);

            Assert.Multiple(() =>
            {
                Assert.That(helper.MaterialModels, Has.Count.EqualTo(2));
                Assert.That(viewModel.Models.Items, Has.Count.EqualTo(2));
                Assert.That(helper.MaterialModels[1].IsActive, Is.False);
                Assert.That(helper.MaterialModels[1].Name, Is.EqualTo("Model copy"));
                Assert.That(helper.MaterialModels[1].Id, Is.Not.EqualTo(helper.MaterialModels[0].Id));
            });
        }

        [Test]
        public void RestoreOriginalState_ShouldRestoreModelsNameAndColor()
        {
            (Mock<IHeadMaterial> head, ICustomConcreteHelperMaterial helper) = CreateHeadMaterial();
            var viewModel = new CustomConcreteMaterialViewModel(head.Object);
            ICustomConcreteModel originalModel = helper.MaterialModels[0];

            viewModel.Name = "Changed material";
            head.Object.Color = Colors.Red;
            originalModel.Name = "Changed model";
            originalModel.IsActive = false;
            originalModel.YoungModulus = 1e10;
            viewModel.Models.CopyMethod(null);

            viewModel.RestoreOriginalState();

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Name, Is.EqualTo("Material"));
                Assert.That(head.Object.Color, Is.EqualTo(Colors.Blue));
                Assert.That(helper.MaterialModels, Has.Count.EqualTo(1));
                Assert.That(helper.MaterialModels[0], Is.SameAs(originalModel));
                Assert.That(originalModel.Name, Is.EqualTo("Model"));
                Assert.That(originalModel.IsActive, Is.True);
                Assert.That(originalModel.YoungModulus, Is.EqualTo(3e10));
            });
        }

        [Test]
        public void SafetyFactors_ShouldUseCollectionViewModelAndRestoreOriginalValues()
        {
            (Mock<IHeadMaterial> head, ICustomConcreteHelperMaterial helper) = CreateHeadMaterial();
            int initialCount = helper.SafetyFactors.Count;
            helper.SafetyFactors.Add(new StructureHelperCommon.Models.Materials.Libraries.MaterialSafetyFactor
            {
                Name = "Original factor"
            });
            var viewModel = new CustomConcreteMaterialViewModel(head.Object);

            Assert.That(viewModel.SafetyFactors.Items, Has.Count.EqualTo(initialCount + 1));

            viewModel.SafetyFactors.AddMethod(null);
            var originalFactor = viewModel.SafetyFactors.Items
                .First(x => x.Name == "Original factor");
            viewModel.SafetyFactors.SelectedItem = originalFactor;
            originalFactor.Name = "Changed factor";
            viewModel.RestoreOriginalState();

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.SafetyFactors.Items, Has.Count.EqualTo(initialCount + 1));
                Assert.That(viewModel.SafetyFactors.Items.Any(x => x.Name == "Original factor"), Is.True);
            });
        }

        [Test]
        public void TensionForUlsAndSls_ShouldChangeAndRestore()
        {
            (Mock<IHeadMaterial> head, ICustomConcreteHelperMaterial helper) = CreateHeadMaterial();
            var viewModel = new CustomConcreteMaterialViewModel(head.Object);
            var concreteHelper = (CustomConcreteHelperMaterial)helper;

            viewModel.TensionForULS = true;
            viewModel.TensionForSLS = false;

            Assert.Multiple(() =>
            {
                Assert.That(concreteHelper.TensionForULS, Is.True);
                Assert.That(concreteHelper.TensionForSLS, Is.False);
            });

            viewModel.RestoreOriginalState();

            Assert.Multiple(() =>
            {
                Assert.That(concreteHelper.TensionForULS, Is.False);
                Assert.That(concreteHelper.TensionForSLS, Is.True);
            });
        }

        [Test]
        public void HeadMaterialClone_ShouldCloneCustomConcreteModels()
        {
            (Mock<IHeadMaterial> head, ICustomConcreteHelperMaterial helper) = CreateHeadMaterial();
            var original = new HeadMaterial
            {
                Name = "Material",
                HelperMaterial = helper
            };

            var clone = original.Clone() as IHeadMaterial;

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(clone.HelperMaterial, Is.InstanceOf<ICustomConcreteHelperMaterial>());
                var cloneHelper = (ICustomConcreteHelperMaterial)clone.HelperMaterial;
                Assert.That(cloneHelper.MaterialModels, Has.Count.EqualTo(1));
                Assert.That(cloneHelper.MaterialModels[0], Is.Not.SameAs(helper.MaterialModels[0]));
                Assert.That(cloneHelper.MaterialModels[0].Name, Is.EqualTo("Model"));
            });
        }

        [Test]
        public void ValidationError_WhenNoActiveModels_ShouldReportError()
        {
            (Mock<IHeadMaterial> head, ICustomConcreteHelperMaterial helper) = CreateHeadMaterial();
            helper.MaterialModels[0].IsActive = false;
            var viewModel = new CustomConcreteMaterialViewModel(head.Object);

            Assert.That(viewModel.ValidationError, Is.Not.Empty);
        }

        [Test]
        public void ValidationError_WhenTwoActiveModelsUseSamePair_ShouldReportError()
        {
            (Mock<IHeadMaterial> head, ICustomConcreteHelperMaterial helper) = CreateHeadMaterial();
            helper.MaterialModels.Add(CreateModel("Second", true));
            var viewModel = new CustomConcreteMaterialViewModel(head.Object);

            Assert.That(viewModel.ValidationError, Is.Not.Empty);
        }

        private static (Mock<IHeadMaterial> Head, ICustomConcreteHelperMaterial Helper) CreateHeadMaterial()
        {
            var helper = new CustomConcreteHelperMaterial(Guid.NewGuid());
            helper.MaterialModels.Add(CreateModel("Model", true));

            var head = new Mock<IHeadMaterial>();
            head.SetupAllProperties();
            head.Object.Name = "Material";
            head.Object.Color = Colors.Blue;
            head.Object.HelperMaterial = helper;
            return (head, helper);
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
