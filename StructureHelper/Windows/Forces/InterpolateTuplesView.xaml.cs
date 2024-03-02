using StructureHelper.Windows.UserControls;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
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
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            DataContext = this.viewModel;
            InitializeComponent();
            InterpolationControl.Properties = viewModel.ForceInterpolationViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
