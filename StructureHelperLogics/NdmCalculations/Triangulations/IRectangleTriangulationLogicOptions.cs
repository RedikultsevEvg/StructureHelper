using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <summary>
    /// Parameter of triangulation of rectangle part of section
    /// Параметры триангуляции прямоугольного участка сечения
    /// </summary>
    public interface IRectangleTriangulationLogicOptions : IShapeTriangulationLogicOptions
    {
        /// <summary>
        /// 
        /// </summary>
        IRectangleShape Rectangle { get; }
    }
}
