namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Interface for point of center of some shape
    /// Интерфейс для точки центра некоторой формы
    /// </summary>
    public interface IPoint2D
    {
        /// <summary>
        /// Coordinate of center of rectangle by local axis X, m
        /// Координата центра вдоль локальной оси X, м
        /// </summary>
        double X { get; set; }
        /// <summary>
        /// Coordinate of center of rectangle by local axis Y, m
        /// Координата центра вдоль локальной оси Y, м
        /// </summary>
        double Y { get; set; }
    }
}
