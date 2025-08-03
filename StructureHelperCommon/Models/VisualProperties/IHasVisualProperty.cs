using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.VisualProperties
{
    public interface IHasVisualProperty
    {
        IPrimitiveVisualProperty VisualProperty { get; set; }
    }
}
