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
    /// Interaction logic for ForceCombinationFromFile.xaml
    /// </summary>
    public partial class ForceCombinationFromFileView : Window
    {
        ForceCombinationFromFileVM viewModel;

        public ForceCombinationFromFileView(ForceCombinationFromFileVM viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            InitializeComponent();
            this.DataContext = this.viewModel;
        }

        public ForceCombinationFromFileView(IForceCombinationFromFile model) : this (new ForceCombinationFromFileVM(model))
        {
            
        }

    }
}
