using System.Windows;

namespace StructureHelper
{
    /// <summary>
    /// Логика взаимодействия для MaterialCatalogView.xaml
    /// </summary>
    public partial class MaterialCatalogView : Window
    {
        public MaterialCatalogView(bool isMaterialCanBeSelected = false, PrimitiveDefinition primitive = null)
        {
            var materialCatalogModel = new MaterialCatalogModel();
            var materialCatalogViewModel = new MaterialCatalogViewModel(materialCatalogModel, this, isMaterialCanBeSelected, primitive);
            DataContext = materialCatalogViewModel;
            InitializeComponent();
        }
    }
}
