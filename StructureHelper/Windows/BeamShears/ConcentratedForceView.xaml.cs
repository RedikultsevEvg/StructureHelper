using StructureHelperCommon.Models.Forces;
using System.Windows;

namespace StructureHelper.Windows.BeamShears
{
    /// <summary>
    /// Interaction logic for ConcentratedForceView.xaml
    /// </summary>
    public partial class ConcentratedForceView : Window
    {
        private readonly ConcentratedForceViewModel viewModel;
        public ConcentratedForceView(ConcentratedForceViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            this.viewModel.ParentWindow = this;
        }
        public ConcentratedForceView(IConcentratedForce concenratedForce) : this(new ConcentratedForceViewModel(concenratedForce))
        {
            
        }
    }
}
