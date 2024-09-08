using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public class DictionaryConvertStrategy<T, V> : IConvertStrategy<T, V>
        where T : ISaveable
        where V : ISaveable
    {
        public IShiftTraceLogger? TraceLogger { get; set; }
        public IConvertStrategy<T,V> ConvertStrategy { get; set; }
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public T ConvertFrom(V source)
        {
            ICheckInputData();
            T val;
            var key = (source.Id, typeof(T));
            if (ReferenceDictionary.ContainsKey(key))
            {
                ISaveable existValue;
                ReferenceDictionary.TryGetValue(key, out existValue);
                val = (T)existValue;
                TraceLogger?.AddMessage($"Value of {typeof(T)} (Id = {existValue.Id}) exists already", TraceLogStatuses.Debug);
            }
            else
            {
                val = ConvertStrategy.ConvertFrom(source);
                ReferenceDictionary.Add(key, val);
                TraceLogger?.AddMessage($"New value of {typeof(T)} (Id = {val.Id}) was added to dictionary", TraceLogStatuses.Debug);
            }
            return val;
        }
        public V ConvertTo(T source)
        {
            ICheckInputData();
            V val;
            var key = (source.Id, typeof(V));
            if (ReferenceDictionary.ContainsKey(key))
            {
                ISaveable existValue;
                ReferenceDictionary.TryGetValue(key, out existValue);
                val = (V)existValue;
                TraceLogger?.AddMessage($"Value of {typeof(V)} (Id = {existValue.Id}) exists already", TraceLogStatuses.Debug);
            }
            else
            {
                val = ConvertStrategy.ConvertTo(source);
                ReferenceDictionary.Add(key, val);
                TraceLogger?.AddMessage($"New value of {typeof(V)} (Id = {val.Id}) was added to dictionary", TraceLogStatuses.Debug);
            }
            return val;
        }
        private void ICheckInputData()
        {
            if(ReferenceDictionary is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": Reference Dictionary";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }
            
    }
}
