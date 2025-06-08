using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ElasticMaterialToDTOConvertStrategy : IConvertStrategy<ElasticMaterialDTO, IElasticMaterial>
    {
        private IUpdateStrategy<IElasticMaterial> updateStrategy;
        private ICheckConvertLogic<ElasticMaterialDTO, IElasticMaterial> checkLogic;

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ElasticMaterialToDTOConvertStrategy(
            IUpdateStrategy<IElasticMaterial> updateStrategy,
            ICheckConvertLogic<ElasticMaterialDTO, IElasticMaterial> checkLogic)
        {
            this.updateStrategy = updateStrategy;
            this.checkLogic = checkLogic;
        }

        public ElasticMaterialToDTOConvertStrategy() : this (
            new ElasticUpdateStrategy(),
            new CheckConvertLogic<ElasticMaterialDTO, IElasticMaterial>())
        {
            
        }

        public ElasticMaterialDTO Convert(IElasticMaterial source)
        {
            Check();
            try
            {
                ElasticMaterialDTO newItem = new() { Id = source.Id };
                updateStrategy.Update(newItem, source);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }

        }

        private void Check()
        {
            checkLogic = new CheckConvertLogic<ElasticMaterialDTO, IElasticMaterial>(this);
            checkLogic.Check();
        }
    }
}
