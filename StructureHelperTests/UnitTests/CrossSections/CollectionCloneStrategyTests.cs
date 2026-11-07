using NUnit.Framework;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;

namespace StructureHelperTests.UnitTests.CrossSections
{
    [TestFixture]
    public class CollectionCloneStrategyTests
    {
        [Test]
        public void ForceActionsCollection_ShouldCloneItemsAndPreserveMode()
        {
            var source = new CrossSectionRepository(Guid.NewGuid());
            var sourceAction = new ForceCombinationList(Guid.NewGuid()) { Name = "Action" };
            source.AddForceAction(sourceAction);
            var target = new CrossSectionRepository(Guid.NewGuid());
            var strategy = new HasForceActionsCloneStrategy(new DeepCloningStrategy());

            strategy.GetClone(target, source, CloningMode.NewEntity);

            Assert.Multiple(() =>
            {
                Assert.That(target.ForceActions, Has.Count.EqualTo(1));
                Assert.That(target.ForceActions[0].Name, Is.EqualTo("Action"));
                Assert.That(target.ForceActions[0].Id, Is.Not.EqualTo(sourceAction.Id));
            });
        }

        [Test]
        public void ForceActionsCollection_PreserveIdentity_ShouldPreserveNestedIds()
        {
            var source = new CrossSectionRepository(Guid.NewGuid());
            var action = new ForceCombinationList(Guid.NewGuid());
            source.AddForceAction(action);
            var target = new CrossSectionRepository(Guid.NewGuid());
            var strategy = new HasForceActionsCloneStrategy(new DeepCloningStrategy());

            strategy.GetClone(target, source, CloningMode.PreserveIdentity);

            var clonedAction = (ForceCombinationList)target.ForceActions[0];
            Assert.Multiple(() =>
            {
                Assert.That(clonedAction.Id, Is.EqualTo(action.Id));
                Assert.That(clonedAction.DesignForces, Has.Count.EqualTo(action.DesignForces.Count));
                for (int i = 0; i < action.DesignForces.Count; i++)
                {
                    Assert.That(clonedAction.DesignForces[i].Id, Is.EqualTo(action.DesignForces[i].Id));
                    Assert.That(clonedAction.DesignForces[i].ForceTuple.Id, Is.EqualTo(action.DesignForces[i].ForceTuple.Id));
                }
            });
        }

        [Test]
        public void ForceActionsCollection_ReferenceMode_ShouldUseSourceItem()
        {
            var source = new CrossSectionRepository(Guid.NewGuid());
            var sourceAction = new ForceCombinationList(Guid.NewGuid());
            source.AddForceAction(sourceAction);
            var target = new CrossSectionRepository(Guid.NewGuid());
            var strategy = new HasForceActionsCloneStrategy(new DeepCloningStrategy());

            strategy.GetClone(target, source, CloningMode.Reference);

            Assert.That(target.ForceActions[0], Is.SameAs(sourceAction));
        }

        [Test]
        public void HeadMaterialsCollection_ShouldCloneMaterial()
        {
            var source = new CrossSectionRepository(Guid.NewGuid());
            var material = new HeadMaterial(Guid.NewGuid())
            {
                Name = "Material",
                HelperMaterial = new ElasticMaterial(Guid.NewGuid())
            };
            source.AddHeadMaterial(material);
            var target = new CrossSectionRepository(Guid.NewGuid());
            var strategy = new HasHeadMaterialsCloneStrategy(new DeepCloningStrategy());

            strategy.GetClone(target, source, CloningMode.PreserveIdentity);

            Assert.Multiple(() =>
            {
                Assert.That(target.HeadMaterials, Has.Count.EqualTo(1));
                Assert.That(target.HeadMaterials[0].Id, Is.EqualTo(material.Id));
                Assert.That(target.HeadMaterials[0].Name, Is.EqualTo("Material"));
            });
        }

        [Test]
        public void PrimitivesCollection_ShouldCloneMaterialReference()
        {
            var source = new CrossSectionRepository(Guid.NewGuid());
            var material = new HeadMaterial(Guid.NewGuid())
            {
                HelperMaterial = new ElasticMaterial(Guid.NewGuid())
            };
            var primitive = new PointNdmPrimitive(Guid.NewGuid());
            primitive.NdmElement.HeadMaterial = material;
            source.AddPrimitive(primitive);
            var target = new CrossSectionRepository(Guid.NewGuid());
            var strategy = new HasPrimitivesCloneStrategy(new DeepCloningStrategy());

            strategy.GetClone(target, source, CloningMode.NewEntity);

            Assert.Multiple(() =>
            {
                Assert.That(target.Primitives, Has.Count.EqualTo(1));
                Assert.That(target.Primitives[0].Id, Is.Not.EqualTo(primitive.Id));
                Assert.That(target.Primitives[0].NdmElement.HeadMaterial, Is.Not.Null);
                Assert.That(target.Primitives[0].NdmElement.HeadMaterial!.Id, Is.Not.EqualTo(material.Id));
            });
        }
    }
}
