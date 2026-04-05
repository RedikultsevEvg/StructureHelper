using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class ForceFactoredListFromDTOConvertStrategy : ConvertStrategy<ForceFactoredList, ForceFactoredListDTO>
    {
        private IUpdateStrategy<IForceAction> baseUpdateStrategy;
        private IUpdateStrategy<IForceFactoredList> updateStrategy;
        private IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy;
        private IConvertStrategy<ForceTuple, ForceTupleDTO> forceTupleConvertStrategy;
        private IConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO> combinationPropertyConvertStrategy;

        public ForceFactoredListFromDTOConvertStrategy(
            IUpdateStrategy<IForceAction> baseUpdateStrategy,
            IUpdateStrategy<IForceFactoredList> updateStrategy,
            IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy,
            IConvertStrategy<ForceTuple, ForceTupleDTO> forceTupleConvertStrategy,
            IConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO> combinationPropertyConvertStrategy)
        {
            this.baseUpdateStrategy = baseUpdateStrategy;
            this.updateStrategy = updateStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.forceTupleConvertStrategy = forceTupleConvertStrategy;
            this.combinationPropertyConvertStrategy = combinationPropertyConvertStrategy;
        }

        public ForceFactoredListFromDTOConvertStrategy() { }

        public override ForceFactoredList GetNewItem(ForceFactoredListDTO source)
        {
            InitializeStrategies();
            TraceLogger?.AddMessage($"Force combination by factor name = {source.Name} converting is starting");
            ForceFactoredList newItem = new(source.Id);
            baseUpdateStrategy.Update(newItem, source);
            updateStrategy.Update(newItem, source);
            SetPoint(source, newItem);
            SetCombinationProperty(source, newItem);
            SetForceTuples(source, newItem);
            TraceLogger?.AddMessage($"Force combination by factor name = {newItem.Name} converting has been finished");
            return newItem;
        }

        private void SetForceTuples(ForceFactoredListDTO source, ForceFactoredList newItem)
        {
            CheckObject.ThrowIfNull(newItem.ForceTuples, nameof(newItem.ForceTuples));
            newItem.ForceTuples.Clear();
            foreach (var item in source.ForceTuples)
            {
                var newTuple = forceTupleConvertStrategy.Convert((ForceTupleDTO)item);
                newItem.ForceTuples.Add(newTuple);
            }
        }
        private void SetPoint(ForceFactoredListDTO source, ForceFactoredList newItem)
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
        private void SetCombinationProperty(ForceFactoredListDTO source, ForceFactoredList newItem)
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
            baseUpdateStrategy ??= new ForceActionBaseUpdateStrategy();
            updateStrategy ??= new ForceFactoredListUpdateStrategy();
            pointConvertStrategy ??= new Point2DFromDTOConvertStrategy(this);
            forceTupleConvertStrategy ??= new ForceTupleFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            combinationPropertyConvertStrategy ??= new FactoredCombinationPropertyFromDTOConvertStrategy(ReferenceDictionary, TraceLogger);
        }
    }
}
