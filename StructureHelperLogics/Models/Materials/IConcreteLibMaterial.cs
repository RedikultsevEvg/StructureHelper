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
        /// <summary>
        /// Humidity of concrete
        /// </summary>
        double RelativeHumidity { get; set; }
        double MinAge { get; set; }
        double MaxAge { get; set; }
    }
}
