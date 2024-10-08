using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ReinforcementLibMaterialToDTOConvertStrategy : LibMaterialToDTOConvertStrategy<ReinforcementLibMaterialDTO, IReinforcementLibMaterial>
    {
        public override IUpdateStrategy<IReinforcementLibMaterial> UpdateStrategy { get; } = new ReinforcementLibUpdateStrategy();

        public override ReinforcementLibMaterialDTO GetMaterialDTO(IReinforcementLibMaterial source)
        {
            ReinforcementLibMaterialDTO newItem = new()
            {
                Id = source.Id
            };
            return newItem;
        }
    }
}
