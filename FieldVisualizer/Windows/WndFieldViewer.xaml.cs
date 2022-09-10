using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Windows.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
