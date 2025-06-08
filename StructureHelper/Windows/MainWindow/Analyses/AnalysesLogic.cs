using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.BeamShears;
using StructureHelper.Windows.MainWindow.Analyses;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.Analyses;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace StructureHelper.Windows.MainWindow
{
    public class AnalysesLogic : ViewModelBase
    {
        private IUpdateStrategy<IAnalysis> updateStrategy = new AnalysisUpdateStrategy();
        private RelayCommand? addAnalyisCommand;
        private RelayCommand? runCommand;
        private RelayCommand? editCommand;
        private RelayCommand? deleteCommand;
        private RelayCommand? copyCommand;
        private RelayCommand versionsCommand;

        public IVisualAnalysis? SelectedAnalysis { get; set; }

        public ObservableCollection<IVisualAnalysis> FilteredAnalyses { get; }
        public RelayCommand AddAnalysisCommand
        {
            get
            {
                return addAnalyisCommand ??= new RelayCommand(obj =>
                {
                    if (obj is AnalysisTypes.CrossSection)
                    {
                        AddCrossSectionNdmAnalysis();
                    }
                    else if (obj is AnalysisTypes.BeamShear)
                    {
                        AddBeamShearAnalysis();
                    }
                    Refresh();
                });
            }
        }
        public RelayCommand RunCommand
        {
            get
            {
                return runCommand ??= new RelayCommand(obj =>
                {
                    RunAnalysis();
                    Refresh();
                },
                b => SelectedAnalysis is not null);
            }
        }
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??= new RelayCommand(obj =>
                {
                    EditAnalysis();
                    Refresh();
                },
                b => SelectedAnalysis is not null);
            }
        }
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??= new RelayCommand(obj =>
                {
                    DeleteAnalysis();
                    Refresh();
                },
                b => SelectedAnalysis is not null);
            }
        }

        public RelayCommand CopyCommand
        {
            get
            {
                return copyCommand ??= new RelayCommand(obj =>
                {
                    CopyCurrentAnalysis();
                },
                b => SelectedAnalysis is not null);
            }
        }

        public RelayCommand VersionsCommand
        {
            get
            {
                return versionsCommand ??= new RelayCommand(obj =>
                {
                    ShowVersions();
                },
                b => SelectedAnalysis is not null);
            }
        }

        private void ShowVersions()
        {
            if (SelectedAnalysis is null) { return; }
            try
            {
                VersionsViewModel viewModel = new(SelectedAnalysis.Analysis.VersionProcessor);
                var wnd = new VersionsView(viewModel);
                wnd.ShowDialog();
                Refresh();
            }
            catch (Exception ex)
            {
                // to do
            }
        }

        private void CopyCurrentAnalysis()
        {
            if (SelectedAnalysis is not null)
            {
                var newAnalysis = SelectedAnalysis.Clone() as IVisualAnalysis;
                newAnalysis.Analysis.Name += " - copy";
                ProgramSetting.CurrentProject.VisualAnalyses.Add(newAnalysis);
                Refresh();
                SelectedAnalysis = newAnalysis;
            }
        }

        public AnalysesLogic()
        {
            FilteredAnalyses = new();
        }
        public void Refresh()
        {
            FilteredAnalyses.Clear();
            var analysesList = ProgramSetting.CurrentProject.VisualAnalyses.ToList();
            foreach (var analysis in analysesList)
            {
                FilteredAnalyses.Add(analysis);
            }
        }
        private void EditAnalysis()
        {
            if (SelectedAnalysis is not null)
            {
                var tmpItem = SelectedAnalysis.Analysis.Clone() as IAnalysis;
                var wnd = new AnalysisView(SelectedAnalysis);
                wnd.ShowDialog();
                if (wnd.DialogResult != true)
                {
                    updateStrategy.Update(SelectedAnalysis.Analysis, tmpItem);
                }
                else
                {
                    ProgramSetting.SetCurrentProjectToNotActual();
                }
            }
        }
        private void DeleteAnalysis()
        {
            if (SelectedAnalysis is not null)
            {
                var dialogResult = MessageBox.Show("Delete analysis?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    ProgramSetting.CurrentProject.VisualAnalyses.Remove(SelectedAnalysis);
                    ProgramSetting.SetCurrentProjectToNotActual();
                }
            }
        }
        private void RunAnalysis()
        {
            if (SelectedAnalysis is null) { return; }
            SelectedAnalysis.ActionToRun = ActionToRun;
            SelectedAnalysis?.Run();
            ProgramSetting.SetCurrentProjectToNotActual();
        }
        private void AddCrossSectionNdmAnalysis()
        {
            CrossSectionNdmAnalysis analysis = new();
            analysis.Name = "New NDM Analysis";
            analysis.Tags = "#New group";
            var visualAnalysis = new VisualAnalysis(analysis);
            ProgramSetting.CurrentProject.VisualAnalyses.Add(visualAnalysis);
            ProgramSetting.SetCurrentProjectToNotActual();
        }

        private void AddBeamShearAnalysis()
        {
            BeamShearAnalysis analysis = new(Guid.NewGuid());
            analysis.Name = "New Beam Shear Analysis";
            analysis.Tags = "#New group";
            VisualAnalysis visualAnalysis = new(analysis);
            ProgramSetting.CurrentProject.VisualAnalyses.Add(visualAnalysis);
            ProgramSetting.SetCurrentProjectToNotActual();
        }

        private void ActionToRun()
        {
            if (SelectedAnalysis is null) { return; }
            var version = SelectedAnalysis.Analysis.VersionProcessor.GetCurrentVersion();
            if (version is null)
            {
                throw new StructureHelperException(ErrorStrings.NullReference);
            }
            if (version.AnalysisVersion is ICrossSection crossSection)
            {
                ProcessCrossSection(crossSection);
            }
            else if (version.AnalysisVersion is IBeamShear beamShear)
            {
                ProcessBeamShear(beamShear);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(version));
            }
        }

        private void ProcessBeamShear(IBeamShear beamShear)
        {
            BeamShearViewModel viewModel = new BeamShearViewModel(beamShear);
            var window = new BeamShearView(viewModel);
            window.ShowDialog();
        }

        private void ProcessCrossSection(ICrossSection crossSection)
        {
            var window = new CrossSectionView(crossSection);
            window.ShowDialog();
        }
    }
}
