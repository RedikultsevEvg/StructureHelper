using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Sections
{
    public interface ICompressedMember : ICloneable
    {
        bool Buckling { get; set; }
        double GeometryLength { get; set; }
        double LengthFactorX { get; set; }
        double DiagramFactorX { get; set; }
        double LengthFactorY { get; set; }
        double DiagramFactorY { get; set; }
    }
}
