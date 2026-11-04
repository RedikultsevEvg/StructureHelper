using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.MaterialTests
{
    [TestFixture]
    public class CustomConcreteModelCheckLogicTests
    {
        [Test]
        public void Check_ValidModel_ReturnsTrue()
        {
            var logic = new CustomConcreteModelCheckLogic { Entity = CreateModel() };

            Assert.That(logic.Check(), Is.True);
        }

        [Test]
        public void Check_EmptyName_StillReturnsTrue()
        {
            ICustomConcreteModel model = CreateModel();
            model.Name = string.Empty;
            var logic = new CustomConcreteModelCheckLogic { Entity = model };

            Assert.That(logic.Check(), Is.True);
        }

        [TestCase(0d)]
        [TestCase(-1d)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void Check_YoungModulusNotPositiveFinite_ReturnsFalse(double value)
        {
            ICustomConcreteModel model = CreateModel();
            model.YoungModulus = value;
            var logic = new CustomConcreteModelCheckLogic { Entity = model };

            Assert.That(logic.Check(), Is.False);
        }

        [Test]
        public void Check_InvalidCompressionOptions_ReturnsFalseAndReportsError()
        {
            ICustomConcreteModel model = CreateModel();
            model.CompressionOptions.PeakStress = 0;
            var logic = new CustomConcreteModelCheckLogic { Entity = model };

            Assert.Multiple(() =>
            {
                Assert.That(logic.Check(), Is.False);
                Assert.That(logic.CheckResult, Is.Not.Empty);
            });
        }

        [Test]
        public void Check_InvalidTensionOptions_ReturnsFalseAndReportsError()
        {
            ICustomConcreteModel model = CreateModel();
            model.TensionOptions.UltimateStrain = 0;
            var logic = new CustomConcreteModelCheckLogic { Entity = model };

            Assert.Multiple(() =>
            {
                Assert.That(logic.Check(), Is.False);
                Assert.That(logic.CheckResult, Is.Not.Empty);
            });
        }

        [Test]
        public void Check_NullEntity_ReturnsFalse()
        {
            var logic = new CustomConcreteModelCheckLogic();

            Assert.That(logic.Check(), Is.False);
        }

        private static ICustomConcreteModel CreateModel()
        {
            var model = new CustomConcreteModel(Guid.NewGuid())
            {
                Name = "Model",
                IsActive = true,
                YoungModulus = 3e10
            };
            model.StateCalcTermPair.LimitState = LimitStates.ULS;
            model.StateCalcTermPair.CalcTerm = CalcTerms.ShortTerm;
            return model;
        }
    }

    [TestFixture]
    public class CustomConcreteHelperMaterialCheckLogicTests
    {
        [Test]
        public void Check_ValidMaterial_ReturnsTrue()
        {
            var helper = CreateHelper();
            helper.MaterialModels.Add(CreateModel("Model", true));
            var logic = new CustomConcreteHelperMaterialCheckLogic { Entity = helper };

            Assert.That(logic.Check(), Is.True);
        }

        [Test]
        public void Check_NoModels_ReturnsFalse()
        {
            var logic = new CustomConcreteHelperMaterialCheckLogic
            {
                Entity = CreateHelper()
            };

            Assert.That(logic.Check(), Is.False);
        }

        [Test]
        public void Check_NullModel_ReturnsFalse()
        {
            var helper = CreateHelper();
            helper.MaterialModels.Add(null!);
            var logic = new CustomConcreteHelperMaterialCheckLogic { Entity = helper };

            Assert.That(logic.Check(), Is.False);
        }

        [Test]
        public void Check_NoActiveModels_ReturnsFalse()
        {
            var helper = CreateHelper();
            helper.MaterialModels.Add(CreateModel("Inactive", false));
            var logic = new CustomConcreteHelperMaterialCheckLogic { Entity = helper };

            Assert.That(logic.Check(), Is.False);
        }

        [Test]
        public void Check_DuplicateActivePairs_ReturnsFalse()
        {
            var helper = CreateHelper();
            helper.MaterialModels.Add(CreateModel("First", true));
            helper.MaterialModels.Add(CreateModel("Second", true));
            var logic = new CustomConcreteHelperMaterialCheckLogic { Entity = helper };

            Assert.Multiple(() =>
            {
                Assert.That(logic.Check(), Is.False);
                Assert.That(logic.CheckResult, Does.Contain("More than one active model"));
            });
        }

        [Test]
        public void Check_InvalidChildModel_ReturnsFalse()
        {
            var helper = CreateHelper();
            ICustomConcreteModel model = CreateModel("Invalid", true);
            model.CompressionOptions.PeakStress = -1;
            helper.MaterialModels.Add(model);
            var logic = new CustomConcreteHelperMaterialCheckLogic { Entity = helper };

            Assert.That(logic.Check(), Is.False);
        }

        [Test]
        public void Check_NullEntity_ReturnsFalse()
        {
            var logic = new CustomConcreteHelperMaterialCheckLogic();

            Assert.That(logic.Check(), Is.False);
        }

        private static CustomConcreteHelperMaterial CreateHelper()
        {
            return new CustomConcreteHelperMaterial(Guid.NewGuid());
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
