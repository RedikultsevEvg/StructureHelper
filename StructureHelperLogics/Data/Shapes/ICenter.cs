using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Data.Shapes
{
    /// <summary>
    /// Interface for point of center of some shape
    /// Интерфейс для точки центра некоторой формы
    /// </summary>
    public interface ICenter
    {
        /// <summary>
        /// Coordinate of center of rectangle by local axis X, m
        /// Координата центра вдоль локальной оси X, м
        /// </summary>
        double CenterX { get;}
        /// <summary>
        /// Coordinate of center of rectangle by local axis Y, m
        /// Координата центра вдоль локальной оси Y, м
        /// </summary>
        double CenterY { get;}
    }
}
