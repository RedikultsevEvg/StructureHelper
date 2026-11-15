using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Infrastructures.History
{
    /// <summary>
    /// Creates an independent project snapshot. Snapshot content uses preserved IDs and
    /// preserved internal references, but never references a live project object.
    /// </summary>
    public sealed class ProjectSnapshotFactory : IProjectSnapshotFactory
    {
        private readonly ICloneStrategy<IProject> projectCloneStrategy;

        public ProjectSnapshotFactory() : this(new ProjectCloneStrategy())
        {
        }

        public ProjectSnapshotFactory(ICloneStrategy<IProject> projectCloneStrategy)
        {
            this.projectCloneStrategy = projectCloneStrategy ?? throw new ArgumentNullException(nameof(projectCloneStrategy));
        }

        public IProjectSnapshot Create(IProject project)
        {
            CheckObject.ThrowIfNull(project);
            var document = projectCloneStrategy.GetClone(
                project,
                CloneMode.PreserveIdentity,
                CloneDepth.PreserveReference);

            // File and dirty flags are not part of the undoable document.
            document.FullFileName = string.Empty;
            document.IsActual = true;

            return new ProjectSnapshot(project.Id, document);
        }
    }
}
