using System.Windows;
using StructureHelper.Infrastructure.UI.DataContexts;

namespace StructureHelper.MaterialCatalogWindow
{
    /// <summary>
    /// Логика взаимодействия для MaterialCatalogView.xaml
    /// </summary>
    public partial class MaterialCatalogView : Window
    {
        public MaterialCatalogView(bool isMaterialCanBeSelected = false, PrimitiveBase primitive = null)
        {
            var materialCatalogModel = new MaterialCatalogModel();
            var materialCatalogViewModel = new MaterialCatalogViewModel(materialCatalogModel, this, isMaterialCanBeSelected, primitive);
            DataContext = materialCatalogViewModel;
            InitializeComponent();
        }
    }
}
