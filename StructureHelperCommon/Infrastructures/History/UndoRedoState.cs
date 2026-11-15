using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Infrastructures.History
{
    public enum UndoRedoState
    {
        Idle = 0,
        Active = 1,
        Committing = 2,
        RollingBack = 3,
        UndoRedoInProgress = 4,
        Disposed = 5
    }
}
