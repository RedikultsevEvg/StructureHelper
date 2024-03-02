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
    /// Логика взаимодействия для ValuePoitsInterpolateView.xaml
    /// </summary>
    public partial class ValuePointsInterpolateView : Window
    {
        private ValuePointsInterpolateViewModel viewModel;
        public ValuePointsInterpolateView(ValuePointsInterpolateViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            this.DataContext = this.viewModel;
        }
    }
}
