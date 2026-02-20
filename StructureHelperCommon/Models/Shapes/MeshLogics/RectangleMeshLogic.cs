using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using TriangleNet.Meshing;

namespace StructureHelperCommon.Models.Shapes
{
    public class RectangleMeshLogic : IMeshShapeLogic
    {
        private IObjectConvertStrategy<ILinePolygonShape, IRectangleShape> transformLogic;
        private IMeshShapeLogic meshLogic;
        private IMeshShapeLogic MeshLogic => meshLogic ??= new LinePolygonMeshLogic();
        private IObjectConvertStrategy<ILinePolygonShape, IRectangleShape> TransformLogic => transformLogic ??= new RectangleShapeToPolygonConvertStrategy();

        public ICenterShape CenterShape { get; set; }
        public double MinimumAngleInDegree { get; set; } = 25.0;
        public double MaximumMeshSize { get; set; } = 0.01;

        public IMesh Triangulate()
        {
            if (CenterShape is null)
            {
                throw new StructureHelperNullReferenceException("Center shape");
            }
            if (CenterShape.Shape is not IRectangleShape rectangle)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(CenterShape.Shape) + ": Shape is not a rectangle");
            }
            var polygon = TransformLogic.Convert(rectangle);
            CenterShape centerShape = new(CenterShape.Center, polygon);
            MeshLogic.CenterShape = centerShape;
            MeshLogic.MaximumMeshSize = MaximumMeshSize;
            MeshLogic.MinimumAngleInDegree = MinimumAngleInDegree;
            return meshLogic.Triangulate();
        }
    }
}
