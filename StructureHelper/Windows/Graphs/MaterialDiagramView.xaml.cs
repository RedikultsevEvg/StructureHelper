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
using LiveCharts;
using LiveCharts.Wpf;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.Graphs;
using StructureHelperLogics.Models.Materials;

namespace StructureHelper.Windows.MainWindow.Materials
{
    /// <summary>
    /// Логика взаимодействия для MaterialDiagramView.xaml
    /// </summary>
    public partial class MaterialDiagramView : Window
    {
        MaterialDiagramViewModel vm;
        public MaterialDiagramView(MaterialDiagramViewModel vm)
        {
            this.vm = vm;
            InitializeComponent();
            this.DataContext = this.vm;
        }
        public MaterialDiagramView(IEnumerable<IHeadMaterial> headMaterials, IHeadMaterial material) : this(new MaterialDiagramViewModel(headMaterials, material))
        {
            
        }
    }
}
