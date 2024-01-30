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
    /// Logic for convert 2DPoint of some plane to point of 3DSpace
    /// </summary>
    public interface IConvert2DPointTo3DPointLogic
    {
        /// <summary>
        /// Returns point in 3D-space by 2D point in some workplane
        /// </summary>
        /// <param name="point2D"></param>
        /// <returns></returns>
        IPoint3D GetPoint3D(IPoint2D point2D);
    }
}
