using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using TriangleNet.Meshing;

namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Logic of mesh fo T-shape
    /// </summary>
    internal class VerticalTShapeMeshLogic : IMeshShapeLogic
    {
        private IVerticalTShape tShape;
        /// <inheritdoc/>
        public ICenterShape CenterShape { get; set; }
        /// <inheritdoc/>
        public double MaximumMeshSize { get; set; } = 0.01;
        /// <inheritdoc/>
        public double MinimumAngleInDegree { get; set; } = 25.0;

        /// <inheritdoc/>
        public IMesh Triangulate()
        {
            if (CenterShape.Shape is not VerticalTShape verticalTShape)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(CenterShape.Shape) + ": Shape is not vertical T-shape");
            }
            tShape = verticalTShape;
            var transformLogic = new VerticalTShapeToPolygonConvertStrategy();
            var polygon = transformLogic.Convert(tShape);
            polygon.IsClosed = true;
            CenterShape centerShape = new(CenterShape.Center, polygon);
            var meshLogic = new LinePolygonMeshLogic()
            {
                CenterShape = centerShape,
                MaximumMeshSize = MaximumMeshSize,
                MinimumAngleInDegree = MinimumAngleInDegree,
            };
            return meshLogic.Triangulate();
        }
    }
}
