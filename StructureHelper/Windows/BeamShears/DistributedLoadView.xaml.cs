using StructureHelperCommon.Models.Forces;
using System.Windows;

namespace StructureHelper.Windows.BeamShears
{
    /// <summary>
    /// Interaction logic for UniformDistributedLoadView.xaml
    /// </summary>
    public partial class DistributedLoadView : Window
    {
        private readonly DistributedLoadViewModel viewModel;
        public DistributedLoadView(DistributedLoadViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
            this.viewModel.ParentWindow = this;
        }

        public DistributedLoadView(IDistributedLoad distributedLoad) : this(new DistributedLoadViewModel(distributedLoad))
        {
            
        }
    }
}
