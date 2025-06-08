using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class ForceCombinationListToDTOConvertStrategy : ConvertStrategy<ForceCombinationListDTO, IForceCombinationList>
    {
        private IUpdateStrategy<IForceCombinationList> updateStrategy;
        private IConvertStrategy<DesignForceTupleDTO, IDesignForceTuple> convertStrategy;
        private IUpdateStrategy<IForceAction> baseUpdateStrategy;
        private IConvertStrategy<Point2DDTO, IPoint2D> pointUpdateStrategy;

        public ForceCombinationListToDTOConvertStrategy(
            IUpdateStrategy<IForceCombinationList> updateStrategy,
            IConvertStrategy<DesignForceTupleDTO, IDesignForceTuple> convertStrategy,
            IUpdateStrategy<IForceAction> baseUpdateStrategy,
            IConvertStrategy<Point2DDTO, IPoint2D> pointUpdateStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.convertStrategy = convertStrategy;
            this.baseUpdateStrategy = baseUpdateStrategy;
            this.pointUpdateStrategy = pointUpdateStrategy;
        }

        public ForceCombinationListToDTOConvertStrategy() { }

        public override ForceCombinationListDTO GetNewItem(IForceCombinationList source)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage($"Factored combination list Name: {source.Name} has been started");
            ForceCombinationListDTO forceCombinationListDTO = GetNewForceCombinationList(source);
            TraceLogger?.AddMessage($"Factored combination list Name: {source.Name} has been finished");
            return forceCombinationListDTO;
        }

        private ForceCombinationListDTO GetNewForceCombinationList(IForceCombinationList source)
        {
            InitializeStrategies();
            ForceCombinationListDTO newItem = new() { Id = source.Id};
            baseUpdateStrategy.Update(newItem, source);
            updateStrategy.Update(newItem, source);
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<DesignForceTupleDTO, IDesignForceTuple>(this, convertStrategy);
            GetNewForcePoint(newItem, source);
            newItem.DesignForces.Clear();
            foreach (var item in source.DesignForces)
            {
                newItem.DesignForces.Add(convertLogic.Convert(item));
            }
            return newItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ForceCombinationListUpdateStrategy();
            convertStrategy ??= new DesignForceTupleToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
            baseUpdateStrategy ??= new ForceActionBaseUpdateStrategy();
            pointUpdateStrategy ??= new Point2DToDTOConvertStrategy();
        }

        private void GetNewForcePoint(ForceCombinationListDTO newItem, IForceCombinationList source)
        {
            if (source.ForcePoint is not null)
            {
                pointUpdateStrategy.ReferenceDictionary = ReferenceDictionary;
                pointUpdateStrategy.TraceLogger = TraceLogger;
                var convertLogic = new DictionaryConvertStrategy<Point2DDTO, IPoint2D>(this, pointUpdateStrategy);
                newItem.ForcePoint = convertLogic.Convert(source.ForcePoint);
            }
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<ForceCombinationListDTO, IForceCombinationList>(this);
            checkLogic.Check();
        }
    }
}
