using LiveCharts.Wpf;
using StructureHelperCommon.Models.Parameters;
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

namespace StructureHelper.Windows.Graphs
{
    /// <summary>
    /// Логика взаимодействия для GraphView.xaml
    /// </summary>
    public partial class GraphView : Window
    {
        GraphViewModel vm;
        public GraphView(GraphViewModel vm)
        {
            this.vm = vm;
            InitializeComponent();
            DataContext = vm;

            var cartesianChart = (CartesianChart)FindName("MainChart");
            vm.MainChart = cartesianChart;
        }
        public GraphView(ArrayParameter<double> arrayParameter) : this(new GraphViewModel(arrayParameter))
        {
        }
        public GraphView(IEnumerable<ArrayParameter<double>> arrayParameters) : this(new GraphViewModel(arrayParameters))
        {
        }
    }
}
