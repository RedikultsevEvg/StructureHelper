using StructureHelper.Windows.Forces;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for FactoredCombination.xaml
    /// </summary>
    public partial class FactoredCombination : UserControl
    {
        FactoredCombinationPropertyVM viewModel;

        // Registering the Dependency Property
        public static readonly DependencyProperty CombinationPropertyProperty =
            DependencyProperty.Register(
                nameof(CombinationProperty),         // Property name
                typeof(IFactoredCombinationProperty), // Property type
                typeof(FactoredCombination),                 // Owner class
                new PropertyMetadata(
                default(IFactoredCombinationProperty),
                OnCombinationPropertyChanged // PropertyChangedCallback
                )
            );

        // CLR Wrapper
        public IFactoredCombinationProperty CombinationProperty
        {
            get => (IFactoredCombinationProperty)GetValue(CombinationPropertyProperty);
            set => SetValue(CombinationPropertyProperty, value);
        }
        public FactoredCombination()
        {
            if (CombinationProperty is not null)
            {
                viewModel = new(CombinationProperty);
            }
            else
            { 
                viewModel = new(new FactoredCombinationProperty());
            }
            DataContext = viewModel;
            InitializeComponent();
        }

        private static void OnCombinationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FactoredCombination;
            var newValue = e.NewValue as IFactoredCombinationProperty;

            if (control?.viewModel != null)
            {
                // Update the ViewModel with the new property value
                control.viewModel.UpdateCombinationProperty(newValue);
            }
        }
    }
}
