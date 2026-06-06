using LoaderCalculator.Infrastructure.Geometry;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;
using System.Numerics;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    internal class GetSectionByMeshLogic : IGetSectionByPrimitiveLogic
    {
        private GetSectionByPrimitiveLogic sectionByPrimitiveLogic;
        private GetSectionByPrimitiveLogic SectionByPrimitiveLogic => sectionByPrimitiveLogic ??= new();
        public List<IDeformedSection> GetSection(INdmPrimitive primitive)
        {
            List<IDeformedSection> result = [];
            if (primitive is IPointNdmPrimitive pointNdmPrimitive)
            {
                return SectionByPrimitiveLogic.GetSection(pointNdmPrimitive);
            }
            CenterShape centerShape = new(primitive.Center, primitive.Shape);
            double maxSize = 0.05;
            if (primitive is IHasDivisionSize divisionSize)
            {
                maxSize = divisionSize.DivisionSize.NdmMaxSize;
            } 
            MeshShapeLogic meshShapeLogic = new()
            {
                CenterShape = centerShape,
                MaximumMeshSize = maxSize
            };

            var mesh = meshShapeLogic.Triangulate();
            foreach (var triangle in mesh.Triangles)
            {
                DeformedSection section = new();
                for (int i = 0; i < 3; i++)
                {
                    var vertex = triangle.GetVertex(i);
                    section.Vertices.Add(new Vector2((float)vertex.X, (float)vertex.Y));
                }
                result.Add(section);
            }
            return result;
        }
    }
}
