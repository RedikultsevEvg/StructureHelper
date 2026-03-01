using StructureHelperCommon.Models.FeaMaterials;
using System.Windows;

namespace StructureHelper.Windows.FeaMaterials
{
    /// <summary>
    /// Interaction logic for ConcreteFeaMaterialView.xaml
    /// </summary>
    public partial class ConcreteFeaMaterialView : Window
    {
        private ConcreteFeaMaterialViewModel viewModel;

        public ConcreteFeaMaterialView(IConcreteFeaMaterial concrete) : this (new  ConcreteFeaMaterialViewModel(concrete))
        {
            
        }
        public ConcreteFeaMaterialView(ConcreteFeaMaterialViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            viewModel.ParentWindow = this;
        }
    }
}
