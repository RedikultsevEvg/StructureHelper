using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials
{
    public interface ICrackedMaterial
    {
        bool TensionForULS { get; set; }
        bool TensionForSLS { get; set; }
    }
}
