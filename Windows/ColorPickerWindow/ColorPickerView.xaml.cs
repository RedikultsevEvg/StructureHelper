using System.Windows;

namespace StructureHelper
{
    /// <summary>
    /// Логика взаимодействия для ColorPickerView.xaml
    /// </summary>
    public partial class ColorPickerView : Window
    {
        public ColorPickerView(PrimitiveDefinitionBase primitive)
        {
            var viewModel = new ColorPickerViewModel(primitive);
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
