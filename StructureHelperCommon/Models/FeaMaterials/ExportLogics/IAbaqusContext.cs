using StructureHelperCommon.Models.ScriptExports;
using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IAbaqusContext
    {
        IKeywordBuilder Builder { get; }

        T Get<T>();
        bool Has(Type type);
        void Set<T>(T value);
        bool TryGet<T>(out T value);
    }
}