using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class ColumnFilePropertyToDTOConvertStrategy : ConvertStrategy<ColumnFilePropertyDTO, IColumnFileProperty>
    {
        private IUpdateStrategy<IColumnFileProperty> updateStrategy;

        public ColumnFilePropertyToDTOConvertStrategy(IUpdateStrategy<IColumnFileProperty> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ColumnFilePropertyToDTOConvertStrategy()
        {
            
        }

        public override ColumnFilePropertyDTO GetNewItem(IColumnFileProperty source)
        {
            TraceLogger?.AddMessage($"Column file property Id={source.Id} converting to {typeof(ColumnFilePropertyDTO)} has been started");
            updateStrategy ??= new ColumnFilePropertyUpdateStrategy();
            ColumnFilePropertyDTO newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            TraceLogger?.AddMessage($"Column file property Id={source.Id} converting to {typeof(ColumnFilePropertyDTO)} has been finished");
            return newItem;
        }
    }
}
