using DataAccess.DTOs;
using NUnit.Framework;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;

namespace StructureHelperTests.UnitTests
{
    [TestFixture]
    public class SaveableDtoConstructorTests
    {
        [Test]
        public void Point2DDto_ShouldKeepSuppliedId()
        {
            Guid id = Guid.NewGuid();
            var dto = new Point2DDTO(id);

            Assert.That(dto.Id, Is.EqualTo(id));
        }

        [Test]
        public void Point2DToDtoConverter_ShouldKeepSourceId()
        {
            Guid id = Guid.NewGuid();
            var source = new Point2D(id) { X = 1, Y = 2 };

            var dto = new Point2DToDTOConvertStrategy().Convert(source);

            Assert.That(dto.Id, Is.EqualTo(id));
        }

        [Test]
        public void Point2DFromDtoConverter_ShouldKeepSourceId()
        {
            Guid id = Guid.NewGuid();
            var dto = new Point2DDTO(id) { X = 1, Y = 2 };

            var result = new Point2DFromDTOConvertStrategy().GetNewItem(dto);

            Assert.That(result.Id, Is.EqualTo(id));
        }

        [Test]
        public void DivisionSizeFromDtoConverter_ShouldKeepSourceId()
        {
            Guid id = Guid.NewGuid();
            var dto = new DivisionSizeDTO(id)
            {
                NdmMaxSize = 0.02,
                NdmMinDivision = 20,
                ClearUnderlying = true
            };

            var result = new DivisionSizeFromDTOConvertStrategy(new DivisionSizeUpdateStrategy()).GetNewItem(dto);

            Assert.That(result.Id, Is.EqualTo(id));
        }

        [Test]
        public void MaterialSafetyFactorToDtoLogic_ShouldKeepIds()
        {
            Guid safetyFactorId = Guid.NewGuid();
            Guid partialFactorId = Guid.NewGuid();
            var safetyFactor = new StructureHelperCommon.Models.Materials.Libraries.MaterialSafetyFactor(safetyFactorId);
            var partialFactor = new StructureHelperCommon.Models.Materials.Libraries.MaterialPartialFactor(partialFactorId);
            var logic = new MaterialSafetyFactorToDTOLogic();

            var safetyFactorDto = logic.GetNewSafetyFactorByOld(safetyFactor);
            var partialFactorDto = logic.GetNewPartialFactorByOld(partialFactor);

            Assert.Multiple(() =>
            {
                Assert.That(safetyFactorDto.Id, Is.EqualTo(safetyFactorId));
                Assert.That(partialFactorDto.Id, Is.EqualTo(partialFactorId));
            });
        }
    }
}
