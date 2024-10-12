using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceTupleToDTOConvertStrategy : IConvertStrategy<ForceTupleDTO, IForceTuple>
    {
        private IUpdateStrategy<IForceTuple> updateStrategy;

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ForceTupleToDTOConvertStrategy(IUpdateStrategy<IForceTuple> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ForceTupleToDTOConvertStrategy() : this(new ForceTupleUpdateStrategy())
        {
            
        }

        public ForceTupleDTO Convert(IForceTuple source)
        {
            Check();
            try
            {
                ForceTupleDTO newItem = new() { Id = source.Id};
                updateStrategy.Update(newItem, source);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<ForceTupleDTO, IForceTuple>(this);
            checkLogic.Check();
        }
    }
}
