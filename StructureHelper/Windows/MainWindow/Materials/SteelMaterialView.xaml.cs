using StructureHelper.Models.Materials;
using System.Windows;

namespace StructureHelper.Windows.MainWindow.Materials
{
    /// <summary>
    /// Interaction logic for SteelMaterialView.xaml
    /// </summary>
    public partial class SteelMaterialView : Window
    {
        private SteelMaterialViewModel viewModel;

        public SteelMaterialView(IHeadMaterial steelLibMaterial) : this (new SteelMaterialViewModel(steelLibMaterial))
        {
        }

        public SteelMaterialView(SteelMaterialViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
            this.viewModel.ParentWindow = this;
        }
    }
}
