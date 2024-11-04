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
    public class ForceCombinationListFromDTOConvertStrategy : ConvertStrategy<ForceCombinationList, ForceCombinationListDTO>
    {
        private IUpdateStrategy<IForceAction> baseUpdateStrategy;
        private IUpdateStrategy<IForceCombinationList> updateStrategy;
        private IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy;
        private IConvertStrategy<DesignForceTuple, DesignForceTupleDTO> designTupleConvertStrategy;

        public ForceCombinationListFromDTOConvertStrategy(
            IUpdateStrategy<IForceAction> baseUpdateStrategy,
            IUpdateStrategy<IForceCombinationList> updateStrategy,
            IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy,
            IConvertStrategy<DesignForceTuple, DesignForceTupleDTO> designTupleConvertStrategy)
        {
            this.baseUpdateStrategy = baseUpdateStrategy;
            this.updateStrategy = updateStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.designTupleConvertStrategy = designTupleConvertStrategy;
        }

        public ForceCombinationListFromDTOConvertStrategy() : this(
            new ForceActionBaseUpdateStrategy(),
            new ForceCombinationListUpdateStrategy(),
            new Point2DFromDTOConvertStrategy(),
            new DesignForceTupleFromDTOConvertStrategy())
        {
            
        }

        public override ForceCombinationList GetNewItem(ForceCombinationListDTO source)
        {
            TraceLogger?.AddMessage($"Force combination list name = {source.Name} is starting");
            ForceCombinationList newItem = new(source.Id);
            baseUpdateStrategy.Update(newItem, source);
            //updateStrategy.Update(newItem, source);
            pointConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            pointConvertStrategy.TraceLogger = TraceLogger;
            newItem.ForcePoint = pointConvertStrategy.Convert((Point2DDTO)source.ForcePoint);
            designTupleConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            designTupleConvertStrategy.TraceLogger = TraceLogger;
            newItem.DesignForces.Clear();
            foreach (var item in source.DesignForces)
            {
                DesignForceTuple newDesignTuple = designTupleConvertStrategy.Convert((DesignForceTupleDTO)item);
                TraceLogger?.AddMessage($"New Design Tuple Limit state = {newDesignTuple.LimitState}, Calc term = {newDesignTuple.CalcTerm}");
                TraceLogger?.AddMessage($"Mx = {newDesignTuple.ForceTuple.Mx}, My = {newDesignTuple.ForceTuple.My}, Nz = {newDesignTuple.ForceTuple.Nz}");
                newItem.DesignForces.Add(newDesignTuple);
            }
            TraceLogger?.AddMessage($"Force combination list name = {newItem.Name} has been finished succesfully");
            return newItem;
        }
    }
}
