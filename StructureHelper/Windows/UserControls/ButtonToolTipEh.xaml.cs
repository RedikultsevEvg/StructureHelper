using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for ButtonToolTipEh.xaml
    /// </summary>
    public partial class ButtonToolTipEh : UserControl
    {
        // Dependency property for HeaderText
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(
                nameof(HeaderText),
                typeof(string),
                typeof(ButtonToolTipEh),
                new PropertyMetadata("Default Header"));

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        // Dependency property for DescriptionText
        public static readonly DependencyProperty DescriptionTextProperty =
            DependencyProperty.Register(
                nameof(DescriptionText),
                typeof(string),
                typeof(ButtonToolTipEh),
                new PropertyMetadata("Default description text"));

        public string DescriptionText
        {
            get => (string)GetValue(DescriptionTextProperty);
            set => SetValue(DescriptionTextProperty, value);
        }

        // Dependency property for IconContent
        public static readonly DependencyProperty IconContentProperty =
            DependencyProperty.Register(
                nameof(IconContent),
                typeof(object),
                typeof(ButtonToolTipEh),
                new PropertyMetadata(null));

        public object IconContent
        {
            get => GetValue(IconContentProperty);
            set => SetValue(IconContentProperty, value);
        }

        // Dependency property for IconTemplate
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register(
                nameof(IconTemplate),
                typeof(DataTemplate),
                typeof(ButtonToolTipEh),
                new PropertyMetadata(null));

        public DataTemplate IconTemplate
        {
            get => (DataTemplate)GetValue(IconTemplateProperty);
            set => SetValue(IconTemplateProperty, value);
        }
        public ButtonToolTipEh()
        {
            InitializeComponent();
        }
    }
}
