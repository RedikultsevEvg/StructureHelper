using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Calculators;
using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.UserControls
{
    /// <summary>
    /// Reusable editor for the properties of an <see cref="IAccuracy"/> entity.
    /// The OK and Cancel buttons use <see cref="OkCancelViewModelBase"/> and close the hosting window.
    /// </summary>
    public partial class AccuracyView : UserControl
    {
        public static readonly DependencyProperty AccuracyProperty = DependencyProperty.Register(
            nameof(Accuracy),
            typeof(IAccuracy),
            typeof(AccuracyView),
            new PropertyMetadata(null, OnAccuracyChanged));

        public static readonly DependencyProperty ShowButtonsProperty = DependencyProperty.Register(
            nameof(ShowButtons),
            typeof(bool),
            typeof(AccuracyView),
            new PropertyMetadata(true));

        public AccuracyView()
        {
            InitializeComponent();
            Loaded += (_, _) => AttachParentWindow();
            DataContextChanged += (_, _) => AttachParentWindow();
        }

        /// <summary>
        /// Entity to edit. When set, the control creates its own <see cref="AccuracyViewModel"/>.
        /// </summary>
        public IAccuracy? Accuracy
        {
            get => (IAccuracy?)GetValue(AccuracyProperty);
            set => SetValue(AccuracyProperty, value);
        }

        /// <summary>
        /// True by default. Set to false when the control is embedded in a dialog which already
        /// owns the OK/Cancel buttons.
        /// </summary>
        public bool ShowButtons
        {
            get => (bool)GetValue(ShowButtonsProperty);
            set => SetValue(ShowButtonsProperty, value);
        }

        private static void OnAccuracyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var view = (AccuracyView)dependencyObject;
            view.DataContext = e.NewValue is IAccuracy accuracy
                ? new AccuracyViewModel(accuracy)
                : null;
        }

        private void AttachParentWindow()
        {
            if (DataContext is OkCancelViewModelBase viewModel && Window.GetWindow(this) is Window window)
            {
                viewModel.ParentWindow = window;
            }
        }
    }
}
