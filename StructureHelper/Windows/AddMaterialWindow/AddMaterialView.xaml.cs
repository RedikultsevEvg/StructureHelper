using System.Windows;
using StructureHelper.MaterialCatalogWindow;

namespace StructureHelper.Windows.AddMaterialWindow
{
    /// <summary>
    /// Логика взаимодействия для AddMaterialView.xaml
    /// </summary>
    public partial class AddMaterialView : Window
    {
        public AddMaterialView(MaterialCatalogModel model, MaterialCatalogViewModel materialCatalogViewModel)
        {
            var viewModel = new AddMaterialViewModel(model, this, materialCatalogViewModel);
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
