using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.Models.Materials.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    internal class HelperMaterialToDTOConvertStrategy : IConvertStrategy<IHelperMaterial, IHelperMaterial>
    {
        private LibMaterialToDTOConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial> concreteConvertStrategy;
        private LibMaterialToDTOConvertStrategy<ReinforcementLibMaterialDTO, IReinforcementLibMaterial> reinforcementConvertStrategy;
        private IConvertStrategy<ElasticMaterialDTO, IElasticMaterial> elasticConvertStrategy;
        private IConvertStrategy<FRMaterialDTO, IFRMaterial> frMaterialConvertStrategy;
        private IUpdateStrategy<IHelperMaterial> safetyFactorUpdateStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy();

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public HelperMaterialToDTOConvertStrategy(
            LibMaterialToDTOConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial> concreteConvertStrategy,
            LibMaterialToDTOConvertStrategy<ReinforcementLibMaterialDTO, IReinforcementLibMaterial> reinforcementConvertStrategy,
            IConvertStrategy<ElasticMaterialDTO, IElasticMaterial> elasticConvertStrategy,
            IConvertStrategy<FRMaterialDTO, IFRMaterial> frMaterialConvertStrategy)
        {
            this.concreteConvertStrategy = concreteConvertStrategy;
            this.reinforcementConvertStrategy = reinforcementConvertStrategy;
            this.elasticConvertStrategy = elasticConvertStrategy;
            this.frMaterialConvertStrategy = frMaterialConvertStrategy;
        }

        public HelperMaterialToDTOConvertStrategy() : this (
            new ConcreteLibMaterialToDTOConvertStrategy(),
            new ReinforcementLibMaterialToDTOConvertStrategy(),
            new ElasticMaterialToDTOConvertStrategy(),
            new FRMaterialToDTOConvertStrategy()
            )
        {
            
        }

        public IHelperMaterial Convert(IHelperMaterial source)
        {
            Check();
            try
            {
                IHelperMaterial helperMaterial = GetMaterial(source);
                safetyFactorUpdateStrategy.Update(helperMaterial, source);
                return helperMaterial;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private IHelperMaterial GetMaterial(IHelperMaterial source)
        {
            if (source is IConcreteLibMaterial concreteLibMaterial)
            {
                return ProcessConcrete(concreteLibMaterial);
            }
            else if (source is IReinforcementLibMaterial reinforcementMaterial)
            {
                return ProcessReinforcement(reinforcementMaterial);
            }
            else if (source is IFRMaterial frMaterial)
            {
                return ProcessFRMaterial(frMaterial);
            }
            else if (source is IElasticMaterial elasticMaterial)
            {
                return ProcessElastic(elasticMaterial);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source));
            }
        }

        private IHelperMaterial ProcessFRMaterial(IFRMaterial frMaterial)
        {
            frMaterialConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            frMaterialConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<FRMaterialDTO, IFRMaterial>(this, frMaterialConvertStrategy);
            return convertLogic.Convert(frMaterial);
        }

        private IHelperMaterial ProcessElastic(IElasticMaterial elasticMaterial)
        {
            elasticConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            elasticConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ElasticMaterialDTO, IElasticMaterial>(this, elasticConvertStrategy);
            return convertLogic.Convert(elasticMaterial);
        }

        private IHelperMaterial ProcessReinforcement(IReinforcementLibMaterial reinforcementMaterial)
        {
            reinforcementConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            reinforcementConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ReinforcementLibMaterialDTO, IReinforcementLibMaterial>(this, reinforcementConvertStrategy);
            return convertLogic.Convert(reinforcementMaterial);
        }

        private IHelperMaterial ProcessConcrete(IConcreteLibMaterial concreteLibMaterial)
        {
            concreteConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            concreteConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial>(this, concreteConvertStrategy);
            return convertLogic.Convert(concreteLibMaterial);
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<IHelperMaterial, IHelperMaterial>(this);
            checkLogic.Check();
        }
    }
}
