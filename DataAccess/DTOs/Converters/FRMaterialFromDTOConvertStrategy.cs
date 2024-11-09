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
    public class FRMaterialFromDTOConvertStrategy : ConvertStrategy<FRMaterial, FRMaterialDTO>
    {
        private IUpdateStrategy<IFRMaterial> updateStrategy;

        public FRMaterialFromDTOConvertStrategy(IUpdateStrategy<IFRMaterial> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public FRMaterialFromDTOConvertStrategy() : this(new FRUpdateStrategy())
        {
            
        }

        public override FRMaterial GetNewItem(FRMaterialDTO source)
        {
            TraceLogger?.AddMessage("Fiber reinforcement material converting is started", TraceLogStatuses.Service);
            FRMaterial newItem = new(source.MaterialType, source.Id);
            updateStrategy.Update(newItem, source);
            TraceLogger?.AddMessage("FiberReinforcement material converting has been finished successfully", TraceLogStatuses.Service);
            return newItem;
        }
    }
}
