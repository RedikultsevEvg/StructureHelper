using NUnit.Framework;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;

namespace StructureHelperTests.UnitTests.CrossSections
{
    [TestFixture]
    public class CollectionCloneStrategyTests
    {
        [Test]
        public void Repository_PreserveReference_ShouldCreateIndependentGraphWithSharedMaterial()
        {
            var source = new CrossSectionRepository(Guid.NewGuid());
            var material = new HeadMaterial(Guid.NewGuid())
            {
                Name = "Shared material",
                HelperMaterial = new ElasticMaterial(Guid.NewGuid())
            };
            var primitive = new PointNdmPrimitive(Guid.NewGuid());
            primitive.NdmElement.HeadMaterial = material;
            source.AddHeadMaterial(material);
            source.AddPrimitive(primitive);

            var strategy = new CrossSectionRepositoryCloneStrategy(new DeepCloningStrategy());
            var clone = strategy.GetClone(source, CloneMode.NewEntity, CloneDepth.PreserveReference);

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.SameAs(source));
                Assert.That(clone.HeadMaterials, Has.Count.EqualTo(1));
                Assert.That(clone.Primitives, Has.Count.EqualTo(1));
                Assert.That(clone.HeadMaterials[0], Is.Not.SameAs(material));
                Assert.That(clone.Primitives[0], Is.Not.SameAs(primitive));
                Assert.That(clone.Primitives[0].NdmElement.HeadMaterial, Is.SameAs(clone.HeadMaterials[0]));
                Assert.That(clone.HeadMaterials[0].Id, Is.Not.EqualTo(material.Id));
            });
        }

        [Test]
        public void Repository_PreserveReference_ShouldRewireCalculatorInputToClonedItems()
        {
            var source = new CrossSectionRepository(Guid.NewGuid());
            var material = new HeadMaterial(Guid.NewGuid())
            {
                HelperMaterial = new ElasticMaterial(Guid.NewGuid())
            };
            var primitive = new PointNdmPrimitive(Guid.NewGuid());
            primitive.NdmElement.HeadMaterial = material;
            var action = new ForceCombinationList(Guid.NewGuid());
            var calculator = new ForceCalculator(Guid.NewGuid());
            var input = new ForceCalculatorInputData(Guid.NewGuid());
            input.Primitives.Add(primitive);
            input.ForceActions.Add(action);
            calculator.InputData = input;

            source.AddHeadMaterial(material);
            source.AddPrimitive(primitive);
            source.AddForceAction(action);
            source.AddCalculator(calculator);

            var strategy = new CrossSectionRepositoryCloneStrategy(new DeepCloningStrategy());
            var clone = strategy.GetClone(source, CloneMode.NewEntity, CloneDepth.PreserveReference);

            Assert.Multiple(() =>
            {
                Assert.That(clone.Calculators, Has.Count.EqualTo(1));
                var clonedCalculator = (ForceCalculator)clone.Calculators[0];
                Assert.That(clonedCalculator.InputData.Primitives[0], Is.SameAs(clone.Primitives[0]));
                Assert.That(clonedCalculator.InputData.ForceActions[0], Is.SameAs(clone.ForceActions[0]));
                Assert.That(clonedCalculator.InputData.Primitives[0].NdmElement.HeadMaterial, Is.SameAs(clone.HeadMaterials[0]));
            });
        }

        [Test]
        public void ForceActionsCollection_ShouldCloneItemsAndPreserveMode()
        {
            var source = new CrossSectionRepository(Guid.NewGuid());
            var sourceAction = new ForceCombinationList(Guid.NewGuid()) { Name = "Action" };
            source.AddForceAction(sourceAction);
            var target = new CrossSectionRepository(Guid.NewGuid());
            var strategy = new HasForceActionsCloneStrategy(new DeepCloningStrategy());

            strategy.GetClone(target, source, CloneMode.NewEntity);

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

            strategy.GetClone(target, source, CloneMode.PreserveIdentity);

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

            strategy.GetClone(target, source, CloneMode.Reference);

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

            strategy.GetClone(target, source, CloneMode.PreserveIdentity);

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

            strategy.GetClone(target, source, CloneMode.NewEntity);

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
