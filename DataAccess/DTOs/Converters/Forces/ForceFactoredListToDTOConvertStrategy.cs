using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class ForceFactoredListToDTOConvertStrategy : ConvertStrategy<ForceFactoredListDTO, IForceFactoredList>
    {
        private IUpdateStrategy<IForceFactoredList> updateStrategy;
        private IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy;
        private IConvertStrategy<ForceTupleDTO, IForceTuple> forceTupleConvertStrategy;
        private IUpdateStrategy<IForceAction> baseUpdateStrategy;
        private IConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty> combinationPropertyConvertStrategy;

        public ForceFactoredListToDTOConvertStrategy(IUpdateStrategy<IForceFactoredList> updateStrategy,
            IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy,
            IConvertStrategy<ForceTupleDTO, IForceTuple> forceTupleConvertStrategy,
            IUpdateStrategy<IForceAction> baseUpdateStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.forceTupleConvertStrategy = forceTupleConvertStrategy;
            this.baseUpdateStrategy = baseUpdateStrategy;
        }

        public ForceFactoredListToDTOConvertStrategy() { }

        public override ForceFactoredListDTO GetNewItem(IForceFactoredList source)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage($"Force combination by factor, name = {source.Name}  converting has been started");
            InitializeStrategies();
            ForceFactoredListDTO newItem = GetNewForceTuple(source);
            TraceLogger?.AddMessage($"Force combination by factor, name = {newItem.Name}  converting has been finished successfully");
            return newItem;

        }
        private void InitializeStrategies()
        {
            baseUpdateStrategy ??= new ForceActionBaseUpdateStrategy();
            updateStrategy ??= new ForceFactoredListUpdateStrategy();
            forceTupleConvertStrategy ??= new ForceTupleToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            pointConvertStrategy ??= new Point2DToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
            combinationPropertyConvertStrategy ??= new FactoredCombinationPropertyToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
        }

        private ForceFactoredListDTO GetNewForceTuple(IForceFactoredList source)
        {
            ForceFactoredListDTO newItem = new(source.Id);
            baseUpdateStrategy.Update(newItem, source);
            updateStrategy.Update(newItem, source);
            SetPoint(source, newItem);
            SetForces(source, newItem);
            SetCombinationProperty(source, newItem);
            return newItem;
        }
        private void SetForces(IForceFactoredList source, ForceFactoredListDTO newItem)
        {
            if (source.ForceTuples is not null)
            {
                var convertForceTupleLogic = new DictionaryConvertStrategy<ForceTupleDTO, IForceTuple>(this, forceTupleConvertStrategy);
                newItem.ForceTuples.Clear();
                foreach (var item in source.ForceTuples)
                {
                    var forceTuple = convertForceTupleLogic.Convert(item);
                    newItem.ForceTuples.Add(forceTuple);
                }
            }
            else
            {
                string errorMessage = ErrorStrings.NullReference + $"Factored combination {source.Name} Id={source.Id} does not have list of forces";
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
        }
        private void SetPoint(IForceFactoredList source, ForceFactoredListDTO newItem)
        {
            if (source.ForcePoint is not null)
            {
                var convertLogic = new DictionaryConvertStrategy<Point2DDTO, IPoint2D>(this, pointConvertStrategy);
                newItem.ForcePoint = convertLogic.Convert(source.ForcePoint);
            }
            else
            {
                string errorMessage = ErrorStrings.NullReference + $"Factored combination {source.Name} Id={source.Id} does not have force point";
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
        }
        private void SetCombinationProperty(IForceFactoredList source, ForceFactoredListDTO newItem)
        {
            if (source.CombinationProperty is not null)
            {
                var convertLogic = new DictionaryConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty>(this, combinationPropertyConvertStrategy);
                newItem.CombinationProperty = convertLogic.Convert(source.CombinationProperty);
            }
            else
            {
                string errorMessage = ErrorStrings.NullReference + $"Factored combination {source.Name} Id={source.Id} does not have combination properties";
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
        }

    }
}
