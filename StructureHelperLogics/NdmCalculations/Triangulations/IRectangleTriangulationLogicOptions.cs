using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <summary>
    /// Parameter of triangulation of rectangle part of section
    /// Параметры триангуляции прямоугольного участка сечения
    /// </summary>
    public interface IRectangleTriangulationLogicOptions : ITriangulationLogicOptions
    {
        /// <summary>
        /// 
        /// </summary>
        ICenter Center { get; }
        /// <summary>
        /// 
        /// </summary>
        IRectangle Rectangle { get; }
        /// <summary>
        /// Maximum size (width or height) of ndm part after triangulation
        /// </summary>
        double NdmMaxSize { get; }
        /// <summary>
        /// Minimum quantity of division of side of rectangle after triangulation
        /// </summary>
        int NdmMinDivision { get; }
    }
}
