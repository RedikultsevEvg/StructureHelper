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
        public DataTemplate ConcentratedForceTemplate { get; set; }
        public DataTemplate DistributedLoadTemplate { get; set; }
        public DataTemplate TrapezoidDistributedLoadTemplate { get; set; }
        public DataTemplate PolygonShapeTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                BeamShearSectionPrimitive => BeamShearSectionTemplate,
                InclinedSectionPrimitive => InclinedSectionTemplate,
                StirrupByRebarPrimitive => StirrupByRebarTemplate,
                StirrupByDensityPrimitive => StirrupByDensityTemplate,
                StirrupByInclinedRebarPrimitive => StirrupByInclinedRebarTemplate,
                ConcentratedForcePrimitive => ConcentratedForceTemplate,
                DistributedLoadPrimitive => DistributedLoadTemplate,
                TrapezoidDistributedLoadPrimitive => TrapezoidDistributedLoadTemplate,
                PolygonShapePrimitive => PolygonShapeTemplate,
                _ => base.SelectTemplate(item, container)
            };
        }
    }

}
