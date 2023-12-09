using StructureHelperCommon.Infrastructures.Interfaces;
using System;


//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Interface for point of center of some shape
    /// Интерфейс для точки центра некоторой формы
    /// </summary>
    public interface IPoint2D : ISaveable, ICloneable
    {
        /// <summary>
        /// Coordinate of center of point by local axis X, m
        /// Координата центра вдоль локальной оси X, м
        /// </summary>
        double X { get; set; }
        /// <summary>
        /// Coordinate of center of point by local axis Y, m
        /// Координата центра вдоль локальной оси Y, м
        /// </summary>
        double Y { get; set; }
    }
}
