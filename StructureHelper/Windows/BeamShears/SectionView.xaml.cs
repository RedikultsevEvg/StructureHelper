using StructureHelperLogics.Models.BeamShears;
using System.Windows;

namespace StructureHelper.Windows.BeamShears
{
    /// <summary>
    /// Interaction logic for SectionView.xaml
    /// </summary>
    public partial class SectionView : Window
    {
        private readonly SectionViewModel viewModel;
        public SectionView(SectionViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            this.DataContext = this.viewModel;
        }
        public SectionView(IBeamShearSection section) : this(new SectionViewModel(section))
        {
            
        }
    }
}
