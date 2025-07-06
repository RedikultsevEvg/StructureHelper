using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для StirrupByInclinedReebarView.xaml
    /// </summary>
    public partial class StirrupByInclinedReebarView : Window
    {
        private StirrupByInclinedRebarViewModel viewModel;
        public StirrupByInclinedReebarView(StirrupByInclinedRebarViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            viewModel.ParentWindow = this;
        }

        public StirrupByInclinedReebarView(IStirrupByInclinedRebar stirrupByInclinedRebar) : this(new StirrupByInclinedRebarViewModel(stirrupByInclinedRebar))
        {
            
        }
    }
}
