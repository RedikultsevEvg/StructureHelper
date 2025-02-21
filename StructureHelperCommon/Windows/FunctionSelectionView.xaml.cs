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

namespace StructureHelperCommon.Windows
{
    /// <summary>
    /// Interaction logic for FunctionSelection.xaml
    /// </summary>
    public partial class FunctionSelectionView : Window
    {
        private FunctionSelectionVM viewModel;
        public FunctionSelectionView(FunctionSelectionVM viewModel)
        {
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            InitializeComponent();
        }
        public FunctionSelectionView() : this(new FunctionSelectionVM())
        {
        }
    }
}
