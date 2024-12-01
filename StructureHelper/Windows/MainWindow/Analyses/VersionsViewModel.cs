using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Windows;
using StructureHelper.Windows.ViewModels.Errors;
using System.Windows.Input;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;

namespace StructureHelper.Windows.MainWindow.Analyses
{
    public class VersionsViewModel : SelectItemVM<IDateVersion>
    {
        private const string errorOfDeleting = "Error of deleting of version";
        private IVersionProcessor versionProcessor;
        private RelayCommand addNewVersionCommand;
        private RelayCommand returnToVersionCommand;
        private RelayCommand deleteVersionCommand;
        private RelayCommand exportToNewCommand;

        public VersionsViewModel(IVersionProcessor versionProcessor) : base(versionProcessor.Versions)
        {
            this.versionProcessor = versionProcessor;
        }

        public RelayCommand AddNewVersionCommand
        {
            get
            {
                return addNewVersionCommand ??= new RelayCommand(obj =>
                {
                    AddNewVersion();
                },
                b => SelectedItem is not null);
            }
        }

        public RelayCommand ReturnToVersionCommand
        {
            get
            {
                return returnToVersionCommand ??= new RelayCommand(obj =>
                {
                    ReturnToVersion();
                },
                b => SelectedItem is not null);
            }
        }

        public RelayCommand DeleteVersionCommand
        {
            get
            {
                return deleteVersionCommand ??= new RelayCommand(obj =>
                {
                    DeleteVersion();
                },
                b => SelectedItem is not null);
            }
        }
        public RelayCommand ExportToNewCommand
        {
            get
            {
                return exportToNewCommand ??= new RelayCommand(obj =>
                {
                    ExportToNew();
                },
                b => SelectedItem is not null);
            }
        }

        private void ExportToNew()
        {
            if (SelectedItem is null) return;
            SafetyProcessor.RunSafeProcess(ExportingToNew, "Error of export");
        }

        private void ExportingToNew()
        {
            if (SelectedItem.AnalysisVersion is ICrossSection oldCrossSection)
            {
                ProcessCrossSectionAnalysis(oldCrossSection);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(SelectedItem.AnalysisVersion));
            }
            ProgramSetting.SetCurrentProjectToNotActual();
            Refresh();
        }

        private void ProcessCrossSectionAnalysis(ICrossSection oldCrossSection)
        {
            string newComment = "Exported version from: " + SelectedItem.DateTime;
            ICrossSection newCrossSection = oldCrossSection.Clone() as ICrossSection;
            ICrossSectionNdmAnalysis newAnalysis = new CrossSectionNdmAnalysis()
            {
                Name = "New NDM Analysis",
                Comment = newComment,
            };
            newAnalysis.VersionProcessor.AddVersion(newCrossSection);
            var visualAnalysis = new VisualAnalysis(newAnalysis);
            ProgramSetting.CurrentProject.VisualAnalyses.Add(visualAnalysis);
        }

        private void DeleteVersion()
        {
            if (SelectedItem is null) return;
            if (CheckIfOnceVersion() == true) { return; }
            if (ConfirmDeletenig() != true) { return; }
            SafetyProcessor.RunSafeProcess(DeletingOfVersion, errorOfDeleting);
        }

        private void DeletingOfVersion()
        {
            versionProcessor.Versions.Remove(SelectedItem);
            Refresh();
        }

        private void ReturnToVersion()
        {
            if (SelectedItem is null) return;
            if (CheckIfOnceVersion() == true) { return; }
            if (ConfirmDeletenig() != true) { return; }
            SafetyProcessor.RunSafeProcess(RemovingOfVersion, errorOfDeleting);
        }

        private bool ConfirmDeletenig()
        {
            MessageBoxResult result = MessageBox.Show("Please, confirm deleting", "Delete version(s)?", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.OK) { return true; }
            return false;
        }

        private bool CheckIfOnceVersion()
        {
            if (versionProcessor.Versions.Count <= 1)
            {
                MessageBox.Show("It is not possible to delete last version", "There is only 1 version", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return true;
            }
            return false;
        }

        private void RemovingOfVersion()
        {
            while (SelectedItem != versionProcessor.GetCurrentVersion())
            {
                versionProcessor
                    .Versions
                    .Remove(versionProcessor.GetCurrentVersion());
            }
            Refresh();
        }

        private void AddNewVersion()
        {
            if (SelectedItem is null) { return; }
            SafetyProcessor.RunSafeProcess(AddVersion, "Error of adding of new version");
        }

        private void AddVersion()
        {
            var selectedItem = SelectedItem.AnalysisVersion as ICloneable;
            versionProcessor.AddVersion(selectedItem.Clone() as ISaveable);
            Refresh();
        }

        private new void Refresh()
        {
            Items.Clear();
            versionProcessor.Versions.ForEach(x => Items.Add(x));
            SelectedItem = Items[^1];
        }

    }
}
