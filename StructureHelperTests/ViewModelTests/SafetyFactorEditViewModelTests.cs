using NUnit.Framework;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using System;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class SafetyFactorEditViewModelTests
    {
        [Test]
        public void Properties_ShouldChangeOriginalFactor()
        {
            var factor = new MaterialSafetyFactor(Guid.NewGuid())
            {
                Name = "Original",
                Take = true,
                Description = "Original description"
            };
            var viewModel = new SafetyFactorEditViewModel(factor)
            {
                Name = "Changed",
                Take = false,
                Description = "Changed description"
            };

            Assert.Multiple(() =>
            {
                Assert.That(factor.Name, Is.EqualTo("Changed"));
                Assert.That(factor.Take, Is.False);
                Assert.That(factor.Description, Is.EqualTo("Changed description"));
            });
        }

        [Test]
        public void RestoreOriginalValues_ShouldRestoreAllValues()
        {
            var factor = new MaterialSafetyFactor(Guid.NewGuid())
            {
                Name = "Original",
                Take = true,
                Description = "Original description"
            };
            var viewModel = new SafetyFactorEditViewModel(factor)
            {
                Name = "Changed",
                Take = false,
                Description = "Changed description"
            };

            viewModel.RestoreOriginalValues();

            Assert.Multiple(() =>
            {
                Assert.That(factor.Name, Is.EqualTo("Original"));
                Assert.That(factor.Take, Is.True);
                Assert.That(factor.Description, Is.EqualTo("Original description"));
            });
        }
    }
}
