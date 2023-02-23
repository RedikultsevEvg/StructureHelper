using StructureHelper.Windows.ViewModels.Calculations.Calculators;
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

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    /// <summary>
    /// Логика взаимодействия для Prestrain.xaml
    /// </summary>
    public partial class SetPrestrainView : Window
    {
        SetPrestrainViewModel viewModel;
        public IStrainTuple StrainTuple { get; set; }

        public SetPrestrainView(SetPrestrainViewModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StrainTuple = viewModel.GetStrainTuple();
            DialogResult = true;
            Close();
        }
    }
}
