using StructureHelper.Windows.MainWindow;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Graphs;
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

namespace StructureHelper.Windows.MainGraph
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : Window
    {
        private GraphViewModel viewModel;
        public GraphView(GraphViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            InitializeComponent();
        }
        public GraphView() : this(new GraphViewModel())
        {
        }
    }
}
