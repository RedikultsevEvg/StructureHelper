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
    /// Interaction logic for ForceCombinationViewerView.xaml
    /// </summary>
    public partial class ForceCombinationViewerView : Window
    {
        ForceCombinationViewerVM viewModel;
        public ForceCombinationViewerView(ForceCombinationViewerVM viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
        }

        public ForceCombinationViewerView(IForceAction action) : this (new ForceCombinationViewerVM(action))
        {
        }
    }
}
