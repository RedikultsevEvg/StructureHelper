using LoaderCalculator.Data.Materials;
using NUnit.Framework;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Windows.Media;

namespace StructureHelperTests.UnitTests.UpdateStrategiesTests
{


    [TestFixture]
    public class HeadMaterialUpdateStrategyTests
    {
        private HeadMaterialUpdateStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _strategy = new HeadMaterialUpdateStrategy(
                new MockBaseUpdateStrategy(),
                new MockHelperMaterialUpdateStrategy()
            );
        }

        [Test]
        public void Update_ShouldThrowException_WhenTargetObjectIsNull()
        {
            var sourceObject = new MockHeadMaterial();

            Assert.Throws<StructureHelperException>(() => _strategy.Update(null, sourceObject));
        }

        [Test]
        public void Update_ShouldThrowException_WhenSourceObjectIsNull()
        {
            var targetObject = new MockHeadMaterial();

            Assert.Throws<StructureHelperException>(() => _strategy.Update(targetObject, null));
        }

        [Test]
        public void Update_ShouldNotThrow_WhenTargetAndSourceAreSameReference()
        {
            var targetAndSource = new MockHeadMaterial();

            Assert.DoesNotThrow(() => _strategy.Update(targetAndSource, targetAndSource));
        }

        [Test]
        public void Update_ShouldUpdateBaseProperties()
        {
            // Arrange
            var targetObject = new MockHeadMaterial { Name = "Original", Color = Color.FromRgb(0,0,0) };
            var sourceObject = new MockHeadMaterial { Name = "Updated", Color = Color.FromRgb(255, 0, 0), HelperMaterial = new MockHelperMaterial() };

            // Act
            _strategy.Update(targetObject, sourceObject);

            // Assert
            Assert.That(targetObject.Name, Is.EqualTo("Updated"), "Name should be updated");
            Assert.That(targetObject.Color, Is.EqualTo(Color.FromRgb(255, 0, 0)), "Color should be updated");
        }

        [Test]
        public void Update_ShouldCloneAndUpdateHelperMaterial()
        {
            // Arrange
            MockHeadMaterial targetObject = new (){ HelperMaterial = new MockHelperMaterial { Property = "Original" } };
            var sourceObject = new MockHeadMaterial { HelperMaterial = new MockHelperMaterial { Property = "Updated" } };

            // Act
            _strategy.Update(targetObject, sourceObject);

            // Assert
            Assert.That((targetObject.HelperMaterial as MockHelperMaterial).Property, Is.EqualTo("Updated"), "HelperMaterial property should be updated");
            Assert.That(targetObject.HelperMaterial, Is.Not.SameAs(sourceObject.HelperMaterial),
                "HelperMaterial should be cloned, not directly assigned");
        }

        #region Mocks
        private class MockHeadMaterial : IHeadMaterial
        {
            public string Name { get; set; }
            public Color Color { get; set; }
            public IHelperMaterial HelperMaterial { get; set; }

            public Guid Id => throw new NotImplementedException();

            public object Clone()
            {
                return new MockHeadMaterial
                {
                    Name = this.Name,
                    Color = this.Color,
                    HelperMaterial = this.HelperMaterial?.Clone() as IHelperMaterial
                };
            }

            public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
            {
                throw new NotImplementedException();
            }

            public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
            {
                throw new NotImplementedException();
            }
        }

        private class MockHelperMaterial : IHelperMaterial
        {
            public string Property { get; set; }

            public Guid Id => throw new NotImplementedException();

            public List<IMaterialSafetyFactor> SafetyFactors { get; set; }

            public object Clone()
            {
                return new MockHelperMaterial { Property = this.Property };
            }

            public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
            {
                throw new NotImplementedException();
            }

            public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
            {
                throw new NotImplementedException();
            }
        }

        private class MockBaseUpdateStrategy : IUpdateStrategy<IHeadMaterial>
        {
            public void Update(IHeadMaterial targetObject, IHeadMaterial sourceObject)
            {
                if (targetObject == null || sourceObject == null) return;

                targetObject.Name = sourceObject.Name;
                targetObject.Color = sourceObject.Color;
            }
        }

        private class MockHelperMaterialUpdateStrategy : IUpdateStrategy<IHelperMaterial>
        {
            public void Update(IHelperMaterial targetObject, IHelperMaterial sourceObject)
            {
                if (targetObject != null && sourceObject != null)
                {
                    (targetObject as MockHelperMaterial).Property = (sourceObject as MockHelperMaterial).Property;
                }
            }
        }
        #endregion
    }

}
