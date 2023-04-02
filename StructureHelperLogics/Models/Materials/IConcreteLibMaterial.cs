using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public interface IConcreteLibMaterial : ILibMaterial
    {
        bool TensionForULS { get; set; }
        bool TensionForSLS { get; set; }
        double Humidity { get; set; }
    }
}
