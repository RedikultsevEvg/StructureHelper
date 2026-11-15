using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.History
{
    /// <summary>
    /// Restores a snapshot into the existing project instance while retaining its root identity.
    /// </summary>
    public sealed class ProjectSnapshotRestorer : IProjectSnapshotRestorer
    {
        private readonly ICloneStrategy<IProject> projectCloneStrategy;

        public ProjectSnapshotRestorer()
            : this(new ProjectCloneStrategy())
        {
        }

        public ProjectSnapshotRestorer(ICloneStrategy<IProject> projectCloneStrategy)
        {
            this.projectCloneStrategy = projectCloneStrategy
                ?? throw new ArgumentNullException(nameof(projectCloneStrategy));
        }

        public void RestoreInto(IProject target, IProjectSnapshot snapshot)
        {
            CheckObject.ThrowIfNull(target);
            CheckObject.ThrowIfNull(snapshot);
            if (target.Id != snapshot.ProjectId)
            {
                throw new InvalidOperationException(
                    "A project snapshot can only be restored into the project with the same ID.");
            }

            var document = projectCloneStrategy.GetClone(
                snapshot.Document,
                CloneMode.PreserveIdentity,
                CloneDepth.PreserveReference);

            target.VisualAnalyses.Clear();
            foreach (var analysis in document.VisualAnalyses)
            {
                target.VisualAnalyses.Add(analysis);
            }
        }
    }
}
