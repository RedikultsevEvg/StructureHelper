using System;

namespace StructureHelperCommon.Models.Sections
{

    //Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
    //All rights reserved.

    /// <inheritdoc/>
    public class CompressedMember : ICompressedMember
    {
        static readonly CompressedMemberUpdateStrategy updateStrategy = new();
        /// <inheritdoc/>
        public Guid Id { get;}
        /// <inheritdoc/>
        public bool Buckling { get; set; } = true;
        /// <inheritdoc/>
        public double GeometryLength { get; set; } = 3d;
        /// <inheritdoc/>
        public double LengthFactorX { get; set; } = 1d;
        /// <inheritdoc/>
        public double DiagramFactorX { get; set; } = 1d;
        /// <inheritdoc/>
        public double LengthFactorY { get; set; } = 1d;
        /// <inheritdoc/>
        public double DiagramFactorY { get; set; } = 1d;

        public CompressedMember(Guid id)
        {
            Id = id;
        }

        public CompressedMember() : this(Guid.NewGuid())
        {
        }


        public object Clone()
        {
            var newItem = new CompressedMember();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
