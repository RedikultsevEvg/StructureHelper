using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.UserControls
{
    /// <summary>
    /// Логика взаимодействия для VisualProperty.xaml
    /// </summary>
    public partial class PrimitiveVisualProperty : UserControl
    {

        public PrimitiveVisualPropertyViewModel ObjectVisual
        {
            get { return (PrimitiveVisualPropertyViewModel)GetValue(ObjectVisualProperty); }
            set { SetValue(ObjectVisualProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ObjectVisualProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ObjectVisualProperty =
            DependencyProperty.Register(
                nameof(ObjectVisual),
                typeof(PrimitiveVisualPropertyViewModel),
                typeof(PrimitiveVisualProperty),
                new PropertyMetadata(null)
                );

        public PrimitiveVisualProperty()
        {
            InitializeComponent();
        }
    }
}
