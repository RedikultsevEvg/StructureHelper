using StructureHelperCommon.Infrastructures.Exceptions;
using TriangleNet.Meshing;

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc/>
    public class MeshShapeLogic : IMeshShapeLogic
    {
        /// <inheritdoc/>
        public ICenterShape CenterShape { get; set; }
        /// <inheritdoc/>
        public double MinimumAngleInDegree { get; set; } = 25.0;
        /// <inheritdoc/>
        public double MaximumMeshSize { get; set; } = 0.01;

        /// <inheritdoc/>
        public IMesh Triangulate()
        {
            Check();
            if (CenterShape.Shape is ILinePolygonShape)
            {
                return ProcessLinePolygon();
            }
            else if (CenterShape.Shape is IVerticalDoubleTShape)
            {
                return ProcessDoubleTShape();
            }
            else if (CenterShape.Shape is IVerticalTShape)
            {
                return ProcessTShape();
            }
            else if (CenterShape.Shape is IRingShape)
            {
                return ProcessRingShape();
            }
            else if (CenterShape.Shape is ITrapezoidShape)
            {
                return ProcessTrapezoidShape();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(CenterShape.Shape));
            }
        }

        private IMesh ProcessDoubleTShape()
        {
            var logic = new VerticalDoubleTShapeMeshLogic()
            {
                CenterShape = CenterShape,
                MaximumMeshSize = MaximumMeshSize,
                MinimumAngleInDegree = MinimumAngleInDegree,
            };
            return logic.Triangulate();
        }

        private IMesh ProcessTrapezoidShape()
        {
            var logic = new TrapezoidMeshLogic()
            {
                CenterShape = CenterShape,
                MaximumMeshSize = MaximumMeshSize,
                MinimumAngleInDegree = MinimumAngleInDegree,
            };
            return logic.Triangulate();
        }

        private IMesh ProcessRingShape()
        {
            var logic = new RingShapeMeshLogic()
            {
                CenterShape = CenterShape,
                MaximumMeshSize = MaximumMeshSize,
                MinimumAngleInDegree = MinimumAngleInDegree,
            };
            return logic.Triangulate();
        }

        private IMesh ProcessTShape()
        {
            var logic = new VerticalTShapeMeshLogic()
            {
                CenterShape = CenterShape,
                MaximumMeshSize = MaximumMeshSize,
                MinimumAngleInDegree = MinimumAngleInDegree,
            };
            return logic.Triangulate();
        }

        private IMesh ProcessLinePolygon()
        {
            var logic = new LinePolygonMeshLogic()
            {
                CenterShape = CenterShape,
                MaximumMeshSize = MaximumMeshSize,
                MinimumAngleInDegree = MinimumAngleInDegree,
            };
            return logic.Triangulate();
        }

        private void Check()
        {
            if (CenterShape is null)
            {
                throw new StructureHelperNullReferenceException("Center shape");
            }
            if (CenterShape.Center is null)
            {
                throw new StructureHelperNullReferenceException("Center");
            }
            if (CenterShape.Shape is null)
            {
                throw new StructureHelperNullReferenceException("Shape");
            }
        }
    }
}
