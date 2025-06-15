using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    internal class StirrupByRebarFromDTOConvertStrategy : ConvertStrategy<StirrupByRebar, StirrupByRebarDTO>
    {
        private IUpdateStrategy<IStirrupByRebar> updateStrategy;
        private IConvertStrategy<ReinforcementLibMaterial, ReinforcementLibMaterialDTO> reinforcementConvertStrategy;
        private HelperMaterialDTOSafetyFactorUpdateStrategy safetyFactorUpdateStrategy;

        public StirrupByRebarFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override StirrupByRebar GetNewItem(StirrupByRebarDTO source)
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
            updateStrategy ??= new StirrupByRebarUpdateStrategy();
            reinforcementConvertStrategy = new ReinforcementLibMaterialFromDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            safetyFactorUpdateStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorsFromDTOLogic());
        }
    }
}
