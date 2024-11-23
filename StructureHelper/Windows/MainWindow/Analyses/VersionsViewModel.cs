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

namespace StructureHelper.Windows.MainWindow.Analyses
{
    public class VersionsViewModel : ViewModelBase
    {
        private IVersionProcessor versionProcessor;
        private RelayCommand addNewVersionCommand;
        private RelayCommand returnToVersionCommand;

        public VersionsViewModel(IVersionProcessor versionProcessor)
        {
            this.versionProcessor = versionProcessor;
            Refresh();
        }

        public IDateVersion SelectedVersion { get; set; }
        public ObservableCollection<IDateVersion> DateVersions { get; set; } = new();

        public RelayCommand AddNewVersionCommand
        {
            get
            {
                return addNewVersionCommand ??= new RelayCommand(obj =>
                {
                    AddNewVersion();
                },
                b => SelectedVersion is not null);
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
                b => SelectedVersion is not null);
            }
        }

        private void ReturnToVersion()
        {
            if (SelectedVersion is null) return;
            if (versionProcessor.Versions.Count <= 1)
            {
                MessageBox.Show("It is not possible to delete last version", "There is only 1 version", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            MessageBoxResult result = MessageBox.Show("Please, confirm deleting", "Delete version(s)?", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result != MessageBoxResult.OK) { return; }
            SafetyProcessor.RunSafeProcess(RemovingOfVersion, "Error of deleting of version");
        }

        private void RemovingOfVersion()
        {
            while (SelectedVersion != versionProcessor.GetCurrentVersion())
            {
                versionProcessor
                    .Versions
                    .Remove(versionProcessor.GetCurrentVersion());
            }
            Refresh();
        }

        private void AddNewVersion()
        {
            if (SelectedVersion is null) { return; }
            SafetyProcessor.RunSafeProcess(AddVersion, "Error of adding of new version");
        }

        private void AddVersion()
        {
            var selectedItem = SelectedVersion.AnalysisVersion as ICloneable;
            versionProcessor.AddVersion(selectedItem.Clone() as ISaveable);
            Refresh();
        }

        private void Refresh()
        {
            DateVersions.Clear();
            versionProcessor.Versions.ForEach(x => DateVersions.Add(x));
            SelectedVersion = DateVersions[^1];
        }

    }
}
