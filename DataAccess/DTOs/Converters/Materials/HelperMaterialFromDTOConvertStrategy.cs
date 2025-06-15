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
        private IConvertStrategy<ConcreteLibMaterial, ConcreteLibMaterialDTO> concreteConvertStrategy;
        private IConvertStrategy<ReinforcementLibMaterial, ReinforcementLibMaterialDTO> reinforcementConvertStrategy;
        private IConvertStrategy<ElasticMaterial, ElasticMaterialDTO> elasticConvertStrategy;
        private IConvertStrategy<FRMaterial, FRMaterialDTO> frConvertStrategy;
        private IUpdateStrategy<IHelperMaterial> safetyFactorUpdateStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorsFromDTOLogic());

        public HelperMaterialFromDTOConvertStrategy() : this(
            new ConcreteLibMaterialFromDTOConvertStrategy(),
            new ReinforcementLibMaterialFromDTOConvertStrategy(),
            new ElasticMaterialFromDTOConvertStrategy(),
            new FRMaterialFromDTOConvertStrategy()
            )
        {
            
        }

        public HelperMaterialFromDTOConvertStrategy(
            IConvertStrategy<ConcreteLibMaterial, ConcreteLibMaterialDTO> concreteConvertStrategy,
            IConvertStrategy<ReinforcementLibMaterial, ReinforcementLibMaterialDTO> reinforcementConvertStrategy,
            IConvertStrategy<ElasticMaterial, ElasticMaterialDTO> elasticConvertStrategy,
            IConvertStrategy<FRMaterial, FRMaterialDTO> frConvertStrategy)
        {
            this.concreteConvertStrategy = concreteConvertStrategy;
            this.reinforcementConvertStrategy = reinforcementConvertStrategy;
            this.elasticConvertStrategy = elasticConvertStrategy;
            this.frConvertStrategy = frConvertStrategy;
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
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source) + ": helper material type";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }

        private IHelperMaterial GetElasticMaterial(ElasticMaterialDTO source)
        {
            TraceLogger?.AddMessage(MaterialIs + "Elastic material", TraceLogStatuses.Service);
            elasticConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            elasticConvertStrategy.TraceLogger = TraceLogger;
            var newItem = elasticConvertStrategy.Convert(source);
            safetyFactorUpdateStrategy.Update(newItem, source);
            return newItem;
        }

        private IHelperMaterial GetFRMaterial(FRMaterialDTO source)
        {
            TraceLogger?.AddMessage(MaterialIs + "Fiber reinforcement material", TraceLogStatuses.Service);
            frConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            frConvertStrategy.TraceLogger = TraceLogger;
            var newItem = frConvertStrategy.Convert(source);
            safetyFactorUpdateStrategy.Update(newItem, source);
            return newItem;
        }

        private IHelperMaterial GetReinforcementMaterial(ReinforcementLibMaterialDTO source)
        {
            TraceLogger?.AddMessage(MaterialIs + "Reinforcement library material", TraceLogStatuses.Service);
            reinforcementConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            reinforcementConvertStrategy.TraceLogger = TraceLogger;
            var newItem = reinforcementConvertStrategy.Convert(source);
            safetyFactorUpdateStrategy.Update(newItem, source);
            return newItem;
        }

        private IHelperMaterial GetConcreteMaterial(ConcreteLibMaterialDTO source)
        {
            TraceLogger?.AddMessage(MaterialIs + "Concrete library material", TraceLogStatuses.Service);
            concreteConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            concreteConvertStrategy.TraceLogger = TraceLogger;
            var newItem = concreteConvertStrategy.Convert(source);
            safetyFactorUpdateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
