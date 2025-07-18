using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class RebarSectionFromDTOConvertStrategy : ConvertStrategy<RebarSection, IRebarSection>
    {
        private IUpdateStrategy<IRebarSection> updateStrategy;
        private IConvertStrategy<ReinforcementLibMaterial, ReinforcementLibMaterialDTO> reinforcementConvertStrategy;
        private HelperMaterialDTOSafetyFactorUpdateStrategy safetyFactorUpdateStrategy;

        public RebarSectionFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override RebarSection GetNewItem(IRebarSection source)
        {
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            if (source.Material is not ReinforcementLibMaterialDTO reinforcement)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.Material));
            }
            NewItem.Material = reinforcementConvertStrategy.Convert(reinforcement);
            safetyFactorUpdateStrategy.Update(NewItem.Material, source.Material);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new RebarSectionUpdateStrategy() { UpdateChildren = false};
            reinforcementConvertStrategy = new ReinforcementLibMaterialFromDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            safetyFactorUpdateStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorsFromDTOLogic());
        }
    }
}
