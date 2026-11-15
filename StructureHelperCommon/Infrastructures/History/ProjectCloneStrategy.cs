using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Infrastructures.History
{
    public sealed class ProjectCloneStrategy : ICloneStrategy<IProject>
    {
        public IProject GetClone(IProject sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference)
            {
                return sourceObject;
            }

            // Prefer a single graph clone so shared references between analyses are
            // preserved. PreserveReference is a deep clone with reference identity,
            // not a shallow clone.
            if (cloneDepth == CloneDepth.Deep || cloneDepth == CloneDepth.PreserveReference)
            {
                try
                {
                    var graphClone = CloneFallback.GetClone(
                        sourceObject,
                        cloneMode,
                        CloneDepth.Deep);
                    if (graphClone is IProject projectClone)
                    {
                        return projectClone;
                    }
                }
                catch
                {
                    // Fall through to the explicit analysis-by-analysis clone.
                    // This keeps snapshot creation usable for unusual project content.
                }
            }

            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            var clone = new Project(id)
            {
                FullFileName = sourceObject.FullFileName,
                IsActual = sourceObject.IsActual
            };

            foreach (var analysis in sourceObject.VisualAnalyses)
            {
                clone.VisualAnalyses.Add(analysis.GetClone(analysis, cloneMode, cloneDepth));
            }

            return clone;
        }
    }
}
