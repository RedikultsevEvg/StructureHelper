using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StructureHelper.Windows.BeamShears
{
    /// <summary>
    /// Interaction logic for StirrupByRebarView.xaml
    /// </summary>
    public partial class StirrupByRebarView : Window
    {
        private StirrupByRebarViewModel viewModel;
        public StirrupByRebarView(StirrupByRebarViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            this.DataContext = this.viewModel;
        }
        public StirrupByRebarView(IStirrupByRebar stirrupByRebar) : this(new StirrupByRebarViewModel(stirrupByRebar))
        {
            
        }
    }
}
