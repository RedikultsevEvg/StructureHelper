using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.UserControls.WorkPlanes
{
    public class PrimitiveTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BeamShearSectionTemplate { get; set; }
        public DataTemplate InclinedSectionTemplate { get; set; }
        public DataTemplate StirrupByRebarTemplate { get; set; }
        public DataTemplate StirrupByDensityTemplate { get; set; }
        public DataTemplate StirrupByInclinedRebarTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                BeamShearSectionPrimitive => BeamShearSectionTemplate,
                InclinedSectionPrimitive => InclinedSectionTemplate,
                StirrupByRebarPrimitive => StirrupByRebarTemplate,
                StirrupByDensityPrimitive => StirrupByDensityTemplate,
                StirrupByInclinedRebarPrimitive => StirrupByInclinedRebarTemplate,
                _ => base.SelectTemplate(item, container)
            };
        }
    }

}
