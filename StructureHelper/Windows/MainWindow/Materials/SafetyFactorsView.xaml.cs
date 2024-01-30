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

namespace StructureHelper.Windows.AddMaterialWindow
{
    /// <summary>
    /// Логика взаимодействия для SafetyFactorsView.xaml
    /// </summary>
    public partial class SafetyFactorsView : Window
    {
        public SafetyFactorsView(List<IMaterialSafetyFactor> safetyFactors)
        {
            InitializeComponent();
            var vm = new SafetyFactorsViewModel(safetyFactors);
            this.DataContext = vm;
        }
    }
}
