using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Forces;
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

namespace StructureHelper.Windows.Forces
{
    /// <summary>
    /// Логика взаимодействия для ForceCombinationView.xaml
    /// </summary>
    public partial class ForceCombinationView : Window
    {
        ForceCombinationViewModel viewModel;
        public ForceCombinationView(ForceCombinationViewModel _viewModel)
        {
            viewModel = _viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        public ForceCombinationView(IForceCombinationList combinationList)
        {
            viewModel = new ForceCombinationViewModel(combinationList);
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
