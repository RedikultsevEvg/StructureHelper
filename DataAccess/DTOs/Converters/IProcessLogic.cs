using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public interface IProcessLogic<T>
    {
        Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        T Source { get; set; }
        T Target { get; set; }
        IShiftTraceLogger TraceLogger { get; set; }
        void Process();
    }
}
