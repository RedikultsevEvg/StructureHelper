using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Analyses
{
    public class VisualAnalysis : IVisualAnalysis
    {
        private IUpdateStrategy<IVisualAnalysis> updateStrategy = new VisualAnalysisUpdateStrategy();
        public Guid Id { get; }
        public IAnalysis Analysis { get; set; }
        public Action ActionToRun { get; set; }

        public VisualAnalysis(Guid id)
            : this(id, null!)
        {
        }

        public VisualAnalysis(Guid id, IAnalysis analysis)
        {
            Id = id;
            Analysis = analysis;
        }

        public VisualAnalysis(IAnalysis analysis) : this (Guid.NewGuid(), analysis)
        {
            
        }

        public void Run()
        {
            ActionToRun?.Invoke();
        }

        public IVisualAnalysis GetClone(IVisualAnalysis sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            var newAnalysis = sourceObject.Analysis.GetClone(sourceObject.Analysis, cloneMode, cloneDepth);
            VisualAnalysis newItem = new(newAnalysis);
            return newItem;
        }
    }
}
