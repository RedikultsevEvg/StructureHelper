using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;

namespace DataAccess.DTOs
{
    public class ColumnedFilePropertyFromDTOConvertStrategy : ConvertStrategy<ColumnedFileProperty, ColumnedFilePropertyDTO>
    {
        private IUpdateStrategy<IColumnedFileProperty>? updateStrategy;
        private IConvertStrategy<ColumnFileProperty, ColumnFilePropertyDTO>? columnConvertStrategy;
        public override ColumnedFileProperty GetNewItem(ColumnedFilePropertyDTO source)
        {
            TraceLogger?.AddMessage($"Converting of columned file property Path={source.FilePath} has been started");
            InitializeStrategies();
            try
            {
                ColumnedFileProperty newItem = GetFilePropertyBySource(source);
                TraceLogger?.AddMessage($"Converting of columned file property Path={newItem.FilePath} has been finished successfully");
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage($"Logic: {LoggerStrings.LogicType(this)} made error: {ex.Message}", TraceLogStatuses.Error);
                throw;
            }
        }

        private ColumnedFileProperty GetFilePropertyBySource(ColumnedFilePropertyDTO source)
        {
            ColumnedFileProperty newItem = new(source.Id);
            updateStrategy?.Update(newItem, source);
            newItem.ColumnProperties.Clear();
            foreach (var item in source.ColumnProperties)
            {
                if (item is ColumnFilePropertyDTO columnPropertyDTO)
                {
                    ColumnFileProperty columnFileProperty = columnConvertStrategy.Convert(columnPropertyDTO);
                    newItem.ColumnProperties.Add(columnFileProperty);
                }
                else
                {
                    string errorString = ErrorStrings.ExpectedWas(typeof(ColumnFilePropertyDTO), item);
                    TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                    throw new StructureHelperException(errorString);
                }
            }

            return newItem;
        }
        private void InitializeStrategies()
        {
            updateStrategy ??= new ColumnedFilePropertyUpdateStrategy();
            columnConvertStrategy ??= new ColumnFilePropertyFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        }
    }
}
