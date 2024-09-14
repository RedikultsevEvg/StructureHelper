using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IConvertStrategy<T,V>
        where T :ISaveable
        where V :ISaveable
    {
        Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        IShiftTraceLogger TraceLogger { get; set; }
        T Convert(V source);
    }
}
