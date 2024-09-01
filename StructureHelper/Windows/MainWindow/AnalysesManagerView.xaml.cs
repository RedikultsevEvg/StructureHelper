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

namespace StructureHelper.Windows.MainWindow
{
    /// <summary>
    /// Логика взаимодействия для AnalisesManagerView.xaml
    /// </summary>
    public partial class AnalysesManagerView : Window
    {
        private AnalysesManagerViewModel viewModel;
        public AnalysesManagerView()
        {
            this.viewModel = new();
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
