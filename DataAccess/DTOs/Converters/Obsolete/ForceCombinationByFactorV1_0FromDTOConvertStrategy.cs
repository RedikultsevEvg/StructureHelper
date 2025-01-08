using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceCombinationByFactorV1_0FromDTOConvertStrategy : ConvertStrategy<ForceFactoredList, ForceCombinationByFactorV1_0DTO>
    {
        private IUpdateStrategy<IForceAction> baseUpdateStrategy;
        private IUpdateStrategy<IForceFactoredList> updateStrategy;
        private IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy;
        private IConvertStrategy<ForceTuple, ForceTupleDTO> forceTupleConvertStrategy;

        public ForceCombinationByFactorV1_0FromDTOConvertStrategy(
            IUpdateStrategy<IForceAction> baseUpdateStrategy,
            IUpdateStrategy<IForceFactoredList> updateStrategy,
            IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy,
            IConvertStrategy<ForceTuple, ForceTupleDTO> forceTupleConvertStrategy)
        {
            this.baseUpdateStrategy = baseUpdateStrategy;
            this.updateStrategy = updateStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.forceTupleConvertStrategy = forceTupleConvertStrategy;
        }

        public ForceCombinationByFactorV1_0FromDTOConvertStrategy() : this(
            new ForceActionBaseUpdateStrategy(),
            new ForceFactoredListUpdateStrategy(),
            new Point2DFromDTOConvertStrategy(),
            new ForceTupleFromDTOConvertStrategy())
        {
            
        }

        public override ForceFactoredList GetNewItem(ForceCombinationByFactorV1_0DTO source)
        {
            TraceLogger.AddMessage($"Force combination by factor name = {source.Name} is starting");
            ForceFactoredList newItem = new(source.Id);
            baseUpdateStrategy.Update(newItem, source);
            updateStrategy.Update(newItem, source);
            pointConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            pointConvertStrategy.TraceLogger = TraceLogger;
            newItem.ForcePoint = pointConvertStrategy.Convert((Point2DDTO)source.ForcePoint);
            forceTupleConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            forceTupleConvertStrategy.TraceLogger = TraceLogger;
            var forceTuple = forceTupleConvertStrategy.Convert((ForceTupleDTO)source.ForceTuple);
            newItem.ForceTuples[0] = forceTuple;
            TraceLogger.AddMessage($"Force combination by factor name = {source.Name} was successfully converted from version 1.0", TraceLogStatuses.Warning);
            TraceLogger.AddMessage($"Force combination by factor name = {newItem.Name} has been finished");
            return newItem;
        }
    }
}
