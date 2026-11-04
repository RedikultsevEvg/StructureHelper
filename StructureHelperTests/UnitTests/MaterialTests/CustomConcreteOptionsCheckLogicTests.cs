using NUnit.Framework;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using System;

namespace StructureHelperTests.UnitTests.MaterialTests
{
    [TestFixture]
    public class CustomConcreteCompressionOptionsCheckLogicTests
    {
        [Test]
        public void Check_DefaultOptions_ReturnsTrue()
        {
            var logic = new CustomConcreteCompressionOptionsCheckLogic
            {
                Entity = new CustomConcreteCompressionOptions(Guid.NewGuid())
            };

            Assert.That(logic.Check(), Is.True);
        }

        [TestCase(0d)]
        [TestCase(-1d)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void Check_PeakStressNotPositiveFinite_ReturnsFalse(double value)
        {
            var options = new CustomConcreteCompressionOptions(Guid.NewGuid())
            {
                PeakStress = value
            };
            var logic = new CustomConcreteCompressionOptionsCheckLogic { Entity = options };

            Assert.Multiple(() =>
            {
                Assert.That(logic.Check(), Is.False);
                Assert.That(logic.CheckResult, Is.Not.Empty);
            });
        }

        [TestCase(0d)]
        [TestCase(-0.1d)]
        [TestCase(double.NaN)]
        public void Check_PeakStrainNotPositiveFinite_ReturnsFalse(double value)
        {
            var options = new CustomConcreteCompressionOptions(Guid.NewGuid())
            {
                PeakStrain = value
            };
            var logic = new CustomConcreteCompressionOptionsCheckLogic { Entity = options };

            Assert.That(logic.Check(), Is.False);
        }

        [TestCase(0d)]
        [TestCase(-0.1d)]
        [TestCase(double.NaN)]
        public void Check_LimitStrainNotPositiveFinite_ReturnsFalse(double value)
        {
            var options = new CustomConcreteCompressionOptions(Guid.NewGuid())
            {
                LimitStrain = value
            };
            var logic = new CustomConcreteCompressionOptionsCheckLogic { Entity = options };

            Assert.That(logic.Check(), Is.False);
        }

        [Test]
        public void Check_LimitStrainNotGreaterThanPeakStrain_ReturnsFalse()
        {
            var options = new CustomConcreteCompressionOptions(Guid.NewGuid())
            {
                PeakStrain = 0.002,
                LimitStrain = 0.002
            };
            var logic = new CustomConcreteCompressionOptionsCheckLogic { Entity = options };

            Assert.That(logic.Check(), Is.False);
        }

        [Test]
        public void Check_DescendingScaleFactorNotPositive_ReturnsFalse()
        {
            var options = new CustomConcreteCompressionOptions(Guid.NewGuid())
            {
                DescendingScaleFactor = 0
            };
            var logic = new CustomConcreteCompressionOptionsCheckLogic { Entity = options };

            Assert.That(logic.Check(), Is.False);
        }

        [Test]
        public void Check_NullEntity_ReturnsFalse()
        {
            var logic = new CustomConcreteCompressionOptionsCheckLogic();

            Assert.Multiple(() =>
            {
                Assert.That(logic.Check(), Is.False);
                Assert.That(logic.CheckResult, Is.Not.Empty);
            });
        }
    }

    [TestFixture]
    public class CustomConcreteTensionOptionsCheckLogicTests
    {
        [Test]
        public void Check_DefaultOptions_ReturnsTrue()
        {
            var logic = new CustomConcreteTensionOptionsCheckLogic
            {
                Entity = new CustomConcreteTensionOptions(Guid.NewGuid())
            };

            Assert.That(logic.Check(), Is.True);
        }

        [TestCase(0d)]
        [TestCase(-1d)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void Check_StrengthNotPositiveFinite_ReturnsFalse(double value)
        {
            var options = new CustomConcreteTensionOptions(Guid.NewGuid())
            {
                Strength = value
            };
            var logic = new CustomConcreteTensionOptionsCheckLogic { Entity = options };

            Assert.That(logic.Check(), Is.False);
        }

        [TestCase(0d)]
        [TestCase(-0.1d)]
        [TestCase(double.NaN)]
        public void Check_UltimateStrainNotPositiveFinite_ReturnsFalse(double value)
        {
            var options = new CustomConcreteTensionOptions(Guid.NewGuid())
            {
                UltimateStrain = value
            };
            var logic = new CustomConcreteTensionOptionsCheckLogic { Entity = options };

            Assert.That(logic.Check(), Is.False);
        }

        [TestCase(0d)]
        [TestCase(-0.1d)]
        [TestCase(double.NaN)]
        public void Check_LimitStrainNotPositive_ReturnsFalse(double value)
        {
            var options = new CustomConcreteTensionOptions(Guid.NewGuid())
            {
                LimitStrain = value
            };
            var logic = new CustomConcreteTensionOptionsCheckLogic { Entity = options };

            Assert.That(logic.Check(), Is.False);
        }

        [Test]
        public void Check_PositiveInfinityLimitStrain_ReturnsTrue()
        {
            var options = new CustomConcreteTensionOptions(Guid.NewGuid())
            {
                LimitStrain = double.PositiveInfinity
            };
            var logic = new CustomConcreteTensionOptionsCheckLogic { Entity = options };

            Assert.That(logic.Check(), Is.True);
        }

        [Test]
        public void Check_ZeroStrength_ReturnsFalse()
        {
            var options = new CustomConcreteTensionOptions(Guid.NewGuid())
            {
                Strength = 0
            };
            var logic = new CustomConcreteTensionOptionsCheckLogic { Entity = options };

            Assert.That(logic.Check(), Is.False);
        }

        [Test]
        public void Check_NullEntity_ReturnsFalse()
        {
            var logic = new CustomConcreteTensionOptionsCheckLogic();

            Assert.That(logic.Check(), Is.False);
        }
    }
}
