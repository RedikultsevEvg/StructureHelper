using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface ICloningStrategy
    {
        T Clone<T>(T original, ICloneStrategy<T>? cloneStrategy = null) where T : class;
    }
}
