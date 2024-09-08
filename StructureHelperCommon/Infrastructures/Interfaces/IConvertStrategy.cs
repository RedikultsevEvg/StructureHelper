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
        IShiftTraceLogger TraceLogger { get; set; }
        V ConvertTo(T source);
        T ConvertFrom(V source);
    }
}
