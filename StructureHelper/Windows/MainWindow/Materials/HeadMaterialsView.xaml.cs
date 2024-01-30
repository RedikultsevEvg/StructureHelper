using StructureHelper.Models.Materials;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperLogics.Models.Materials;
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
using System.Xml.Linq;

namespace StructureHelper.Windows.MainWindow.Materials
{
    /// <summary>
    /// Логика взаимодействия для HeadMaterials.xaml
    /// </summary>
    public partial class HeadMaterialsView : Window
    {
        private HeadMaterialsViewModel viewModel;

        //public HeadMaterialsView(List<IHeadMaterial> headMaterials) : this(new HeadMaterialsViewModel(headMaterials)) {}
        public HeadMaterialsView(IHasHeadMaterials hasHeadMaterials) : this(new HeadMaterialsViewModel(hasHeadMaterials)) { }
        public HeadMaterialsView(HeadMaterialsViewModel vm)
        {
            viewModel = vm;
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
