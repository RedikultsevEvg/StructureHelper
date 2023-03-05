using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.ViewModels.PrimitiveProperties;
using StructureHelperLogics.Models.Materials;
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
using PointViewPrimitive = StructureHelper.Infrastructure.UI.DataContexts.PointViewPrimitive;
using RectangleViewPrimitive = StructureHelper.Infrastructure.UI.DataContexts.RectangleViewPrimitive;

namespace StructureHelper.Windows.PrimitiveProperiesWindow
{
    /// <summary>
    /// Логика взаимодействия для PrimitiveProperties.xaml
    /// </summary>
    public partial class PrimitivePropertiesView : Window
    {
        PrimitiveBase primitive;
        private PrimitivePropertiesViewModel viewModel;
        public PrimitivePropertiesView(PrimitiveBase primitive, IHasHeadMaterials headMaterials)
        {
            this.primitive = primitive;
            viewModel = new PrimitivePropertiesViewModel(this.primitive, headMaterials);
            this.DataContext = viewModel;
            InitializeComponent();
            if (primitive is RectangleViewPrimitive) { AddPrimitiveProperties(PrimitiveType.Rectangle); }
            else if (primitive is CircleViewPrimitive) { AddPrimitiveProperties(PrimitiveType.Circle); }
            else if (primitive is PointViewPrimitive) { AddPrimitiveProperties(PrimitiveType.Point); }
            else { throw new Exception("Type of object is unknown"); }   
        }
        private void AddPrimitiveProperties(PrimitiveType type)
        {
            List<string> names = new List<string>();
            if (primitive.DivisionViewModel is not null) { names.Add("TriangulationProperties");}
            if (primitive is RectangleViewPrimitive) { names.Add("RectangleProperties"); }
            if (primitive is CircleViewPrimitive) { names.Add("CircleProperties"); }
            if (primitive is PointViewPrimitive) { names.Add("PointProperties"); }
            if (primitive is ReinforcementViewPrimitive) { names.Add("ReinforcementProperties"); }
            foreach (var name in names)
            {
                ContentControl contentControl = new ContentControl();
                contentControl.SetResourceReference(ContentTemplateProperty, name);
                Binding binding = new Binding {Source = viewModel};
                contentControl.SetBinding(ContentProperty, binding);
                StpProperties.Children.Add(contentControl);
            }
        }
    }
}
