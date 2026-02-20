using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Meshing;

namespace StructureHelperCommon.Models.Shapes
{
    public class VerticalDoubleTShapeMeshLogic : IMeshShapeLogic
    {
        private const double minThicknessDivision = 2.0;
        private IObjectConvertStrategy<ILinePolygonShape, IVerticalDoubleTShape> transformLogic;
        private IMeshShapeLogic meshLogic;
        private IMeshShapeLogic MeshLogic => meshLogic ??= new LinePolygonMeshLogic();
        private IObjectConvertStrategy<ILinePolygonShape, IVerticalDoubleTShape> TransformLogic => transformLogic ??= new VerticalDoubleTShapeToPolygonConvertStrategy();
        /// <inheritdoc/>
        public ICenterShape CenterShape { get; set; }
        /// <inheritdoc/>
        public double MaximumMeshSize { get; set; } = 0.01;
        /// <inheritdoc/>
        public double MinimumAngleInDegree { get; set; } = 25.0;

        public VerticalDoubleTShapeMeshLogic(IObjectConvertStrategy<ILinePolygonShape, IVerticalDoubleTShape> transformLogic, IMeshShapeLogic meshLogic)
        {
            this.transformLogic = transformLogic;
            this.meshLogic = meshLogic;
        }

        public VerticalDoubleTShapeMeshLogic()
        {

        }

        /// <inheritdoc/>
        public IMesh Triangulate()
        {
            if (CenterShape is null)
            {
                throw new StructureHelperNullReferenceException("Center shape");
            }
            if (CenterShape.Shape is not VerticalDoubleTShape doubleTShape)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(CenterShape.Shape) + ": Shape is not vertical I-shape");
            }
            var polygon = TransformLogic.Convert(doubleTShape);
            List<double> maxSizeList = [MaximumMeshSize, doubleTShape.TopFlangeThickness / minThicknessDivision, doubleTShape.WebThickness / minThicknessDivision, doubleTShape.BottomFlangeThickness / minThicknessDivision];
            CenterShape centerShape = new(CenterShape.Center, polygon);
            MeshLogic.CenterShape = centerShape;
            MeshLogic.MaximumMeshSize = maxSizeList.Min();
            MeshLogic.MinimumAngleInDegree = MinimumAngleInDegree;
            return meshLogic.Triangulate();
        }
    }
}
