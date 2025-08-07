using FieldVisualizer.Entities.Values.Primitives;
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
        IPrimitiveVisualProperty VisualProperty { get; }
    }
}
