using StructureHelperCommon.Models;
using System.Collections.Generic;

namespace StructureHelperCommon.Models
{
    public interface ITraceEntityFactory<T> where T : class
    {
        IEnumerable<T>? Collection { get; set; }
        int Priority { get; set; }

        List<ITraceLoggerEntry> GetTraceEntries();
    }
}