using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using HelixToolkit.Geometry;
using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using StructureHelper.Services.Reports.Services;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Shapes;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class GetModels3dByValuePrimivesLogic : IGetModels3dLogic
    {
        private MaterialService materialService = new();
        public double ZoomValue { get; set; }
        public bool InvertNormal { get; set; }
        public IColorMap ColorMap { get; set; }
        public IValueRange ValueRange { get; set; }
        public bool ShowZeroPlane { get; set; }

        public void GetModels3d(IEnumerable<IValuePrimitive> valuePrimitives, Viewport3DX viewport)
        {
            foreach (var primitive in valuePrimitives)
            {
                if (primitive is ICirclePrimitive circlePrimitive)
                {
                    if (circlePrimitive.Value != 0)
                    {
                        var model2 = CreateCylinder(circlePrimitive);
                        viewport.Items.Add(model2);
                    }
                }
                else if (primitive is ITrianglePrimitive triangle)
                {
                    var model2 = CreateTriangle(triangle);
                    viewport.Items.Add(model2);
                    if (ShowZeroPlane == true)
                    {
                        var model1 = CreateZeroTriangle(triangle);
                        viewport.Items.Add(model1);
                    }
                }
            }
        }

        private MeshGeometryModel3D CreateCylinder(ICirclePrimitive circle)
        {
            // Create mesh along Z-axis
            var builder = new MeshBuilder();

            Vector3 p0 = new Vector3((float)circle.CenterX, (float)circle.CenterY, 0);    // bottom center
            Vector3 p1 = new Vector3((float)circle.CenterX, (float)circle.CenterY, (float)(circle.Value * ZoomValue));   // top center

            builder.AddCylinder(p0, p1, (float)circle.Diameter, 8);

            // Build geometry
            var cylinderGeometry = builder.ToMeshGeometry3D();

            // Create material (constant grey)
            var material = materialService.CreateValueMaterial(circle.Value, ValueRange, ColorMap);

            // Create model
            var cylinderModel = new MeshGeometryModel3D
            {
                Geometry = cylinderGeometry,
                Material = material
            };
            return cylinderModel;
        }

        private MeshGeometryModel3D CreatePrismByPolygon(ILinePolygonShape polygonShape, float height, int divisions = 30)
        {
            // Create mesh along Z-axis
            var builder = new MeshBuilder();

            int verticesCount = polygonShape.Vertices.Count;

            List<Vector2> section = [];
            foreach (var vertex in polygonShape.Vertices)
            {
                section.Add(new Vector2((float)vertex.Point.X, (float)vertex.Point.Y));
            }

            float dz = height / divisions;

            //for (int i = 0; i <= divisions - 1; i++)
            //{
            //    float z0 = i * dz;
            //    float z1 = (i + 1) * dz;
            //    builder.AddExtrudedGeometry(section, Vector3.UnitX,
            //    new Vector3(0, 0, z0),
            //    new Vector3(0, 0, z1));
            //}

            List<Vector3> path = [];

            for (int i = 0; i <= divisions; i++)
            {
                path.Add(new Vector3(0, 0, - height / 2 + i * dz));
            }

            builder.AddTube(path, null, null, section, null, true, true);

            //builder.AddSphere(new Vector3(0, 0, 0), 0.5f);
            //builder.AddBox(new Vector3(0, 0, 0), 1, 0.25f, 2, BoxFaces.All);

            var mesh = builder.ToMeshGeometry3D();

            System.Windows.Media.Color c = ColorProcessor.GetRandomColor();

            // Create model
            var prismModel = new MeshGeometryModel3D
            {
                Geometry = mesh,
                Material = new DiffuseMaterial()
                {
                    DiffuseColor = new Color4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f),
                    //SpecularShininess = 50f
                },
                CullMode = SharpDX.Direct3D11.CullMode.None,
                InvertNormal = InvertNormal,
            };


            return prismModel;
        }

        private MeshGeometryModel3D CreateZeroTriangle(ITrianglePrimitive triangle)
        {
            var mesh = CreateTriangleMesh(triangle, false);
            var material = materialService.CreateTransparentPlaneMaterial(0.4f);
            var model = new MeshGeometryModel3D
            {
                Geometry = mesh,
                Material = material,
                CullMode = SharpDX.Direct3D11.CullMode.None,
                InvertNormal = InvertNormal,
            };
            return model;
        }

        private MeshGeometryModel3D CreateTriangle(ITrianglePrimitive triangle)
        {
            var mesh = CreateTriangleMesh(triangle, true);
            var material = materialService.CreateValueMaterial(triangle.Value, ValueRange, ColorMap);
            var model = new MeshGeometryModel3D
            {
                Geometry = mesh,
                Material = material,
                CullMode = SharpDX.Direct3D11.CullMode.None,
                InvertNormal = InvertNormal
            };
            return model;
        }

        private HelixToolkit.SharpDX.MeshGeometry3D CreateTriangleMesh(ITrianglePrimitive triangle, bool scaleByValue)
        {
            float z1 = scaleByValue ? (float)(triangle.ValuePoint1 * ZoomValue) : 0;
            float z2 = scaleByValue ? (float)(triangle.ValuePoint2 * ZoomValue) : 0;
            float z3 = scaleByValue ? (float)(triangle.ValuePoint3 * ZoomValue) : 0;

            Vector3 p0 = new((float)triangle.Point1.X, (float)triangle.Point1.Y, z1);
            Vector3 p1 = new((float)triangle.Point2.X, (float)triangle.Point2.Y, z2);
            Vector3 p2 = new((float)triangle.Point3.X, (float)triangle.Point3.Y, z3);

            var builder = new MeshBuilder();
            builder.AddTriangle(p0, p1, p2);

            return builder.ToMeshGeometry3D();
        }


        private Color4 ToColor4(System.Windows.Media.Color c)
        {
            return new Color4(
                c.R / 255f,
                c.G / 255f,
                c.B / 255f,
                c.A / 255f
            );
        }

        public void GetModels3d(IEnumerable<INdmPrimitive> ndmPrimitives, Viewport3DX viewport)
        {
            foreach (var primitive in ndmPrimitives)
            {
                try
                {
                    if (primitive.Shape is IRectangleShape rectangleShape)
                    {
                        var logic = new RectangleShapeToPolygonConvertStrategy();
                        var polygon = logic.Convert(rectangleShape);
                        var model = CreatePrismByPolygon(polygon, 2f);
                        viewport.Items.Add(model);
                    }
                    else if (primitive.Shape is ILinePolygonShape linePolygonShape)
                    {
                        var model = CreatePrismByPolygon(linePolygonShape, 3f);
                        viewport.Items.Add(model);
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
