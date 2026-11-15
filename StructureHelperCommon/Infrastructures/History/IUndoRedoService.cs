using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.History
{
    public interface IUndoRedoService
    {
        bool CanUndo { get; }
        bool CanRedo { get; }
        bool IsTransactionActive { get; }
        int UndoCount { get; }
        int RedoCount { get; }
        UndoRedoState State { get; }
        int MaximumHistoryCount { get; }

        event EventHandler<UndoRedoEventArgs>? StateChanged;
        event EventHandler<UndoRedoEventArgs>? DocumentRestored;

        void SetProject(IProject project);
        IUndoTransaction BeginTransaction(
            string name,
            HistoryScope scope = HistoryScope.Project,
            IReadOnlyCollection<Guid>? affectedAnalysisIds = null);
        IReadOnlyList<UndoRedoHistoryEntry> GetHistory(HistoryScopeFilter? filter = null);
        void Undo();
        void Redo();
        void Clear();
        void MarkSaved();
    }
}
