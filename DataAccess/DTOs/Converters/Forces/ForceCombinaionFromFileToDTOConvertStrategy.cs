using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class ForceCombinaionFromFileToDTOConvertStrategy : ConvertStrategy<ForceCombinationFromFileDTO, IForceCombinationFromFile>
    {

        private IUpdateStrategy<IForceCombinationFromFile> updateStrategy;
        private IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy;
        private IConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty> combinationPropertyConvertStrategy;
        private IConvertStrategy<ColumnedFilePropertyDTO, IColumnedFileProperty> filePropertyConvertStrategy;
        


        public override ForceCombinationFromFileDTO GetNewItem(IForceCombinationFromFile source)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            TraceLogger.AddMessage($"Force combination from file, name = {source.Name}  converting has been started");
            InitializeStrategies();
            ForceCombinationFromFileDTO newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            newItem.ForceFiles.Clear();
            foreach (var item in source.ForceFiles)
            {
                ColumnedFilePropertyDTO columnedFilePropertyDTO = filePropertyConvertStrategy.Convert(item);
                newItem.ForceFiles.Add(columnedFilePropertyDTO);
            }
            SetPoint(source, newItem);
            SetCombinationProperty(source, newItem);
            TraceLogger.AddMessage($"Force combination from file, name = {source.Name}  converting has been finished successfully");
            return newItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ForceCombinationFromFileUpdateStrategy();
            pointConvertStrategy ??= new Point2DToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
            combinationPropertyConvertStrategy ??= new FactoredCombinationPropertyToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            filePropertyConvertStrategy ??= new ColumnedFilePropertyToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        }

        private void SetPoint(IForceCombinationFromFile source, ForceCombinationFromFileDTO newItem)
        {
            if (source.ForcePoint is not null)
            {
                var convertLogic = new DictionaryConvertStrategy<Point2DDTO, IPoint2D>(this, pointConvertStrategy);
                newItem.ForcePoint = convertLogic.Convert(source.ForcePoint);
            }
            else
            {
                string errorMessage = ErrorStrings.NullReference + $"File combination {source.Name} Id={source.Id} does not have force point";
                TraceLogger.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
        }
        private void SetCombinationProperty(IForceCombinationFromFile source, ForceCombinationFromFileDTO newItem)
        {
            if (source.CombinationProperty is not null)
            {
                var convertLogic = new DictionaryConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty>(this, combinationPropertyConvertStrategy);
                newItem.CombinationProperty = convertLogic.Convert(source.CombinationProperty);
            }
            else
            {
                string errorMessage = ErrorStrings.NullReference + $"Factored combination {source.Name} Id={source.Id} does not have combination properties";
                TraceLogger.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
        }
    }
}
