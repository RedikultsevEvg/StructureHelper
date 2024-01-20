using StructureHelper.Infrastructure;
using StructureHelper.Windows.AddMaterialWindow;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class SafetyFactorsViewModel : SelectItemVM<IMaterialSafetyFactor>
    {
        List<IMaterialSafetyFactor> safetyFactors;
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

        private ICommand showSafetyFactors;

        public ICommand ShowSafetyFactors
        {
            get
            {
                return showSafetyFactors ??= new RelayCommand(o =>
                {
                    var wnd = new SafetyFactorsView(safetyFactors);
                    wnd.ShowDialog();
                    Refresh();
                }
                );
            }
        }

        public override void AddMethod(object parameter)
        {
            NewItem = new MaterialSafetyFactor();
            base.AddMethod(parameter);
        }

        public SafetyFactorsViewModel(List<IMaterialSafetyFactor> safetyFactors) : base(safetyFactors)
        {
            this.safetyFactors = safetyFactors;
        }
    }
}
