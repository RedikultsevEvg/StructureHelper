using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FeaMaterialAnalysisUpdateStrategy : IParentUpdateStrategy<IFeaMaterialAnalysis>
    {
        private IUpdateStrategy<IAnalysis> analysisUpdateStrategy;
        private IUpdateStrategy<IDateVersion> dateUpdateStrategy;
        private IUpdateStrategy<IFeaMaterialRepository> repositoryUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

        private IUpdateStrategy<IAnalysis> AnalysisUpdateStrategy => analysisUpdateStrategy ??= new AnalysisUpdateStrategy();
        private IUpdateStrategy<IDateVersion> DateUpdateStrategy => dateUpdateStrategy ??= new DateVersionUpdateStrategy();
        private IUpdateStrategy<IFeaMaterialRepository> RepositoryUpdateStrategy => repositoryUpdateStrategy ??= new FeaMaterialRepositoryUpdateStrategy() { UpdateChildren = true };
        public void Update(IFeaMaterialAnalysis targetObject, IFeaMaterialAnalysis sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, "Analysis Properties");
            CheckObject.ThrowIfNull(targetObject, sourceObject, "Analysis Properties");
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            AnalysisUpdateStrategy.Update(targetObject, sourceObject);
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(targetObject, sourceObject, "Analysis Properties");
            }
        }
    }
}
