using StructureHelper.Infrastructure;
using StructureHelper.Windows.MainWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public class HelpLogic : ViewModelBase
    {
        private RelayCommand showAbout;

        public RelayCommand ShowAbout
        {
            get
            {
                return showAbout ??
                    (
                    showAbout = new RelayCommand(param =>
                    {
                        var wnd = new AboutView();
                        wnd.ShowDialog();
                    }
                    ));
            }
        }
    }
}
