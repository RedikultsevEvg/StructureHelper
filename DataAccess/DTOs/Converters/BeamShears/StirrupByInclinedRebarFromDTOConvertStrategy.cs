using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class StirrupByInclinedRebarFromDTOConvertStrategy : ConvertStrategy<StirrupByInclinedRebar, IStirrupByInclinedRebar>
    {
        private IUpdateStrategy<IStirrupByInclinedRebar> updateStrategy;
        private IConvertStrategy<RebarSection, IRebarSection> rebarSectionConvertStrategy;

        public StirrupByInclinedRebarFromDTOConvertStrategy(
            Dictionary<(Guid id, Type type), ISaveable> referenceDictionary,
            IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override StirrupByInclinedRebar GetNewItem(IStirrupByInclinedRebar source)
        {
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.RebarSection = rebarSectionConvertStrategy.Convert(source.RebarSection);    
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new StirrupByInclinedRebarUpdateStrategy() { UpdateChildren = false};
            rebarSectionConvertStrategy ??= new RebarSectionFromDTOConvertStrategy(this);
        }
    }
}
