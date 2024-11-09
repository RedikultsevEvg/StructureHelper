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
    public class ElasticMaterialFromDTOConvertStrategy : ConvertStrategy<ElasticMaterial, ElasticMaterialDTO>
    {
        private readonly IUpdateStrategy<IElasticMaterial> updateStrategy;

        public ElasticMaterialFromDTOConvertStrategy(IUpdateStrategy<IElasticMaterial> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ElasticMaterialFromDTOConvertStrategy() : this (new ElasticUpdateStrategy())
        {
            
        }

        public override ElasticMaterial GetNewItem(ElasticMaterialDTO source)
        {
            TraceLogger?.AddMessage("Elastic material converting is started", TraceLogStatuses.Service);
            ElasticMaterial newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            TraceLogger?.AddMessage("Elastic material converting has been finished successfully", TraceLogStatuses.Service);
            return newItem;
        }
    }
}
