using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class ForceCombinationByFactorToDTOConvertStrategy : IConvertStrategy<ForceCombinationByFactorDTO, IForceFactoredList>
    {
        private IUpdateStrategy<IForceFactoredList> updateStrategy;
        private IConvertStrategy<Point2DDTO, IPoint2D> pointUpdateStrategy;

        private IConvertStrategy<ForceTupleDTO, IForceTuple> forceTupleConvertStrategy;
        private IUpdateStrategy<IForceAction> baseUpdateStrategy;

        public ForceCombinationByFactorToDTOConvertStrategy(IUpdateStrategy<IForceFactoredList> updateStrategy,
            IConvertStrategy<Point2DDTO, IPoint2D> pointUpdateStrategy,
            IConvertStrategy<ForceTupleDTO, IForceTuple> convertStrategy,
            IUpdateStrategy<IForceAction> baseUpdateStrategy)
        {
            this.baseUpdateStrategy = baseUpdateStrategy;
            this.updateStrategy = updateStrategy;
            this.forceTupleConvertStrategy = convertStrategy;
            this.pointUpdateStrategy = pointUpdateStrategy;
        }

        public ForceCombinationByFactorToDTOConvertStrategy() : this (
            new ForceFactoredListUpdateStrategy(),
            new Point2DToDTOConvertStrategy(),
            new ForceTupleToDTOConvertStrategy(),
            new ForceActionBaseUpdateStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ForceCombinationByFactorDTO Convert(IForceFactoredList source)
        {
            Check();
            try
            {
                ForceCombinationByFactorDTO newItem = GetNewForceTuple(source);
                TraceLogger.AddMessage($"Force combination by factor, name = {newItem.Name}  was converted", TraceLogStatuses.Debug);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
            
        }

        private ForceCombinationByFactorDTO GetNewForceTuple(IForceFactoredList source)
        {
            ForceCombinationByFactorDTO newItem = new() { Id = source.Id };
            baseUpdateStrategy.Update(newItem, source);
            updateStrategy.Update(newItem, source);
            GetNewForcePoint(source, newItem);
            GetNewFullSLSForces(source, newItem);
            return newItem;
        }

        private void GetNewFullSLSForces(IForceFactoredList source, ForceCombinationByFactorDTO newItem)
        {
            if (source.ForceTuples[0] is not null)
            {
                forceTupleConvertStrategy.ReferenceDictionary = ReferenceDictionary;
                forceTupleConvertStrategy.TraceLogger = TraceLogger;
                var convertForceTupleLogic = new DictionaryConvertStrategy<ForceTupleDTO, IForceTuple>(this, forceTupleConvertStrategy);
                newItem.ForceTuples.Clear();
                foreach (var item in source.ForceTuples)
                {
                    var forceTuple = convertForceTupleLogic.Convert(item);
                    newItem.ForceTuples.Add(forceTuple);
                }
            }  
        }

        private void GetNewForcePoint(IForceFactoredList source, ForceCombinationByFactorDTO newItem)
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
            var checkLogic = new CheckConvertLogic<ForceCombinationByFactorDTO, IForceFactoredList>(this);
            checkLogic.Check();
        }
    }
}
