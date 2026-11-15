using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.History
{
    public sealed class ProjectSnapshot : IProjectSnapshot
    {
        public ProjectSnapshot(Guid projectId, IProject document)
        {
            ProjectId = projectId;
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public Guid ProjectId { get; }
        public IProject Document { get; }
    }
}
