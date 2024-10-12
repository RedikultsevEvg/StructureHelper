using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IBaseConvertStrategy
    {
        Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        IShiftTraceLogger TraceLogger { get; set; }
    }
}