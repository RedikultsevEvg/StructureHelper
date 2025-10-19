using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.Forces
{
    public class ForceCombinationFromFileVM : ForceActionVMBase
    {
        private readonly IForceCombinationFromFile model;
        private ListOfFilesVM files;
        private FactoredCombinationPropertyVM combinationProperty;
        private RelayCommand showCombinationViewerCommand;

        public FactoredCombinationPropertyVM CombinationProperty
        {
            get => combinationProperty;
            set
            {
                combinationProperty = value;
                OnPropertyChanged(nameof(CombinationProperty));
            }
        }

        public bool SkipWrongRows
        {
            get
            {
                return model.SkipWrongRows;
            }
            set
            {
                model.SkipWrongRows = value;
                OnPropertyChanged(nameof(SkipWrongRows));
            }
        }

        public ICommand ShowCombinationViewerCommand
        {
            get => showCombinationViewerCommand ??= new RelayCommand(
                o =>SafetyProcessor.RunSafeProcess(ShowCombinationViewer, "Combination viewer"));
        }

        private void ShowCombinationViewer()
        {
            var wnd = new ForceCombinationViewerView(model);
            wnd.ShowDialog();
        }

        public ListOfFilesVM Files
        {
            get => files; set
            {
                files = value;
                OnPropertyChanged();
            }
        }

        public ForceCombinationFromFileVM(IForceCombinationFromFile model) : base(model)
        {
            this.model = model;
            files = new(this.model.ForceFiles);
            CombinationProperty = new FactoredCombinationPropertyVM(model.CombinationProperty);
        }

    }
}
