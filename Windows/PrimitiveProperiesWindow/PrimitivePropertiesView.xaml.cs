using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels.PrimitiveProperties;
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
using System.Xml.Linq;
using Point = StructureHelper.Infrastructure.UI.DataContexts.Point;
using Rectangle = StructureHelper.Infrastructure.UI.DataContexts.Rectangle;

namespace StructureHelper.Windows.PrimitiveProperiesWindow
{
    /// <summary>
    /// Логика взаимодействия для PrimitiveProperties.xaml
    /// </summary>
    public partial class PrimitiveProperties : Window
    {
        PrimitiveBase primitive;
        private PrimitivePropertiesViewModel viewModel;
        public PrimitiveProperties(PrimitiveBase primitive)
        {
            this.primitive = primitive;
            viewModel = new PrimitivePropertiesViewModel(this.primitive);
            this.DataContext = viewModel;
            InitializeComponent();
            if (primitive is Rectangle) { AddPrimitiveProperties(PrimitiveType.Rectangle); }
            else if (primitive is Point) { AddPrimitiveProperties(PrimitiveType.Point); }
            else { throw new Exception("Type of object is unknown"); }   
        }
        private void AddPrimitiveProperties(PrimitiveType type)
        {
            List<string> names = new List<string>();
            if (type == PrimitiveType.Rectangle)
            {
                names.Add("TriangulationProperties");
                names.Add("RectangleProperties");
            }
            else if (type == PrimitiveType.Point)
            {
                names.Add("PointProperties");
            }
            else { throw new Exception("Type of object is unknown"); }
            foreach (var name in names)
            {
                ContentControl contentControl = new ContentControl();
                contentControl.SetResourceReference(ContentControl.ContentTemplateProperty, name);
                Binding binding = new Binding();
                binding.Source = viewModel;
                contentControl.SetBinding(ContentControl.ContentProperty, binding);
                StpProperties.Children.Add(contentControl);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.EditColor();
        }
    }
}
