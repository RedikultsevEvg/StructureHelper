using DataAccess.DTOs;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.MaterialTests
{
    [TestFixture]
    public class CustomConcreteDtoConvertersTests
    {
        [Test]
        public void CompressionOptions_ShouldRoundTripThroughDTO()
        {
            var source = new CustomConcreteCompressionOptions(Guid.NewGuid())
            {
                PeakStress = 2e7,
                PeakStrain = 0.0015,
                LimitStrain = 0.003,
                DescendingScaleFactor = 0.8
            };

            var dto = new CustomConcreteCompressionOptionsToDTOConvertStrategy().Convert(source);
            var restored = new CustomConcreteCompressionOptionsFromDTOConvertStrategy().Convert(dto);

            Assert.Multiple(() =>
            {
                Assert.That(dto.Id, Is.EqualTo(source.Id));
                Assert.That(restored.Id, Is.EqualTo(source.Id));
                Assert.That(restored.PeakStress, Is.EqualTo(source.PeakStress));
                Assert.That(restored.PeakStrain, Is.EqualTo(source.PeakStrain));
                Assert.That(restored.LimitStrain, Is.EqualTo(source.LimitStrain));
                Assert.That(restored.DescendingScaleFactor, Is.EqualTo(source.DescendingScaleFactor));
            });
        }

        [Test]
        public void TensionOptions_ShouldRoundTripThroughDTO()
        {
            var source = new CustomConcreteTensionOptions(Guid.NewGuid())
            {
                Strength = 2e6,
                UltimateStrain = 0.0002,
                LimitStrain = 0.01
            };

            var dto = new CustomConcreteTensionOptionsToDTOConvertStrategy().Convert(source);
            var restored = new CustomConcreteTensionOptionsFromDTOConvertStrategy().Convert(dto);

            Assert.Multiple(() =>
            {
                Assert.That(dto.Id, Is.EqualTo(source.Id));
                Assert.That(restored.Id, Is.EqualTo(source.Id));
                Assert.That(restored.Strength, Is.EqualTo(source.Strength));
                Assert.That(restored.UltimateStrain, Is.EqualTo(source.UltimateStrain));
                Assert.That(restored.LimitStrain, Is.EqualTo(source.LimitStrain));
            });
        }

        [Test]
        public void Model_ShouldRoundTripThroughDTO()
        {
            var source = CreateModel("Model", true);
            source.CompressionOptions.PeakStress = 4e7;
            source.TensionOptions.Strength = 3e6;

            var dto = new CustomConcreteModelToDTOConvertStrategy().Convert(source);
            var restored = new CustomConcreteModelFromDTOConvertStrategy().Convert(dto);

            Assert.Multiple(() =>
            {
                Assert.That(dto.Id, Is.EqualTo(source.Id));
                Assert.That(restored.Id, Is.EqualTo(source.Id));
                Assert.That(restored.Name, Is.EqualTo(source.Name));
                Assert.That(restored.IsActive, Is.True);
                Assert.That(restored.YoungModulus, Is.EqualTo(source.YoungModulus));
                Assert.That(restored.StateCalcTermPair.LimitState, Is.EqualTo(source.StateCalcTermPair.LimitState));
                Assert.That(restored.StateCalcTermPair.CalcTerm, Is.EqualTo(source.StateCalcTermPair.CalcTerm));
                Assert.That(restored.CompressionOptions.PeakStress, Is.EqualTo(source.CompressionOptions.PeakStress));
                Assert.That(restored.TensionOptions.Strength, Is.EqualTo(source.TensionOptions.Strength));
            });
        }

        [Test]
        public void HelperMaterial_ShouldRoundTripThroughDTO()
        {
            var source = new CustomConcreteHelperMaterial(Guid.NewGuid());
            source.MaterialModels.Add(CreateModel("Model 1", true));
            source.MaterialModels.Add(CreateModel("Model 2", false));
            source.TensionForULS = true;
            source.TensionForSLS = false;

            var dto = new CustomConcreteHelperMaterialToDTOConvertStrategy().Convert(source);
            var restored = new CustomConcreteHelperMaterialFromDTOConvertStrategy().Convert(dto);

            Assert.Multiple(() =>
            {
                Assert.That(dto.Id, Is.EqualTo(source.Id));
                Assert.That(dto.TensionForULS, Is.True);
                Assert.That(dto.TensionForSLS, Is.False);
                Assert.That(restored.Id, Is.EqualTo(source.Id));
                Assert.That(restored.TensionForULS, Is.True);
                Assert.That(restored.TensionForSLS, Is.False);
                Assert.That(restored.MaterialModels, Has.Count.EqualTo(2));
                Assert.That(restored.MaterialModels[0].Name, Is.EqualTo("Model 1"));
                Assert.That(restored.MaterialModels[1].Name, Is.EqualTo("Model 2"));
                Assert.That(restored.MaterialModels[1].IsActive, Is.False);
            });
        }

        [Test]
        public void GenericHelperMaterialConverters_ShouldSupportCustomConcrete()
        {
            var source = new CustomConcreteHelperMaterial(Guid.NewGuid());
            source.MaterialModels.Add(CreateModel("Model", true));

            var toDto = new HelperMaterialToDTOConvertStrategy
            {
                ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>()
            };
            var dto = toDto.Convert(source) as CustomConcreteHelperMaterialDTO;

            Assert.That(dto, Is.Not.Null);

            var fromDto = new HelperMaterialFromDTOConvertStrategy
            {
                ReferenceDictionary = new Dictionary<(Guid id, Type type), ISaveable>()
            };
            var restored = fromDto.Convert(dto!) as ICustomConcreteHelperMaterial;

            Assert.Multiple(() =>
            {
                Assert.That(restored, Is.Not.Null);
                Assert.That(restored!.MaterialModels, Has.Count.EqualTo(1));
                Assert.That(restored.MaterialModels[0].Name, Is.EqualTo("Model"));
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
