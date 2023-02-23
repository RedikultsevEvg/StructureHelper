using System;

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
