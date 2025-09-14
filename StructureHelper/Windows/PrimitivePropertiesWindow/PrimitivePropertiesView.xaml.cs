using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels.PrimitiveProperties;
using StructureHelperLogics.Models.CrossSections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StructureHelper.Windows.PrimitivePropertiesWindow
{
    /// <summary>
    /// Логика взаимодействия для PrimitiveProperties.xaml
    /// </summary>
    public partial class PrimitivePropertiesView : Window
    {
        PrimitiveBase primitive;
        private PrimitivePropertiesViewModel viewModel;
        public PrimitivePropertiesView(PrimitiveBase primitive, ICrossSectionRepository sectionRepository)
        {
            this.primitive = primitive;
            viewModel = new PrimitivePropertiesViewModel(this.primitive, sectionRepository);
            viewModel.ParentWindow = this;
            this.DataContext = viewModel;
            InitializeComponent();
            AddPrimitiveProperties();
        }
        private void AddPrimitiveProperties()
        {
            List<string> templateNames = new List<string>();
            if (primitive.DivisionViewModel is not null) { templateNames.Add("TriangulationProperties");}
            if (primitive is RectangleViewPrimitive) { templateNames.Add("RectangleProperties"); }
            if (primitive is CircleViewPrimitive) { templateNames.Add("CircleProperties"); }
            if (primitive is ShapeViewPrimitive) { templateNames.Add("PolygonProperties"); }
            if (primitive is PointViewPrimitive) { templateNames.Add("PointProperties"); }
            if (primitive is ReinforcementViewPrimitive) { templateNames.Add("ReinforcementProperties"); }
            foreach (var name in templateNames)
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
