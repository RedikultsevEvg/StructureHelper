using Moq;
using NUnit.Framework;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.CrossSections
{
    [TestFixture]
    public class RepositoryActionOperationTests
    {
        [Test]
        public void Remove_ShouldRemoveActionFromCalculatorAndRepository()
        {
            var action = new Mock<IForceAction>();
            action.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            var actions = new List<IForceAction> { action.Object };
            var inputData = new Mock<IForceCalculatorInputData>();
            inputData.SetupGet(x => x.ForceActions).Returns(actions);
            var calculator = new Mock<IForceCalculator>();
            calculator.SetupGet(x => x.InputData).Returns(inputData.Object);
            var repository = new CrossSectionRepository();
            repository.AddForceAction(action.Object);
            repository.AddCalculator(calculator.Object);

            var operation = new RepositoryActionOperations(repository);
            operation.Remove(action.Object);

            Assert.Multiple(() =>
            {
                Assert.That(repository.ForceActions, Is.Empty);
                Assert.That(actions, Is.Empty);
            });
        }

        [Test]
        public void RemoveAll_ShouldClearActionsInCalculatorAndRepository()
        {
            var action = new Mock<IForceAction>();
            action.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            var actions = new List<IForceAction> { action.Object };
            var inputData = new Mock<IForceCalculatorInputData>();
            inputData.SetupGet(x => x.ForceActions).Returns(actions);
            var calculator = new Mock<IForceCalculator>();
            calculator.SetupGet(x => x.InputData).Returns(inputData.Object);
            var repository = new CrossSectionRepository();
            repository.AddForceAction(action.Object);
            repository.AddCalculator(calculator.Object);

            var operation = new RepositoryActionOperations(repository);
            operation.RemoveAll();

            Assert.Multiple(() =>
            {
                Assert.That(repository.ForceActions, Is.Empty);
                Assert.That(actions, Is.Empty);
            });
        }

        [Test]
        public void ClearAll_ShouldClearAllRepositoryCollections()
        {
            var repository = new CrossSectionRepository();
            var material = new Mock<IHeadMaterial>();
            material.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            var action = new Mock<IForceAction>();
            action.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            var primitive = new Mock<INdmPrimitive>();
            primitive.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            var calculator = new Mock<ILimitCurvesCalculator>();
            calculator.SetupGet(x => x.Id).Returns(Guid.NewGuid());

            repository.AddHeadMaterial(material.Object);
            repository.AddForceAction(action.Object);
            repository.AddPrimitive(primitive.Object);
            repository.AddCalculator(calculator.Object);

            repository.ClearAll();

            Assert.Multiple(() =>
            {
                Assert.That(repository.HeadMaterials, Is.Empty);
                Assert.That(repository.ForceActions, Is.Empty);
                Assert.That(repository.Primitives, Is.Empty);
                Assert.That(repository.Calculators, Is.Empty);
            });
        }
    }
}
