using StructureHelperLogics.Models.BeamShears;
using System.Windows;

namespace StructureHelper.Windows.BeamShears
{
    /// <summary>
    /// Interaction logic for BeamShearRankInclinedSectionView.xaml
    /// </summary>
    public partial class BeamShearRankInclinedSectionView : Window
    {
        private BeamShearRankInclinedSectionViewModel viewModel;
        public BeamShearRankInclinedSectionView(BeamShearRankInclinedSectionViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
        }

        public BeamShearRankInclinedSectionView(IBeamShearActionResult actionResult) : this(new BeamShearRankInclinedSectionViewModel(actionResult))
        {
            
        }
    }
}
