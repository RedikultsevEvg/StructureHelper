using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Windows.UserControls;
using System;
using System.Collections.Generic;
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
        public WndFieldViewer(IEnumerable<IPrimitiveSet> primitiveSets)
        {
            InitializeComponent();
            this.DataContext = primitiveSets;
            foreach (var primitiveSet in primitiveSets)
            {
                FieldViewerControl._PrimitiveSet = primitiveSet;
            }

        }

        private void SetsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FieldViewerControl.Refresh();
        }
    }
}
