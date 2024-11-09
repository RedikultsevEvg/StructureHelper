using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IDictionaryConvertStrategy<T, V> : IConvertStrategy<T, V>
        where T : ISaveable
        where V : ISaveable
    {
        IConvertStrategy<T, V> ConvertStrategy { get; set; }
    }
}