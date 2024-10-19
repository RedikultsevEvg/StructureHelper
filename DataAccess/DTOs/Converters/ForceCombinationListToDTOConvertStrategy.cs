using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class ForceCombinationListToDTOConvertStrategy : IConvertStrategy<ForceCombinationListDTO, IForceCombinationList>
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

        public ForceCombinationListToDTOConvertStrategy() : this (
            new ForceCombinationListUpdateStrategy(),
            new DesignForceTupleToDTOConvertStrategy(),
            new ForceActionBaseUpdateStrategy(),
            new Point2DToDTOConvertStrategy())
        {
            
        }


        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ForceCombinationListDTO Convert(IForceCombinationList source)
        {
            try
            {
                Check();
                return GetNewForceCombinationList(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private ForceCombinationListDTO GetNewForceCombinationList(IForceCombinationList source)
        {

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
