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
    /// Interaction logic for FunctionMaterialCreationView.xaml
    /// </summary>
    public partial class FunctionMaterialCreationView : Window
    {
        public FunctionMaterialCreationVM ViewModel { get; set; }
        public FunctionMaterialCreationView(FunctionMaterialCreationVM viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this.ViewModel;
            InitializeComponent();
        }
        public FunctionMaterialCreationView() : this(new FunctionMaterialCreationVM())
        {
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
