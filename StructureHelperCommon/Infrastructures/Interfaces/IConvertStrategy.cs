using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IConvertStrategy<T,V> : IBaseConvertStrategy
        where T :ISaveable
        where V :ISaveable
    {
        T Convert(V source);
    }
}
