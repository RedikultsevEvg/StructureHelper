using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class AbaqusContext : IAbaqusContext
    {
        private readonly Dictionary<Type, object> data = [];
        public IKeywordBuilder Builder { get; } = new KeywordBuilder();

        public void Set<T>(T value) => data[typeof(T)] = value;

        public bool Has(Type type) => data.ContainsKey(type);

        public T Get<T>() => (T)data[typeof(T)];

        public bool TryGet<T>(out T value)
        {
            if (data.TryGetValue(typeof(T), out var obj))
            {
                value = (T)obj;
                return true;
            }

            value = default;
            return false;
        }
    }
}
