using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <inheritdoc/>
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
                TraceStartOfConverting(source);
                T target = GetNewItem(source);
                TraceFinishOfConverting(target);
                return target;
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

        public void TraceErrorByEntity(object obj, string message)
        {
            TraceLogger?.AddMessage($"Logic: {LoggerStrings.LogicType(obj)} made error: {message}", TraceLogStatuses.Error);
        }
        private void TraceStartOfConverting(ISaveable saveable)
        {
            TraceLogger?.AddMessage($"Converting {saveable.GetType()} Id = {saveable.Id} has been started");
        }
        private void TraceFinishOfConverting(ISaveable saveable)
        {
            TraceLogger?.AddMessage($"Converting {saveable.GetType()} Id = {saveable.Id} has been started");
        }
    }
}
