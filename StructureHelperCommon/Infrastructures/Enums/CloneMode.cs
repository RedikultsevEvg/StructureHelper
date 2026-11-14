using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.Enums
{
    public enum CloneMode
    {
        NewEntity = 0,
        PreserveIdentity = 1,
        Reference = 2,
    }

    public enum CloneDepth
    {
        Shallow = 0,
        Deep = 1,
        PreserveReference = 2,
        DeleteChildren = 3,
    }
}
