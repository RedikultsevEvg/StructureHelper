using StructureHelperCommon.Models.FeaMaterials;
using System.Windows;

namespace StructureHelper.Windows.FeaMaterials
{
    /// <summary>
    /// Interaction logic for ElasticFeaMaterialView.xaml
    /// </summary>
    public partial class ElasticFeaMaterialView : Window
    {
        ElasticFeaMaterialViewModel viewModel;
        public ElasticFeaMaterialView(IElasticFeaMaterial material) : this(new ElasticFeaMaterialViewModel(material))
        {
            
        }

        public ElasticFeaMaterialView(ElasticFeaMaterialViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            viewModel.ParentWindow = this;
        }
    }
}
