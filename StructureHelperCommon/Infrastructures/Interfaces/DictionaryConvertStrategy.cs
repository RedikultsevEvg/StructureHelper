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

        public DictionaryConvertStrategy(IBaseConvertStrategy baseConvertStrategy, IConvertStrategy<T, V> convertStrategy)
        {
            ReferenceDictionary = baseConvertStrategy.ReferenceDictionary;
            TraceLogger = baseConvertStrategy.TraceLogger;
            ConvertStrategy = convertStrategy;
        }
        public DictionaryConvertStrategy()
        {
            
        }

        public T Convert(V source)
        {
            CheckInputData();
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
                val = ConvertStrategy.Convert(source);
                ReferenceDictionary.Add(key, val);
                TraceLogger?.AddMessage($"New value of {typeof(T)} (Id = {val.Id}) was added to dictionary", TraceLogStatuses.Debug);
            }
            return val;
        }
        private void CheckInputData()
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
