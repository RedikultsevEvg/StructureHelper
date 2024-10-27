using StructureHelperCommon.Infrastructures.Interfaces;
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
    public class ForceCombinationByFactorFromDTOConvertStrategy : ConvertStrategy<ForceCombinationByFactor, ForceCombinationByFactorDTO>
    {
        private IUpdateStrategy<IForceAction> baseUpdateStrategy;
        private IUpdateStrategy<IForceCombinationByFactor> updateStrategy;
        private IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy;
        private IConvertStrategy<ForceTuple, ForceTupleDTO> forceTupleConvertStrategy;

        public ForceCombinationByFactorFromDTOConvertStrategy(
            IUpdateStrategy<IForceAction> baseUpdateStrategy,
            IUpdateStrategy<IForceCombinationByFactor> updateStrategy,
            IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy,
            IConvertStrategy<ForceTuple, ForceTupleDTO> forceTupleConvertStrategy)
        {
            this.baseUpdateStrategy = baseUpdateStrategy;
            this.updateStrategy = updateStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.forceTupleConvertStrategy = forceTupleConvertStrategy;
        }

        public ForceCombinationByFactorFromDTOConvertStrategy() : this(
            new ForceActionBaseUpdateStrategy(),
            new ForceCombinationByFactorUpdateStrategy(),
            new Point2DFromDTOConvertStrategy(),
            new ForceTupleFromDTOConvertStrategy())
        {
            
        }

        public override ForceCombinationByFactor GetNewItem(ForceCombinationByFactorDTO source)
        {
            TraceLogger.AddMessage($"Force combination by factor name = {source.Name} is starting");
            ForceCombinationByFactor newItem = new(source.Id);
            baseUpdateStrategy.Update(newItem, source);
            updateStrategy.Update(newItem, source);
            pointConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            pointConvertStrategy.TraceLogger = TraceLogger;
            newItem.ForcePoint = pointConvertStrategy.Convert((Point2DDTO)source.ForcePoint);
            forceTupleConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            forceTupleConvertStrategy.TraceLogger = TraceLogger;
            newItem.FullSLSForces = forceTupleConvertStrategy.Convert((ForceTupleDTO)source.FullSLSForces);
            TraceLogger.AddMessage($"Force combination by factor name = {newItem.Name} has been finished");
            return newItem;
        }
    }
}
