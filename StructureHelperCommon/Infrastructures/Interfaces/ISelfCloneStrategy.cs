using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface ISelfCloneStrategy<T> where T : class
    {
        T GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep);
    }
}
