using StructureHelper.Models.Materials;
using StructureHelper.Windows.ViewModels.Materials;
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
    /// Логика взаимодействия для HeadMaterials.xaml
    /// </summary>
    public partial class HeadMaterialsView : Window
    {
        private HeadMaterialsViewModel viewmodel;

        public HeadMaterialsView(IEnumerable<IHeadMaterial> materials)
        {
            viewmodel = new HeadMaterialsViewModel(materials);
            this.DataContext = viewmodel;
            InitializeComponent();
        }

        public HeadMaterialsView(HeadMaterialsViewModel vm)
        {
            viewmodel = vm;
            this.DataContext = viewmodel;
            InitializeComponent();
        }
    }
}
