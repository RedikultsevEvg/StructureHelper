using System.Windows;

namespace StructureHelper.Windows.Forces
{
    /// <summary>
    /// Логика взаимодействия для ValuePoitsInterpolateView.xaml
    /// </summary>
    public partial class ValuePointsInterpolateView : Window
    {
        private ValuePointsInterpolateViewModel viewModel;
        public ValuePointsInterpolateView(ValuePointsInterpolateViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            this.DataContext = this.viewModel;
            InitializeComponent();
            InterpolationControl.Properties = viewModel.ForceInterpolationViewModel;
        }
    }
}
