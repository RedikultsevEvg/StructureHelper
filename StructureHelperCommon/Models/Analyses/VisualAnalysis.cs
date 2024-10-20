using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using System;

namespace StructureHelperCommon.Models.Analyses
{
    public class VisualAnalysis : IVisualAnalysis
    {
        private IUpdateStrategy<IVisualAnalysis> updateStrategy = new VisualAnalysisUpdateStrategy();
        public Guid Id { get; }
        public IAnalysis Analysis { get; set; }
        public Action ActionToRun { get; set; }

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

        public object Clone()
        {
                var newAnalysis = Analysis.Clone() as IAnalysis;
                VisualAnalysis newItem = new(newAnalysis);
                return newItem;
        }
    }
}
