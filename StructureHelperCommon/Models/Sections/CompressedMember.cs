namespace StructureHelperCommon.Models.Sections
{

    //Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
    //All rights reserved.

    /// <inheritdoc/>
    public class CompressedMember : ICompressedMember
    {
        static readonly CompressedMemberUpdateStrategy updateStrategy = new();
        /// <inheritdoc/>
        public bool Buckling { get; set; }
        /// <inheritdoc/>
        public double GeometryLength { get; set; }
        /// <inheritdoc/>
        public double LengthFactorX { get; set; }
        /// <inheritdoc/>
        public double DiagramFactorX { get; set; }
        /// <inheritdoc/>
        public double LengthFactorY { get; set; }
        /// <inheritdoc/>
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
