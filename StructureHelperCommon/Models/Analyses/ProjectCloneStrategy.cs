using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Analyses
{
    public class ProjectCloneStrategy : ICloneStrategy<IProject>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly ICloneStrategy<IVisualAnalysis> analysisCloneStrategy;
        private CloneMode cloneMode;
        private CloneDepth cloneDepth;
        private IProject sourceObject;
        private Project project;

        public ProjectCloneStrategy(ICloningStrategy cloningStrategy, ICloneStrategy<IVisualAnalysis> versionCloneStrategy = null)
        {
            this.cloningStrategy = cloningStrategy;
            this.analysisCloneStrategy = versionCloneStrategy;
        }

        public IProject GetClone(IProject sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject, "Source project");
            CheckObject.ThrowIfNull(sourceObject.VisualAnalyses, "Source visual processors");
            CheckObject.ThrowIfNull(cloningStrategy);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            if (cloneMode == CloneMode.NewEntity || cloneMode == CloneMode.PreserveIdentity)
            {
                this.cloneMode = cloneMode;
                this.cloneDepth = cloneDepth;
                this.sourceObject = sourceObject;
                GetProject();
                return project;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cloneMode) + $": Project cloning, clone mode {cloneMode} is not correct");
            }
        }

        private void GetProject()
        {
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            project = new(id);
            new ProjectUpdateStrategy() { UpdateChildren = false }.Update(project, sourceObject);
            project.VisualAnalyses.Clear();
            foreach (var analysis in sourceObject.VisualAnalyses)
            {
                IVisualAnalysis visualAnalysis = cloningStrategy.Clone(analysis, analysisCloneStrategy, cloneMode);
                project.VisualAnalyses.Add(visualAnalysis);
            }
        }
    }
}
