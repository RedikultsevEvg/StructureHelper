namespace StructureHelperCommon.Models.Sections
{
    public class CompressedMember : ICompressedMember
    {
        static readonly CompressedMemberUpdateStrategy updateStrategy = new();
        public bool Buckling { get; set; }
        public double GeometryLength { get; set; }
        public double LengthFactorX { get; set; }
        public double DiagramFactorX { get; set; }
        public double LengthFactorY { get; set; }
        public double DiagramFactorY { get; set; }
        

        public CompressedMember()
        {
            Buckling = true;
            GeometryLength = 3d;
            LengthFactorX = 1d;
            DiagramFactorX = 1d;
            LengthFactorY = 1d;
            DiagramFactorY = 1d;
        }

        public object Clone()
        {
            var newItem = new CompressedMember();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
