using TriangleNet.Meshing;

namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Implements logic for converting shape to IMesh of traingle library
    /// </summary>
    public interface IMeshShapeLogic
    {
        /// <summary>
        /// Shape with center and angle of rotation
        /// </summary>
        ICenterShape CenterShape { get; set; }
        /// <summary>
        /// Minimum angle between sides of parts of triangle of mesh
        /// </summary>
        double MinimumAngleInDegree { get; set; }
        /// <summary>
        /// Maximum size of mesh
        /// </summary>
        double MaximumMeshSize { get; set; }
        /// <summary>
        /// Converts shape to mesh (set of triangles)
        /// </summary>
        /// <returns></returns>
        IMesh Triangulate();
    }
}
