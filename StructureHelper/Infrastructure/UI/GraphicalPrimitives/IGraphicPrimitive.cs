using FieldVisualizer.Entities.Values.Primitives;
using StructureHelper.Windows.UserControls;
using StructureHelperCommon.Models.VisualProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public interface IGraphicalPrimitive : ICenter
    {
        PrimitiveVisualPropertyViewModel VisualProperty { get; }
    }
}
