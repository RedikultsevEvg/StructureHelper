using FieldVisualizer.Entities.Values.Primitives;
using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.UserControls.WorkPlanes
{
    public class PrimitiveTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BeamShearSectionTemplate { get; set; }
        public DataTemplate InclinedSectionTemplate { get; set; }
        public DataTemplate BeamShearStirrupByRebarTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                BeamShearSectionPrimitive => BeamShearSectionTemplate,
                InclinedSectionPrimitive => InclinedSectionTemplate,
                _ => base.SelectTemplate(item, container)
            };
        }
    }

}
