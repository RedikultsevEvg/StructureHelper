using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ConcreteLibMaterialToDTOConvertStrategy : LibMaterialToDTOConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial>
    {
        public override IUpdateStrategy<IConcreteLibMaterial> UpdateStrategy { get; } = new ConcreteLibUpdateStrategy();

        public override ConcreteLibMaterialDTO GetMaterialDTO(IConcreteLibMaterial source)
        {
            ConcreteLibMaterialDTO newItem = new()
            {
                Id = source.Id
            };
            return newItem;
        }
    }
}
