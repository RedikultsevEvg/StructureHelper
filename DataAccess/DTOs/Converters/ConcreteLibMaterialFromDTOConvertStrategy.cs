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
    public class ConcreteLibMaterialFromDTOConvertStrategy : ConvertStrategy<ConcreteLibMaterial, ConcreteLibMaterialDTO>
    {
        private readonly IUpdateStrategy<IConcreteLibMaterial> updateStrategy;

        public ConcreteLibMaterialFromDTOConvertStrategy(IUpdateStrategy<IConcreteLibMaterial> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ConcreteLibMaterialFromDTOConvertStrategy() : this (new ConcreteLibUpdateStrategy())
        {
            
        }

        public override ConcreteLibMaterial GetNewItem(ConcreteLibMaterialDTO source)
        {
            TraceLogger?.AddMessage("Concrete library material converting is started", TraceLogStatuses.Service);
            ConcreteLibMaterial newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            TraceLogger?.AddMessage("Concrete library material converting has been finished succesfully", TraceLogStatuses.Service);
            return newItem;
        }
    }
}
