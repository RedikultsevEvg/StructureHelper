using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.MaterialBuilders
{
    internal interface IPartialFactor
    {
        double YoungsModulus { get; set; }
        double Compressive { get; set; }
        double Tensile { get; set; }
    }
}
