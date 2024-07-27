using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IDecorator<T> where T : class
    {
        T SourceValue { get; }
        T GetModifiedValue();
    }
}
