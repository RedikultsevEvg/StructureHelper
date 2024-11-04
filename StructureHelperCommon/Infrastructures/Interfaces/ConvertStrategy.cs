using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public abstract class ConvertStrategy<T, V> : IConvertStrategy<T, V>
        where T : ISaveable
        where V : ISaveable
    {
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public T NewItem { get; set; }

        public virtual T Convert(V source)
        {
            try
            {
                Check();
                return GetNewItem(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        public abstract T GetNewItem(V source);
        private void Check()
        {
            var checkLogic = new CheckConvertLogic<T,V>(this);
            checkLogic.Check();
        }
    }
}
