using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface IShapeTriangulationLogicOptions : ITriangulationLogicOptions, IHasCenter2D
    {
        /// <summary>
        /// Parameters of division
        /// </summary>
        IDivisionSize DivisionSize { get; }
        IShape Shape { get; set; }
    }
}
