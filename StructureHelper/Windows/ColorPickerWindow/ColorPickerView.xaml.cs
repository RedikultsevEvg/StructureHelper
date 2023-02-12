using StructureHelper.Infrastructure.UI.DataContexts;
using System.Windows;

namespace StructureHelper.Windows.ColorPickerWindow
{
    /// <summary>
    /// Логика взаимодействия для ColorPickerView.xaml
    /// </summary>
    public partial class ColorPickerView : Window
    {
        public ColorPickerView(PrimitiveBase primitive)
        {
            var viewModel = new ColorPickerViewModel(primitive);
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
