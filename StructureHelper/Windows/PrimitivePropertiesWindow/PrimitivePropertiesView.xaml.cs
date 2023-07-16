using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels.PrimitiveProperties;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        public PrimitivePropertiesView(PrimitiveBase primitive, ICrossSectionRepository sectionRepository)
        {
            this.primitive = primitive;
            viewModel = new PrimitivePropertiesViewModel(this.primitive, sectionRepository);
            viewModel.ParentWindow = this;
            this.DataContext = viewModel;
            InitializeComponent();
            if (primitive is RectangleViewPrimitive) { AddPrimitiveProperties(PrimitiveType.Rectangle); }
            else if (primitive is CircleViewPrimitive) { AddPrimitiveProperties(PrimitiveType.Circle); }
            else if (primitive is PointViewPrimitive) { AddPrimitiveProperties(PrimitiveType.Point); }
            else { throw new Exception("Type of object is unknown"); }   
        }
        private void AddPrimitiveProperties(PrimitiveType type)
        {
            List<string> templateNames = new List<string>();
            if (primitive.DivisionViewModel is not null) { templateNames.Add("TriangulationProperties");}
            if (primitive is RectangleViewPrimitive) { templateNames.Add("RectangleProperties"); }
            if (primitive is CircleViewPrimitive) { templateNames.Add("CircleProperties"); }
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
