using StructureHelperCommon.Models.Sections;

namespace StructureHelperCommon.Services.Sections
{
    public static class CompressedMemberServices
    {
        public static void CopyProperties(ICompressedMember source, ICompressedMember target)
        {
            target.Buckling = source.Buckling;
            target.GeometryLength = source.GeometryLength;
            target.LengthFactorX = source.LengthFactorX;
            target.DiagramFactorX = source.DiagramFactorX;
            target.LengthFactorY = source.LengthFactorY;
            target.DiagramFactorY = source.DiagramFactorY;
        }
    }
}
