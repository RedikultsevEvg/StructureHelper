using FieldVisualizer.Entities.Values.Primitives;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FieldVisualizer.Windows
{
    /// <summary>
    /// Логика взаимодействия для WndFieldViewer.xaml
    /// </summary>
    public partial class WndFieldViewer : Window
    {
        public ObservableCollection<IPrimitiveSet> PrimitiveSets { get; private set; }
        public WndFieldViewer(IEnumerable<IPrimitiveSet> primitiveSets)
        {
            InitializeComponent();
            PrimitiveSets = new ObservableCollection<IPrimitiveSet>();
            foreach (var primitiveSet in primitiveSets)
            {
                PrimitiveSets.Add(primitiveSet);
            }
            this.DataContext = PrimitiveSets;

        }

        private void SetsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb.SelectedItem != null)
            {
                FieldViewerControl.PrimitiveSet = lb.SelectedItem as IPrimitiveSet;
                FieldViewerControl.Refresh();
            }
        }
    }
}
