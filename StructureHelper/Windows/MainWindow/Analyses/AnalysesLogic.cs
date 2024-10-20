using StructureHelper.Infrastructure;
using StructureHelper.Windows.MainWindow.Analyses;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelper.Windows.MainWindow
{
    public class AnalysesLogic : ViewModelBase
    {
        private RelayCommand? addAnalyisCommand;
        private RelayCommand? runCommand;
        private RelayCommand? editCommand;
        private RelayCommand? deleteCommand;

        public IVisualAnalysis? SelectedAnalysis { get; set; }

        public ObservableCollection<IVisualAnalysis> FilteredAnalyses { get; }
        public RelayCommand AddAnalysisCommand
        {
            get
            {
                return addAnalyisCommand ??= new RelayCommand(obj =>
                {
                    AddCrossSectionNdmAnalysis();
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
                var name = SelectedAnalysis.Analysis.Name;
                var tags = SelectedAnalysis.Analysis.Tags;
                var wnd = new AnalysisView(SelectedAnalysis);
                wnd.ShowDialog();
                if (wnd.DialogResult != true)
                {
                    SelectedAnalysis.Analysis.Name = name;
                    SelectedAnalysis.Analysis.Tags = tags;
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
                }
            }
        }
        private void RunAnalysis()
        {
            if (SelectedAnalysis is null) { return; }
            SelectedAnalysis.ActionToRun = ActionToRun;
            SelectedAnalysis?.Run();
        }
        private void AddCrossSectionNdmAnalysis()
        {
            var analysis = new CrossSectionNdmAnalysis();
            analysis.Name = "New NDM Analysis";
            analysis.Tags = "#New group";
            var visualAnalysis = new VisualAnalysis(analysis);
            ProgramSetting.CurrentProject.VisualAnalyses.Add(visualAnalysis);
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
