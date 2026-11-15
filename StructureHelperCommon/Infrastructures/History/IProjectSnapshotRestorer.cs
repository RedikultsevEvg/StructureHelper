using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.History
{
    public interface IProjectSnapshotRestorer
    {
        void RestoreInto(IProject target, IProjectSnapshot snapshot);
    }
}
