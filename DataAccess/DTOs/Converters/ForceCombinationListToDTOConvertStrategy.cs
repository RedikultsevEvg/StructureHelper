using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
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

        public ForceCombinationListToDTOConvertStrategy(
            IUpdateStrategy<IForceCombinationList> updateStrategy,
            IConvertStrategy<DesignForceTupleDTO, IDesignForceTuple> convertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.convertStrategy = convertStrategy;
        }

        public ForceCombinationListToDTOConvertStrategy() : this (
            new ForceCombinationListUpdateStrategy(),
            new DesignForceTupleToDTOConvertStrategy())
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
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private ForceCombinationListDTO GetNewForceCombinationList(IForceCombinationList source)
        {

            ForceCombinationListDTO newItem = new() { Id = source.Id};
            updateStrategy.Update(newItem, source);
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<DesignForceTupleDTO, IDesignForceTuple>(this, convertStrategy);
            newItem.DesignForces.Clear();
            foreach (var item in source.DesignForces)
            {
                newItem.DesignForces.Add(convertLogic.Convert(item));
            }
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<ForceCombinationListDTO, IForceCombinationList>(this);
            checkLogic.Check();
        }
    }
}
