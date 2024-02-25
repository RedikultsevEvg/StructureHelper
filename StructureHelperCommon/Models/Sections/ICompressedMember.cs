using System;

namespace StructureHelperCommon.Models.Sections
{

    //Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
    //All rights reserved.

    /// <summary>
    /// Interface of properties for compressed strucrue members
    /// </summary>
    public interface ICompressedMember : ICloneable
    {
        /// <summary>
        /// Flag of considering of buckling
        /// </summary>
        bool Buckling { get; set; }
        /// <summary>
        /// Geometry length of structure member, m
        /// </summary>
        double GeometryLength { get; set; }
        /// <summary>
        /// Factor of design length in plane XOZ
        /// </summary>
        double LengthFactorX { get; set; }    
        double DiagramFactorX { get; set; }
        /// <summary>
        /// Factor of design length in plane YOZ
        /// </summary>
        double LengthFactorY { get; set; }
        double DiagramFactorY { get; set; }
    }
}
