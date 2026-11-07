using Moq;
using NUnit.Framework;
using DataAccess.DTOs;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
namespace StructureHelperTests.UnitTests.CrossSections
{
    [TestFixture]
    public class CrossSectionRepositoryTests
    {
        [Test]
        public void Constructor_ShouldKeepSuppliedId()
        {
            Guid id = Guid.NewGuid();
            var repository = new CrossSectionRepository(id);
            Assert.That(repository.Id, Is.EqualTo(id));
        }

        [Test]
        public void AddHeadMaterial_ShouldRejectDuplicateId()
        {
            var repository = new CrossSectionRepository();
            var first = new Mock<IHeadMaterial>();
            var second = new Mock<IHeadMaterial>();
            first.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            second.SetupGet(x => x.Id).Returns(first.Object.Id);

            repository.AddHeadMaterial(first.Object);

            Assert.Throws<StructureHelperException>(() => repository.AddHeadMaterial(second.Object));
        }

        [Test]
        public void AddHeadMaterial_ShouldRejectDuplicateInstance()
        {
            var repository = new CrossSectionRepository();
            var material = new Mock<IHeadMaterial>();
            material.SetupGet(x => x.Id).Returns(Guid.NewGuid());

            repository.AddHeadMaterial(material.Object);

            Assert.Throws<StructureHelperException>(() => repository.AddHeadMaterial(material.Object));
        }

        [Test]
        public void RemoveMethods_ShouldBeIdempotent()
        {
            var repository = new CrossSectionRepository();
            var material = new Mock<IHeadMaterial>();
            material.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            repository.AddHeadMaterial(material.Object);

            Assert.That(repository.RemoveHeadMaterial(material.Object), Is.True);
            Assert.That(repository.RemoveHeadMaterial(material.Object), Is.False);
            Assert.That(repository.RemoveHeadMaterial(material.Object.Id), Is.False);
        }

        [Test]
        public void RemoveHeadMaterial_ShouldRejectMaterialUsedByPrimitive()
        {
            var repository = new CrossSectionRepository();
            var material = new Mock<IHeadMaterial>();
            material.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            var ndmElement = new Mock<INdmElement>();
            ndmElement.SetupProperty(x => x.HeadMaterial, material.Object);
            var primitive = new Mock<INdmPrimitive>();
            primitive.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            primitive.SetupGet(x => x.NdmElement).Returns(ndmElement.Object);

            repository.AddHeadMaterial(material.Object);
            repository.AddPrimitive(primitive.Object);

            Assert.Throws<StructureHelperException>(() => repository.RemoveHeadMaterial(material.Object));
            Assert.That(repository.RemovePrimitive(primitive.Object), Is.True);
            Assert.That(repository.RemoveHeadMaterial(material.Object), Is.True);
        }

        [Test]
        public void AddForceActionAndCalculator_ShouldRejectDuplicateIds()
        {
            var repository = new CrossSectionRepository();
            var actionId = Guid.NewGuid();
            var firstAction = new Mock<IForceAction>();
            var secondAction = new Mock<IForceAction>();
            firstAction.SetupGet(x => x.Id).Returns(actionId);
            secondAction.SetupGet(x => x.Id).Returns(actionId);

            repository.AddForceAction(firstAction.Object);
            Assert.Throws<StructureHelperException>(() => repository.AddForceAction(secondAction.Object));

            var calculatorId = Guid.NewGuid();
            var firstCalculator = new Mock<ICalculator>();
            var secondCalculator = new Mock<ICalculator>();
            firstCalculator.SetupGet(x => x.Id).Returns(calculatorId);
            secondCalculator.SetupGet(x => x.Id).Returns(calculatorId);

            repository.AddCalculator(firstCalculator.Object);
            Assert.Throws<StructureHelperException>(() => repository.AddCalculator(secondCalculator.Object));
        }

        [Test]
        public void DtoRepository_ShouldImplementMutationContract()
        {
            var repository = new CrossSectionRepositoryDTO(Guid.NewGuid());
            var material = new Mock<IHeadMaterial>();
            material.SetupGet(x => x.Id).Returns(Guid.NewGuid());

            repository.AddHeadMaterial(material.Object);

            Assert.That(repository.HeadMaterials, Has.Count.EqualTo(1));
            Assert.Throws<StructureHelperException>(() => repository.AddHeadMaterial(material.Object));
            Assert.That(repository.RemoveHeadMaterial(material.Object), Is.True);
            Assert.That(repository.RemoveHeadMaterial(material.Object), Is.False);
        }

        [Test]
        public void CrossSectionUpdate_ShouldPreserveTargetRepositoryIdAndCopyContents()
        {
            var source = new CrossSection(Guid.NewGuid());
            var sourceMaterial = new HeadMaterial(Guid.NewGuid())
            {
                Name = "Source material",
                HelperMaterial = new ElasticMaterial(Guid.NewGuid())
            };
            source.SectionRepository.AddHeadMaterial(sourceMaterial);

            var target = new CrossSection(Guid.NewGuid());
            target.SectionRepository.AddHeadMaterial(new HeadMaterial(Guid.NewGuid())
            {
                Name = "Old target material",
                HelperMaterial = new ElasticMaterial(Guid.NewGuid())
            });
            Guid targetRepositoryId = target.SectionRepository.Id;

            new CrossSectionUpdateStrategy().Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.SectionRepository.Id, Is.EqualTo(targetRepositoryId));
                Assert.That(target.SectionRepository.HeadMaterials, Has.Count.EqualTo(1));
                Assert.That(target.SectionRepository.HeadMaterials[0].Name, Is.EqualTo("Source material"));
                Assert.That(target.SectionRepository.HeadMaterials[0].Id, Is.Not.EqualTo(sourceMaterial.Id));
            });
        }
    }
}
