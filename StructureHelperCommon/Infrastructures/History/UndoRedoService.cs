using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Infrastructures.History
{
    /// <summary>
    /// Project-level undo/redo service.
    /// History entries are created only by an explicit transaction Commit().
    /// </summary>
    public sealed class UndoRedoService : IUndoRedoService
    {
        private readonly IProjectSnapshotFactory snapshotFactory;
        private readonly IProjectSnapshotRestorer snapshotRestorer;
        private readonly List<UndoRedoHistoryEntry> undoEntries = new();
        private readonly List<UndoRedoHistoryEntry> redoEntries = new();
        private UndoTransaction? activeTransaction;
        private IProject? project;
        private UndoRedoState state = UndoRedoState.Idle;

        public UndoRedoService(IProject project, int maximumHistoryCount = 50)
            : this(
                new ProjectSnapshotFactory(),
                new ProjectSnapshotRestorer(),
                project,
                maximumHistoryCount)
        {
        }

        public UndoRedoService(
            IProjectSnapshotFactory snapshotFactory,
            IProjectSnapshotRestorer snapshotRestorer,
            IProject project,
            int maximumHistoryCount = 50)
        {
            this.snapshotFactory = snapshotFactory
                ?? throw new ArgumentNullException(nameof(snapshotFactory));
            this.snapshotRestorer = snapshotRestorer
                ?? throw new ArgumentNullException(nameof(snapshotRestorer));
            if (maximumHistoryCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumHistoryCount));
            }

            MaximumHistoryCount = maximumHistoryCount;
            SetProject(project);
        }

        public bool CanUndo => state == UndoRedoState.Idle && undoEntries.Count > 0;
        public bool CanRedo => state == UndoRedoState.Idle && redoEntries.Count > 0;
        public bool IsTransactionActive => activeTransaction is not null;
        public int UndoCount => undoEntries.Count;
        public int RedoCount => redoEntries.Count;
        public UndoRedoState State => state;
        public int MaximumHistoryCount { get; }

        public event EventHandler<UndoRedoEventArgs>? StateChanged;
        public event EventHandler<UndoRedoEventArgs>? DocumentRestored;

        public void SetProject(IProject project)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (activeTransaction is not null)
            {
                throw new InvalidOperationException(
                    "Cannot switch projects while a transaction is active.");
            }

            this.project = project;
            undoEntries.Clear();
            redoEntries.Clear();
            state = UndoRedoState.Idle;
            RaiseStateChanged();
        }

        public IUndoTransaction BeginTransaction(
            string name,
            HistoryScope scope = HistoryScope.Project,
            IReadOnlyCollection<Guid>? affectedAnalysisIds = null)
        {
            if (project is null)
            {
                throw new InvalidOperationException("No project is attached to the undo service.");
            }

            if (state != UndoRedoState.Idle || activeTransaction is not null)
            {
                throw new InvalidOperationException(
                    "A transaction is already active or undo/redo is in progress.");
            }

            var analysisIds = ValidateScope(scope, affectedAnalysisIds);
            var before = snapshotFactory.Create(project);

            activeTransaction = new UndoTransaction(
                this,
                name ?? string.Empty,
                scope,
                analysisIds,
                before);
            state = UndoRedoState.Active;
            RaiseStateChanged();
            return activeTransaction;
        }

        public IReadOnlyList<UndoRedoHistoryEntry> GetHistory(HistoryScopeFilter? filter = null)
        {
            var effectiveFilter = filter ?? HistoryScopeFilter.All;
            return undoEntries
                .Where(effectiveFilter.Matches)
                .ToList()
                .AsReadOnly();
        }

        public void Undo()
        {
            EnsureIdle();
            if (undoEntries.Count == 0)
            {
                return;
            }

            var entry = undoEntries[^1];
            state = UndoRedoState.UndoRedoInProgress;
            RaiseStateChanged();
            try
            {
                snapshotRestorer.RestoreInto(RequireProject(), entry.Before);
                undoEntries.RemoveAt(undoEntries.Count - 1);
                redoEntries.Add(entry);
                project!.IsActual = false;
            }
            finally
            {
                state = UndoRedoState.Idle;
            }

            RaiseDocumentRestored(entry);
            RaiseStateChanged();
        }

        public void Redo()
        {
            EnsureIdle();
            if (redoEntries.Count == 0)
            {
                return;
            }

            var entry = redoEntries[^1];
            state = UndoRedoState.UndoRedoInProgress;
            RaiseStateChanged();
            try
            {
                snapshotRestorer.RestoreInto(RequireProject(), entry.After);
                redoEntries.RemoveAt(redoEntries.Count - 1);
                undoEntries.Add(entry);
                TrimHistory();
                project!.IsActual = false;
            }
            finally
            {
                state = UndoRedoState.Idle;
            }

            RaiseDocumentRestored(entry);
            RaiseStateChanged();
        }

        public void Clear()
        {
            if (activeTransaction is not null)
            {
                throw new InvalidOperationException("Cannot clear history while a transaction is active.");
            }

            undoEntries.Clear();
            redoEntries.Clear();
            state = UndoRedoState.Idle;
            RaiseStateChanged();
        }

        public void MarkSaved()
        {
            if (project is null)
            {
                return;
            }

            project.IsActual = true;
        }

        private void Commit(UndoTransaction transaction)
        {
            if (!ReferenceEquals(activeTransaction, transaction))
            {
                throw new InvalidOperationException("The transaction is not the active transaction.");
            }

            state = UndoRedoState.Committing;
            RaiseStateChanged();

            IProjectSnapshot after;
            try
            {
                after = snapshotFactory.Create(RequireProject());
                if (after.ProjectId != transaction.Before.ProjectId)
                {
                    throw new InvalidOperationException(
                        "The project ID changed while a transaction was active.");
                }
            }
            catch
            {
                state = UndoRedoState.Active;
                RaiseStateChanged();
                throw;
            }

            var entry = new UndoRedoHistoryEntry(
                Guid.NewGuid(),
                transaction.Name,
                DateTimeOffset.Now,
                transaction.Scope,
                transaction.AffectedAnalysisIds,
                transaction.Before,
                after);

            undoEntries.Add(entry);
            TrimHistory();
            redoEntries.Clear();
            project!.IsActual = false;

            activeTransaction = null;
            transaction.MarkCommitted();
            state = UndoRedoState.Idle;
            RaiseStateChanged();
        }

        private void Rollback(UndoTransaction transaction)
        {
            if (!ReferenceEquals(activeTransaction, transaction))
            {
                throw new InvalidOperationException("The transaction is not the active transaction.");
            }

            state = UndoRedoState.RollingBack;
            RaiseStateChanged();
            try
            {
                snapshotRestorer.RestoreInto(RequireProject(), transaction.Before);
                project!.IsActual = false;
                activeTransaction = null;
                transaction.MarkRolledBack();
            }
            finally
            {
                state = UndoRedoState.Idle;
            }

            RaiseStateChanged();
        }

        private IProject RequireProject()
        {
            return project
                ?? throw new InvalidOperationException("No project is attached to the undo service.");
        }

        private void EnsureIdle()
        {
            if (state != UndoRedoState.Idle || activeTransaction is not null)
            {
                throw new InvalidOperationException(
                    "Undo/redo cannot run while a transaction is active or another restore is in progress.");
            }
        }

        private void TrimHistory()
        {
            while (undoEntries.Count > MaximumHistoryCount)
            {
                undoEntries.RemoveAt(0);
            }

            while (redoEntries.Count > MaximumHistoryCount)
            {
                redoEntries.RemoveAt(0);
            }
        }

        private static IReadOnlyList<Guid> ValidateScope(
            HistoryScope scope,
            IReadOnlyCollection<Guid>? affectedAnalysisIds)
        {
            var ids = affectedAnalysisIds?
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToArray() ?? Array.Empty<Guid>();

            switch (scope)
            {
                case HistoryScope.Project:
                    if (ids.Length != 0)
                    {
                        throw new ArgumentException(
                            "Project scope cannot contain affected analysis IDs.",
                            nameof(affectedAnalysisIds));
                    }
                    break;

                case HistoryScope.Analysis:
                    if (ids.Length != 1)
                    {
                        throw new ArgumentException(
                            "Analysis scope requires exactly one affected analysis ID.",
                            nameof(affectedAnalysisIds));
                    }
                    break;

                case HistoryScope.MultipleAnalyses:
                    if (ids.Length < 2)
                    {
                        throw new ArgumentException(
                            "MultipleAnalyses scope requires at least two distinct analysis IDs.",
                            nameof(affectedAnalysisIds));
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(scope));
            }

            return Array.AsReadOnly(ids);
        }

        private void RaiseStateChanged()
        {
            StateChanged?.Invoke(this, new UndoRedoEventArgs(state));
        }

        private void RaiseDocumentRestored(UndoRedoHistoryEntry entry)
        {
            DocumentRestored?.Invoke(this, new UndoRedoEventArgs(state, entry));
        }

        private sealed class UndoTransaction : IUndoTransaction
        {
            private readonly UndoRedoService owner;
            private bool completed;

            public UndoTransaction(
                UndoRedoService owner,
                string name,
                HistoryScope scope,
                IReadOnlyList<Guid> affectedAnalysisIds,
                IProjectSnapshot before)
            {
                this.owner = owner;
                Id = Guid.NewGuid();
                Name = name ?? string.Empty;
                Scope = scope;
                AffectedAnalysisIds = affectedAnalysisIds;
                Before = before;
            }

            public Guid Id { get; }
            public string Name { get; }
            public HistoryScope Scope { get; }
            public IReadOnlyList<Guid> AffectedAnalysisIds { get; }
            public IProjectSnapshot Before { get; }
            public bool IsCommitted { get; private set; }

            public void Commit()
            {
                if (completed)
                {
                    throw new InvalidOperationException("The transaction is already completed.");
                }

                owner.Commit(this);
            }

            public void Rollback()
            {
                if (completed)
                {
                    throw new InvalidOperationException("The transaction is already completed.");
                }

                owner.Rollback(this);
            }

            public void Dispose()
            {
                if (!completed)
                {
                    Rollback();
                }
            }

            public void MarkCommitted()
            {
                IsCommitted = true;
                completed = true;
            }

            public void MarkRolledBack()
            {
                IsCommitted = false;
                completed = true;
            }
        }
    }
}
