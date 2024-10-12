using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class DesignForceTupleToDTOConvertStrategy : IConvertStrategy<DesignForceTupleDTO, IDesignForceTuple>
    {
        private IUpdateStrategy<IDesignForceTuple> updateStrategy;
        private IConvertStrategy<ForceTupleDTO, IForceTuple> forceTupleConvertStrategy;

        public DesignForceTupleToDTOConvertStrategy(IUpdateStrategy<IDesignForceTuple> updateStrategy,
            IConvertStrategy<ForceTupleDTO, IForceTuple> forceTupleConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.forceTupleConvertStrategy = forceTupleConvertStrategy;
        }

        public DesignForceTupleToDTOConvertStrategy() : this(new DesignForceTupleUpdateStrategy(),
            new ForceTupleToDTOConvertStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public DesignForceTupleDTO Convert(IDesignForceTuple source)
        {
            try
            {
                Check();
                DesignForceTupleDTO designForceTupleDTO = GetNewDesignForceTuple(source);
                return designForceTupleDTO;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private DesignForceTupleDTO GetNewDesignForceTuple(IDesignForceTuple source)
        {
            DesignForceTupleDTO newItem = new() { Id = source.Id };
            updateStrategy.Update(newItem, source);
            forceTupleConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            forceTupleConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ForceTupleDTO, IForceTuple>(this, forceTupleConvertStrategy);
            newItem.ForceTuple = convertLogic.Convert(source.ForceTuple);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<DesignForceTupleDTO, IDesignForceTuple>(this);
            checkLogic.Check();
        }
    }
}
