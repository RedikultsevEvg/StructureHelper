using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Interface for geometry point in 3D space
    /// </summary>
    public interface IPoint3D : ISaveable, ICloneable
    {
        /// <summary>
        /// Coordinate along X-axis
        /// </summary>
        double X { get; set; }
        /// <summary>
        /// Coordinate along Y-axis
        /// </summary>
        double Y { get; set; }
        /// <summary>
        /// Coordinate along Z-axis
        /// </summary>
        double Z { get; set; }
    }
}
