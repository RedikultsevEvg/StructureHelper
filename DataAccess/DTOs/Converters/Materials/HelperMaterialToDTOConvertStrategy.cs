using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class HelperMaterialToDTOConvertStrategy : IConvertStrategy<IHelperMaterial, IHelperMaterial>
    {
        private IHelperMaterialToDTOStrategyContainer strategyContainer;
        private IHelperMaterialToDTOStrategyContainer StrategyContainer => strategyContainer ??= new HelperMaterialToDTOStrategyContainer();

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public HelperMaterialToDTOConvertStrategy(IHelperMaterialToDTOStrategyContainer strategyContainer)
        {
            this.strategyContainer = strategyContainer;
        }

        public HelperMaterialToDTOConvertStrategy()
        {
            
        }

        public IHelperMaterial Convert(IHelperMaterial source)
        {
            Check();
            try
            {
                IHelperMaterial helperMaterial = GetMaterial(source);
                StrategyContainer.SafetyFactorUpdateStrategy.Update(helperMaterial, source);
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
            else if (source is ISteelLibMaterial steelLibMaterial)
            {
                return ProcessSteel(steelLibMaterial);
            }
            else if (source is IUserMaterial userMaterial)
            {
                return GetUserMaterial(userMaterial);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source));
            }
        }

        private IHelperMaterial GetUserMaterial(IUserMaterial source)
        {
            var strategy = StrategyContainer.UserMaterialConvertStrategy;
            strategy.ReferenceDictionary = ReferenceDictionary;
            strategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<UserMaterialDTO, IUserMaterial>(this, strategy);
            return convertLogic.Convert(source);
        }

        private IHelperMaterial ProcessSteel(ISteelLibMaterial source)
        {
            var strategy = StrategyContainer.SteelConvertStrategy;
            strategy.ReferenceDictionary = ReferenceDictionary;
            strategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<SteelLibMaterialDTO, ISteelLibMaterial>(this, strategy);
            return convertLogic.Convert(source);
        }

        private IHelperMaterial ProcessFRMaterial(IFRMaterial frMaterial)
        {
            var frMaterialConvertStrategy = StrategyContainer.FrMaterialConvertStrategy;
            frMaterialConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            frMaterialConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<FRMaterialDTO, IFRMaterial>(this, frMaterialConvertStrategy);
            return convertLogic.Convert(frMaterial);
        }

        private IHelperMaterial ProcessElastic(IElasticMaterial elasticMaterial)
        {
            var elasticConvertStrategy = StrategyContainer.ElasticConvertStrategy;
            elasticConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            elasticConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ElasticMaterialDTO, IElasticMaterial>(this, elasticConvertStrategy);
            return convertLogic.Convert(elasticMaterial);
        }

        private IHelperMaterial ProcessReinforcement(IReinforcementLibMaterial reinforcementMaterial)
        {
            var reinforcementConvertStrategy = StrategyContainer.ReinforcementConvertStrategy;
            reinforcementConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            reinforcementConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ReinforcementLibMaterialDTO, IReinforcementLibMaterial>(this, reinforcementConvertStrategy);
            return convertLogic.Convert(reinforcementMaterial);
        }

        private IHelperMaterial ProcessConcrete(IConcreteLibMaterial concreteLibMaterial)
        {
            var concreteConvertStrategy = StrategyContainer.ConcreteConvertStrategy;
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
