using StructureHelperCommon.Models.Forces;
using System.Windows;

namespace StructureHelper.Windows.BeamShears
{
    /// <summary>
    /// Interaction logic for TrapezoidLoadView.xaml
    /// </summary>
    public partial class TrapezoidLoadView : Window
    {
        private TrapezoidLoadViewModel viewModel;
        public TrapezoidLoadView(ITrapezoidDistributedLoad trapezoidLoad) : this (new TrapezoidLoadViewModel(trapezoidLoad))
        {
            
        }
        public TrapezoidLoadView(TrapezoidLoadViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            viewModel.ParentWindow = this;
        }
    }
}
