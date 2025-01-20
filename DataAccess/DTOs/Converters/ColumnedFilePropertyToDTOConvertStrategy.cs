using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;

namespace DataAccess.DTOs
{
    /// <inheritdoc/>
    public class ColumnedFilePropertyToDTOConvertStrategy : ConvertStrategy<ColumnedFilePropertyDTO, IColumnedFileProperty>
    {
        private IUpdateStrategy<IColumnedFileProperty>? updateStrategy;
        private IConvertStrategy<ColumnFilePropertyDTO, IColumnFileProperty>? columnConvertStrategy;

        public override ColumnedFilePropertyDTO GetNewItem(IColumnedFileProperty source)
        {
            TraceLogger?.AddMessage($"Columned file property Id = {source.Id}, Path = {source.FilePath} converting has been started");
            InitializeStrategies();
            try
            {
                ColumnedFilePropertyDTO newItem = GetNewItemBySource(source);
                TraceLogger?.AddMessage($"Columned file property Id={newItem.Id}, Path = {newItem.FilePath} converting has been finished successfully");
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage($"Logic: {LoggerStrings.LogicType(this)} made error: {ex.Message}", TraceLogStatuses.Error);
                throw;
            }

        }

        private ColumnedFilePropertyDTO GetNewItemBySource(IColumnedFileProperty source)
        {
            ColumnedFilePropertyDTO newItem = new(source.Id);
            updateStrategy?.Update(newItem, source);
            newItem.ColumnProperties.Clear();
            foreach (var item in source.ColumnProperties)
            {
                ColumnFilePropertyDTO columnFilePropertyDTO = columnConvertStrategy.Convert(item);
                newItem.ColumnProperties.Add(columnFilePropertyDTO);
            }

            return newItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ColumnedFilePropertyUpdateStrategy();
            columnConvertStrategy ??= new ColumnFilePropertyToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
        }
    }
}
