using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class HelperMaterialFromDTOConvertStrategy : ConvertStrategy<IHelperMaterial, IHelperMaterial>
    {
        const string MaterialIs = "Material type is: ";
        private IHelperMaterialFromDTOStrategyContainer strategyContainer;


        private IHelperMaterialFromDTOStrategyContainer StrategyContainer => strategyContainer ??= new HelperMaterialFromDTOStrategyContainer();
        public HelperMaterialFromDTOConvertStrategy(IHelperMaterialFromDTOStrategyContainer strategyContainer)
        {
            this.strategyContainer = strategyContainer;
        }

        public HelperMaterialFromDTOConvertStrategy()
        {
            
        }


        public override IHelperMaterial GetNewItem(IHelperMaterial source)
        {
            if (source is ConcreteLibMaterialDTO concrete)
            {
                return GetConcreteMaterial(concrete);
            }
            else if (source is ReinforcementLibMaterialDTO reinforcement)
            {
                return GetReinforcementMaterial(reinforcement);
            }
            else if (source is FRMaterialDTO frMaterial)
            {
                return GetFRMaterial(frMaterial);
            }
            else if (source is ElasticMaterialDTO elastic)
            {
                return GetElasticMaterial(elastic);
            }
            else if (source is SteelLibMaterialDTO steel)
            {
                return GetSteelMaterial(steel);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source) + ": helper material type";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }

        private IHelperMaterial GetSteelMaterial(SteelLibMaterialDTO source)
        {
            var strategy = StrategyContainer.SteelConvertStrategy;
            TraceLogger?.AddMessage(MaterialIs + "Elastic material", TraceLogStatuses.Service);
            strategy.ReferenceDictionary = ReferenceDictionary;
            strategy.TraceLogger = TraceLogger;
            var newItem = strategy.Convert(source);
            strategyContainer.SafetyFactorUpdateStrategy.Update(newItem, source);
            return newItem;
        }

        private IHelperMaterial GetElasticMaterial(ElasticMaterialDTO source)
        {
            var strategy = StrategyContainer.ElasticConvertStrategy;
            TraceLogger?.AddMessage(MaterialIs + "Elastic material", TraceLogStatuses.Service);
            strategy.ReferenceDictionary = ReferenceDictionary;
            strategy.TraceLogger = TraceLogger;
            var newItem = strategy.Convert(source);
            strategyContainer.SafetyFactorUpdateStrategy.Update(newItem, source);
            return newItem;
        }

        private IHelperMaterial GetFRMaterial(FRMaterialDTO source)
        {
            var strategy = StrategyContainer.FrConvertStrategy;
            TraceLogger?.AddMessage(MaterialIs + "Fiber reinforcement material", TraceLogStatuses.Service);
            strategy.ReferenceDictionary = ReferenceDictionary;
            strategy.TraceLogger = TraceLogger;
            var newItem = strategy.Convert(source);
            StrategyContainer.SafetyFactorUpdateStrategy.Update(newItem, source);
            return newItem;
        }

        private IHelperMaterial GetReinforcementMaterial(ReinforcementLibMaterialDTO source)
        {
            var strategy = StrategyContainer.ReinforcementConvertStrategy;
            TraceLogger?.AddMessage(MaterialIs + "Reinforcement library material", TraceLogStatuses.Service);
            strategy.ReferenceDictionary = ReferenceDictionary;
            strategy.TraceLogger = TraceLogger;
            var newItem = strategy.Convert(source);
            StrategyContainer.SafetyFactorUpdateStrategy.Update(newItem, source);
            return newItem;
        }

        private IHelperMaterial GetConcreteMaterial(ConcreteLibMaterialDTO source)
        {
            var strategy = StrategyContainer.ConcreteConvertStrategy;
            TraceLogger?.AddMessage(MaterialIs + "Concrete library material", TraceLogStatuses.Service);
            strategy.ReferenceDictionary = ReferenceDictionary;
            strategy.TraceLogger = TraceLogger;
            var newItem = strategy.Convert(source);
            StrategyContainer.SafetyFactorUpdateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
