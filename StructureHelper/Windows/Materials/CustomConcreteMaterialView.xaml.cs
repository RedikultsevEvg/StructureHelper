using StructureHelper.Models.Materials;
using StructureHelper.Windows.ViewModels.Materials;
using System.ComponentModel;
using System.Windows;

namespace StructureHelper.Windows.Materials
{
    /// <summary>
    /// Material-level editor for custom concrete materials.
    /// </summary>
    public partial class CustomConcreteMaterialView : Window
    {
        private readonly CustomConcreteMaterialViewModel viewModel;

        public CustomConcreteMaterialView()
        {
            InitializeComponent();
        }

        public CustomConcreteMaterialView(IHeadMaterial headMaterial)
            : this(new CustomConcreteMaterialViewModel(headMaterial))
        {
        }

        public CustomConcreteMaterialView(CustomConcreteMaterialViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            viewModel.ParentWindow = this;
            DataContext = viewModel;
        }
    }
}
