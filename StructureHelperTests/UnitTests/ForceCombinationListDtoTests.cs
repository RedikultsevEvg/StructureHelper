using DataAccess.DTOs;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System;

namespace StructureHelperTests.UnitTests
{
    [TestFixture]
    public class ForceCombinationListDtoTests
    {
        [Test]
        public void Clone_ShouldCopyCombinationAndDesignForces()
        {
            Guid id = Guid.NewGuid();
            var source = new ForceCombinationListDTO(id)
            {
                Name = "Combination",
                SetInGravityCenter = true,
                ForcePoint = new Point2DDTO(Guid.NewGuid()) { X = 1, Y = 2 },
                DesignForces =
                {
                    new DesignForceTupleDTO(Guid.NewGuid())
                    {
                        LimitState = StructureHelperCommon.Infrastructures.Enums.LimitStates.ULS,
                        CalcTerm = StructureHelperCommon.Infrastructures.Enums.CalcTerms.ShortTerm,
                        ForceTuple = new ForceTupleDTO(Guid.NewGuid()) { Nz = 100d }
                    }
                }
            };

            var clone = (ForceCombinationListDTO)source.GetClone(source, CloneMode.NewEntity, CloneDepth.Deep);

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.SameAs(source));
                Assert.That(clone.Id, Is.EqualTo(source.Id));
                Assert.That(clone.Name, Is.EqualTo(source.Name));
                Assert.That(clone.SetInGravityCenter, Is.True);
                Assert.That(clone.DesignForces, Has.Count.EqualTo(1));
                Assert.That(clone.DesignForces[0], Is.Not.SameAs(source.DesignForces[0]));
                Assert.That(clone.DesignForces[0].ForceTuple.Nz, Is.EqualTo(100d));
            });
        }

        [Test]
        public void ForceFactoredListClone_ShouldCopyTuplesAndCombinationProperty()
        {
            Guid id = Guid.NewGuid();
            var source = new ForceFactoredListDTO(id)
            {
                Name = "Factored list",
                SetInGravityCenter = true,
                ForcePoint = new Point2DDTO(Guid.NewGuid()) { X = 3, Y = 4 },
                CombinationProperty = new FactoredCombinationPropertyDTO(Guid.NewGuid())
                {
                    LimitState = StructureHelperCommon.Infrastructures.Enums.LimitStates.ULS,
                    CalcTerm = StructureHelperCommon.Infrastructures.Enums.CalcTerms.ShortTerm,
                    ULSFactor = 1.2,
                    LongTermFactor = 0.9
                }
            };
            source.ForceTuples.Add(new ForceTupleDTO(Guid.NewGuid()) { Nz = 200d });

            var clone = (ForceFactoredListDTO)source.GetClone(source, CloneMode.NewEntity, CloneDepth.Deep);

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.SameAs(source));
                Assert.That(clone.Id, Is.EqualTo(source.Id));
                Assert.That(clone.Name, Is.EqualTo(source.Name));
                Assert.That(clone.ForceTuples, Has.Count.EqualTo(1));
                Assert.That(clone.ForceTuples[0].Nz, Is.EqualTo(200d));
                Assert.That(clone.CombinationProperty, Is.Not.SameAs(source.CombinationProperty));
                Assert.That(clone.CombinationProperty.ULSFactor, Is.EqualTo(1.2));
            });
        }
    }
}
