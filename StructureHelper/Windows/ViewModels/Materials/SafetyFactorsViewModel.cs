using StructureHelper.Infrastructure;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class SafetyFactorsViewModel : CRUDViewModelBase<IMaterialSafetyFactor>
    {
        private RelayCommand showPartialCommand;

        public RelayCommand ShowPartialFactors
        {
            get
            {
                return showPartialCommand ??
                    (showPartialCommand = new RelayCommand(o =>
                    {
                        var wnd = new PartialFactorsView(SelectedItem.PartialFactors);
                        wnd.ShowDialog();
                    }, o => SelectedItem != null
                    ));
            }
        }

        public override void AddMethod(object parameter)
        {
            NewItem = new MaterialSafetyFactor();
            base.AddMethod(parameter);
        }

        public SafetyFactorsViewModel(List<IMaterialSafetyFactor> safetyFactors) : base(safetyFactors)
        {
        }
    }
}
