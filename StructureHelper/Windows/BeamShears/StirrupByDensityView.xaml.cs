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
    /// Interaction logic for StirrupByDensityView.xaml
    /// </summary>
    public partial class StirrupByDensityView : Window
    {
        private readonly StirrupByDensityViewModel viewModel;
        public StirrupByDensityView(StirrupByDensityViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
        }
        public StirrupByDensityView(IStirrupByDensity stirrupByDensity) : this(new StirrupByDensityViewModel(stirrupByDensity))
        {
            
        }
    }
}
