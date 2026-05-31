using System.Windows;

namespace StructureHelper.Windows.MainWindow.Materials
{
    /// <summary>
    /// Interaction logic for UserMaterialView.xaml
    /// </summary>
    public partial class UserMaterialView : Window
    {
        UserMaterialViewModel viewModel;
        public UserMaterialView(UserMaterialViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            this.DataContext = this.viewModel;
        }
    }
}
