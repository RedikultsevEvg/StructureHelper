using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{ 
    public class ForceCombinationFromFileFromDTOConvertStrategy : ConvertStrategy<ForceCombinationFromFile, ForceCombinationFromFileDTO>
    {
        private IUpdateStrategy<IForceCombinationFromFile> updateStrategy;
        private IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy;
        private IConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO> combinationPropertyConvertStrategy;
        private IConvertStrategy<ColumnedFileProperty, ColumnedFilePropertyDTO> fileConvertStrategy;

        public ForceCombinationFromFileFromDTOConvertStrategy(
            IUpdateStrategy<IForceCombinationFromFile> updateStrategy,
            IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy,
            IConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO> combinationPropertyConvertStrategy,
            IConvertStrategy<ColumnedFileProperty, ColumnedFilePropertyDTO> fileConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.combinationPropertyConvertStrategy = combinationPropertyConvertStrategy;
            this.fileConvertStrategy = fileConvertStrategy;
        }

        public ForceCombinationFromFileFromDTOConvertStrategy() { }

        public override ForceCombinationFromFile GetNewItem(ForceCombinationFromFileDTO source)
        {
            TraceLogger?.AddMessage($"Force combination from file Name = {source.Name} converting has been started");
            InitializeStrategies();
            try
            {
                ForceCombinationFromFile newItem = GetForceCombination(source);
                TraceLogger?.AddMessage($"Force combination from file Name = {newItem.Name} converting has been finished successfully");
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage($"Logic: {LoggerStrings.LogicType(this)} made error: {ex.Message}", TraceLogStatuses.Error);
                throw;
            }
        }

        private ForceCombinationFromFile GetForceCombination(ForceCombinationFromFileDTO source)
        {
            ForceCombinationFromFile newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            SetForceFiles(source, newItem);
            SetPoint(source, newItem);
            SetCombinationProperty(source, newItem);
            return newItem;
        }

        private void SetForceFiles(ForceCombinationFromFileDTO source, ForceCombinationFromFile newItem)
        {
            newItem.ForceFiles.Clear();
            foreach (var item in source.ForceFiles)
            {
                if (item is ColumnedFilePropertyDTO filePropertyDTO)
                {
                    ColumnedFileProperty columnFileProperty = fileConvertStrategy.Convert(filePropertyDTO);
                    newItem.ForceFiles.Add(columnFileProperty);
                }
                else
                {
                    string errorString = ErrorStrings.ExpectedWas(typeof(ColumnFilePropertyDTO), item);
                    TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                    throw new StructureHelperException(errorString);
                }
            }
        }

        private void SetPoint(IForceAction source, IForceAction newItem)
        {
            if (source.ForcePoint is Point2DDTO pointDTO)
            {
                newItem.ForcePoint = pointConvertStrategy.Convert(pointDTO);
            }
            else
            {
                string errorMessage = ErrorStrings.ExpectedWas(typeof(Point2DDTO), source.ForcePoint);
                TraceLogger.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
        }
        private void SetCombinationProperty(IForceFactoredCombination source, IForceFactoredCombination newItem)
        {
            if (source.CombinationProperty is FactoredCombinationPropertyDTO factoredPropertyDTO)
            {
                newItem.CombinationProperty = combinationPropertyConvertStrategy.Convert(factoredPropertyDTO);
            }
            else
            {
                string errorMessage = ErrorStrings.ExpectedWas(typeof(FactoredCombinationPropertyDTO), source.CombinationProperty);
                TraceLogger.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ForceCombinationFromFileUpdateStrategy();
            pointConvertStrategy = new Point2DFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
            combinationPropertyConvertStrategy = new FactoredCombinationPropertyFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
            fileConvertStrategy ??= new ColumnedFilePropertyFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        }
    }
}
