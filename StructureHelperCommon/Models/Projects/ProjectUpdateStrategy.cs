using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Projects
{
    public class ProjectUpdateStrategy : IUpdateStrategy<IProject>
    {
        private IUpdateStrategy<IVisualAnalysis> updateStrategy;

        public ProjectUpdateStrategy(IUpdateStrategy<IVisualAnalysis> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ProjectUpdateStrategy() : this(new VisualAnalysisUpdateStrategy())
        {
            
        }

        public void Update(IProject targetObject, IProject sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.VisualAnalyses.Clear();
            foreach (var item in sourceObject.VisualAnalyses)
            {
                targetObject.VisualAnalyses.Add(item.Clone() as IVisualAnalysis);
            }
        }
    }
}
