﻿using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using System;

namespace StructureHelper.Windows.MainWindow.Analyses
{
    public class VisualAnalysis : IVisualAnalysis
    {
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
            if (version.Item is ICrossSection crossSection)
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
    }
}
