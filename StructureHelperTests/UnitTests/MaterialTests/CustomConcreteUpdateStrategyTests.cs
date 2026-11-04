using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using System;

namespace StructureHelperTests.UnitTests.MaterialTests
{
    [TestFixture]
    public class CustomConcreteOptionsUpdateStrategyTests
    {
        [Test]
        public void CompressionUpdate_ShouldCopyAllValues()
        {
            var target = new CustomConcreteCompressionOptions(Guid.NewGuid());
            var source = new CustomConcreteCompressionOptions(Guid.NewGuid())
            {
                PeakStress = 2e7,
                PeakStrain = 0.0015,
                LimitStrain = 0.003,
                DescendingScaleFactor = 0.8
            };

            new CustomConcreteCompressionOptionsUpdateStrategy().Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.PeakStress, Is.EqualTo(2e7));
                Assert.That(target.PeakStrain, Is.EqualTo(0.0015));
                Assert.That(target.LimitStrain, Is.EqualTo(0.003));
                Assert.That(target.DescendingScaleFactor, Is.EqualTo(0.8));
            });
        }

        [Test]
        public void TensionUpdate_ShouldCopyAllValues()
        {
            var target = new CustomConcreteTensionOptions(Guid.NewGuid());
            var source = new CustomConcreteTensionOptions(Guid.NewGuid())
            {
                Strength = 2e6,
                UltimateStrain = 0.0002,
                LimitStrain = 0.01
            };

            new CustomConcreteTensionOptionsUpdateStrategy().Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.Strength, Is.EqualTo(2e6));
                Assert.That(target.UltimateStrain, Is.EqualTo(0.0002));
                Assert.That(target.LimitStrain, Is.EqualTo(0.01));
            });
        }

        [Test]
        public void CompressionUpdate_NullTarget_Throws()
        {
            var source = new CustomConcreteCompressionOptions(Guid.NewGuid());
            Assert.Throws<StructureHelperException>(
                () => new CustomConcreteCompressionOptionsUpdateStrategy().Update(null!, source));
        }

        [Test]
        public void TensionUpdate_NullSource_Throws()
        {
            var target = new CustomConcreteTensionOptions(Guid.NewGuid());
            Assert.Throws<StructureHelperException>(
                () => new CustomConcreteTensionOptionsUpdateStrategy().Update(target, null!));
        }
    }

    [TestFixture]
    public class CustomConcreteModelUpdateStrategyTests
    {
        [Test]
        public void Update_ShouldCopyScalarsAndChildren()
        {
            ICustomConcreteModel target = CreateModel("Target", false, 2e10);
            ICustomConcreteModel source = CreateModel("Source", true, 3e10);
            source.StateCalcTermPair.LimitState = LimitStates.SLS;
            source.StateCalcTermPair.CalcTerm = CalcTerms.LongTerm;
            source.CompressionOptions.PeakStress = 4e7;
            source.TensionOptions.Strength = 2e6;

            new CustomConcreteModelUpdateStrategy().Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.Name, Is.EqualTo("Source"));
                Assert.That(target.IsActive, Is.True);
                Assert.That(target.YoungModulus, Is.EqualTo(3e10));
                Assert.That(target.StateCalcTermPair.LimitState, Is.EqualTo(LimitStates.SLS));
                Assert.That(target.StateCalcTermPair.CalcTerm, Is.EqualTo(CalcTerms.LongTerm));
                Assert.That(target.CompressionOptions.PeakStress, Is.EqualTo(4e7));
                Assert.That(target.TensionOptions.Strength, Is.EqualTo(2e6));
            });
        }

        [Test]
        public void Update_WhenChildrenAreDisabled_ShouldKeepChildren()
        {
            ICustomConcreteModel target = CreateModel("Target", false, 2e10);
            ICustomConcreteModel source = CreateModel("Source", true, 3e10);
            target.CompressionOptions.PeakStress = 1e7;
            source.CompressionOptions.PeakStress = 4e7;

            new CustomConcreteModelUpdateStrategy { UpdateChildren = false }.Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.Name, Is.EqualTo("Source"));
                Assert.That(target.CompressionOptions.PeakStress, Is.EqualTo(1e7));
            });
        }

        private static ICustomConcreteModel CreateModel(string name, bool isActive, double modulus)
        {
            var model = new CustomConcreteModel(Guid.NewGuid())
            {
                Name = name,
                IsActive = isActive,
                YoungModulus = modulus
            };
            model.StateCalcTermPair.LimitState = LimitStates.ULS;
            model.StateCalcTermPair.CalcTerm = CalcTerms.ShortTerm;
            return model;
        }
    }

    [TestFixture]
    public class CustomConcreteHelperMaterialUpdateStrategyTests
    {
        [Test]
        public void Update_ShouldUpdateMatchingModelsAndAddNewOnes()
        {
            Guid existingId = Guid.NewGuid();
            var target = new CustomConcreteHelperMaterial(Guid.NewGuid());
            var existingTargetModel = new CustomConcreteModel(existingId)
            {
                Name = "Old name",
                IsActive = false,
                YoungModulus = 1e10
            };
            existingTargetModel.StateCalcTermPair.LimitState = LimitStates.ULS;
            existingTargetModel.StateCalcTermPair.CalcTerm = CalcTerms.ShortTerm;
            target.MaterialModels.Add(existingTargetModel);
            target.MaterialModels.Add(CreateModel("Removed", false));

            var source = new CustomConcreteHelperMaterial(Guid.NewGuid());
            var existingSourceModel = new CustomConcreteModel(existingId)
            {
                Name = "New name",
                IsActive = true,
                YoungModulus = 3e10
            };
            existingSourceModel.StateCalcTermPair.LimitState = LimitStates.SLS;
            existingSourceModel.StateCalcTermPair.CalcTerm = CalcTerms.LongTerm;
            source.MaterialModels.Add(existingSourceModel);
            source.MaterialModels.Add(CreateModel("Added", false));

            new CustomConcreteHelperMaterialUpdateStrategy().Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.MaterialModels, Has.Count.EqualTo(2));
                Assert.That(target.MaterialModels[0], Is.SameAs(existingTargetModel));
                Assert.That(existingTargetModel.Name, Is.EqualTo("New name"));
                Assert.That(existingTargetModel.IsActive, Is.True);
                Assert.That(existingTargetModel.StateCalcTermPair.LimitState, Is.EqualTo(LimitStates.SLS));
                Assert.That(target.MaterialModels[1].Name, Is.EqualTo("Added"));
                Assert.That(target.MaterialModels[1].Id, Is.EqualTo(source.MaterialModels[1].Id));
            });
        }

        [Test]
        public void Update_WhenChildrenAreDisabled_ShouldKeepModelList()
        {
            var target = new CustomConcreteHelperMaterial(Guid.NewGuid());
            target.MaterialModels.Add(CreateModel("Target", false));
            var source = new CustomConcreteHelperMaterial(Guid.NewGuid());
            source.MaterialModels.Add(CreateModel("Source", true));

            new CustomConcreteHelperMaterialUpdateStrategy { UpdateChildren = false }.Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.MaterialModels, Has.Count.EqualTo(1));
                Assert.That(target.MaterialModels[0].Name, Is.EqualTo("Target"));
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
