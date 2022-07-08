using System.Windows;
using StructureHelper.Infrastructure.UI.DataContexts;

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
