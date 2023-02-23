using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
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

namespace StructureHelper.Windows.MainWindow.Materials
{
    /// <summary>
    /// Логика взаимодействия для PartialFactorsView.xaml
    /// </summary>
    public partial class PartialFactorsView : Window
    {
        public PartialFactorsView(List<IMaterialPartialFactor> factors)
        {
            InitializeComponent();
            var vm = new PartialFactorsViewModel(factors);
            this.DataContext = vm;
        }
    }
}
