using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ReinforcementLibMaterialFromDTOConvertStrategy : ConvertStrategy<ReinforcementLibMaterial, ReinforcementLibMaterialDTO>
    {
        private readonly IUpdateStrategy<IReinforcementLibMaterial> updateStrategy;

        public ReinforcementLibMaterialFromDTOConvertStrategy(IUpdateStrategy<IReinforcementLibMaterial> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ReinforcementLibMaterialFromDTOConvertStrategy() : this(new ReinforcementLibUpdateStrategy())
        {
            
        }

        public override ReinforcementLibMaterial GetNewItem(ReinforcementLibMaterialDTO source)
        {
            TraceLogger?.AddMessage("Reinforcement library material converting is started", TraceLogStatuses.Service);
            ReinforcementLibMaterial newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            TraceLogger?.AddMessage("Reinforcement library material converting has been finished successfully", TraceLogStatuses.Service);
            return newItem;
        }
    }
}
