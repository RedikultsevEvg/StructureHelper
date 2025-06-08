using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;

namespace DataAccess.DTOs
{
    public class FactoredCombinationPropertyFromDTOConvertStrategy : ConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO>
    {
        private IUpdateStrategy<IFactoredCombinationProperty> updateStrategy;
        public override FactoredCombinationProperty GetNewItem(FactoredCombinationPropertyDTO source)
        {
            InitializeStrategies();
            TraceLogger.AddMessage($"Force factored combination property Id={source.Id} converting has been started");
            FactoredCombinationProperty newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            TraceLogger.AddMessage($"Force factored combination property Id={newItem.Id} converting has been finished");
            return newItem;
        }
        private void InitializeStrategies()
        {
            updateStrategy ??= new FactoredCombinationPropertyUpdateStrategy();
        }
    }
}
