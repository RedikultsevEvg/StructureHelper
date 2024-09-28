using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using System;

namespace StructureHelper.Windows.MainWindow.Analyses
{
    public class VisualAnalysis : IVisualAnalysis
    {
        private IUpdateStrategy<IVisualAnalysis> updateStrategy = new VisualAnalysisUpdateStrategy();
        public Guid Id { get; }
        public IAnalysis Analysis { get; set; }


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
            var version = Analysis.VersionProcessor.GetCurrentVersion();
            if (version is null)
            {
                throw new StructureHelperException(ErrorStrings.NullReference);
            }
            if (version.AnalysisVersion is ICrossSection crossSection)
            {
                ProcessCrossSection(crossSection);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(version));
            }
        }

        private void ProcessCrossSection(ICrossSection crossSection)
        {
            var window = new CrossSectionView(crossSection);
            window.ShowDialog();
        }

        public object Clone()
        {
                var newAnalysis = Analysis.Clone() as IAnalysis;
                VisualAnalysis newItem = new(newAnalysis);
                return newItem;
        }
    }
}
