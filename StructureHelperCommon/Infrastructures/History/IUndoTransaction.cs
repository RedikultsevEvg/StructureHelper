using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.History
{
    public interface IUndoTransaction : IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        HistoryScope Scope { get; }
        IReadOnlyList<Guid> AffectedAnalysisIds { get; }
        bool IsCommitted { get; }

        void Commit();
        void Rollback();
    }
}
