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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StructureHelper.Windows.Forces
{
    /// <summary>
    /// Логика взаимодействия для InterpolateTuplesView.xaml
    /// </summary>
    public partial class InterpolateTuplesView : Window
    {
        InterpolateTuplesViewModel viewModel;
        public InterpolateTuplesView(InterpolateTuplesViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void StartValueChanged(object sender, EventArgs e)
        {
            viewModel.RefreshStartTuple();
        }

        private void FinishValueChanged(object sender, EventArgs e)
        {
            viewModel.RefreshFinishTuple();
        }
    }
}
