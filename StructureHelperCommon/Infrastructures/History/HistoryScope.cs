using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Infrastructures.History
{
    /// <summary>
    /// Describes which part of the project a history entry belongs to.
    /// Scope is metadata only; it never creates a separate undo sequence.
    /// </summary>
    public enum HistoryScope
    {
        Project = 0,
        Analysis = 1,
        MultipleAnalyses = 2
    }

    /// <summary>
    /// Filter used by the read-only history panel.
    /// The filter is applied to the single global history stack.
    /// </summary>
    public sealed class HistoryScopeFilter
    {
        public bool IncludeProjectScope { get; }
        public bool ProjectOnlyScope { get; }
        public Guid? AnalysisId { get; }

        public HistoryScopeFilter(bool includeProjectScope = true, Guid? analysisId = null, bool projectOnlyScope = false)
        {
            IncludeProjectScope = includeProjectScope;
            AnalysisId = analysisId;
            ProjectOnlyScope = projectOnlyScope;
        }

        public static HistoryScopeFilter All => new(includeProjectScope: true);

        public static HistoryScopeFilter ProjectOnly => new(includeProjectScope: true, projectOnlyScope: true);

        public static HistoryScopeFilter ForAnalysis(Guid analysisId, bool includeProjectScope = true)
            => new(includeProjectScope, analysisId);

        public bool Matches(UndoRedoHistoryEntry entry)
        {
            if (entry is null)
            {
                return false;
            }

            if (ProjectOnlyScope)
            {
                return entry.Scope == HistoryScope.Project;
            }

            if (AnalysisId is null)
            {
                return true;
            }

            if (entry.Scope == HistoryScope.Project)
            {
                return IncludeProjectScope;
            }

            return entry.AffectedAnalysisIds.Contains(AnalysisId.Value);
        }
    }

    /// <summary>
    /// Immutable information about one committed transaction.
    /// </summary>
    public sealed class UndoRedoHistoryEntry
    {
        public UndoRedoHistoryEntry(
            Guid id,
            string name,
            DateTimeOffset createdAt,
            HistoryScope scope,
            IReadOnlyList<Guid> affectedAnalysisIds,
            IProjectSnapshot before,
            IProjectSnapshot after)
        {
            Id = id;
            Name = name ?? string.Empty;
            CreatedAt = createdAt;
            Scope = scope;
            AffectedAnalysisIds = affectedAnalysisIds ?? Array.Empty<Guid>();
            Before = before;
            After = after;
        }

        public Guid Id { get; }
        public string Name { get; }
        public DateTimeOffset CreatedAt { get; }
        public HistoryScope Scope { get; }
        public IReadOnlyList<Guid> AffectedAnalysisIds { get; }
        public IProjectSnapshot Before { get; }
        public IProjectSnapshot After { get; }
    }
}
