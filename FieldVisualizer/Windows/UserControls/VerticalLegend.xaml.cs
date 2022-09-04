using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FieldVisualizer.Windows.UserControls
{
    /// <summary>
    /// Логика взаимодействия для VerticalLegend.xaml
    /// </summary>
    public partial class VerticalLegend : UserControl
    {
        public IEnumerable<IValueColorRange> ValueColorRanges;
        public VerticalLegend()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            this.DataContext = ValueColorRanges;
        }
    }
}
