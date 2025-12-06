using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Services.ColorServices;
using HelixToolkit.Geometry;
using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class GetModels3dByValuePrimivesLogic : IGetModels3dLogic
    {
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
            float cylinderHeight = (float)(circle.Value * ZoomValue);
            Vector3 p1 = new Vector3((float)circle.CenterX, (float)circle.CenterY, cylinderHeight);   // top center

            builder.AddCylinder(p0, p1, (float)circle.Diameter, 8);

            // Build geometry
            var cylinderGeometry = builder.ToMeshGeometry3D();

            // Create material (constant grey)
            var material = new PhongMaterial
            {
                DiffuseColor = ToColor4(ColorOperations.GetColorByValue(ValueRange, ColorMap, circle.Value)),
                SpecularShininess = 50f
            };

            // Create model
            var cylinderModel = new MeshGeometryModel3D
            {
                Geometry = cylinderGeometry,
                Material = material
            };
            return cylinderModel;
        }

        private MeshGeometryModel3D CreateZeroTriangle(ITrianglePrimitive triangle)
        {
            // Triangle vertices
            Vector3 p0 = new Vector3((float)triangle.Point1.X, (float)triangle.Point1.Y, 0);
            Vector3 p1 = new Vector3((float)triangle.Point2.X, (float)triangle.Point2.Y, 0);
            Vector3 p2 = new Vector3((float)triangle.Point3.X, (float)triangle.Point3.Y, 0);

            var builder = new MeshBuilder();
            builder.AddTriangle(p0, p1, p2);

            var mesh = builder.ToMeshGeometry3D();

            var material = new PBRMaterial
            {
                AlbedoColor = new Color4(0.5f, 0.5f, 0.5f, 0.4f), // 40% opacity
                RoughnessFactor = 0.8f,
                MetallicFactor = 0.0f
            };


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
            // Triangle vertices
            Vector3 p0 = new Vector3((float)triangle.Point1.X, (float)triangle.Point1.Y, (float)(triangle.ValuePoint1 * ZoomValue));
            Vector3 p1 = new Vector3((float)triangle.Point2.X, (float)triangle.Point2.Y, (float)(triangle.ValuePoint2 * ZoomValue));
            Vector3 p2 = new Vector3((float)triangle.Point3.X, (float)triangle.Point3.Y, (float)(triangle.ValuePoint3 * ZoomValue));

            var builder = new MeshBuilder();
            builder.AddTriangle(p0, p1, p2);

            var mesh = builder.ToMeshGeometry3D();

            var material = new PhongMaterial
            {
                DiffuseColor = ToColor4(ColorOperations.GetColorByValue(ValueRange, ColorMap, triangle.Value)),
                SpecularShininess = 50f
            };


            var model = new MeshGeometryModel3D
            {
                Geometry = mesh,
                Material = material,
                CullMode = SharpDX.Direct3D11.CullMode.None,
                InvertNormal = InvertNormal
            };
            return model;
        }

        public static Color4 ToColor4(System.Windows.Media.Color c)
        {
            return new Color4(
                c.R / 255f,
                c.G / 255f,
                c.B / 255f,
                c.A / 255f
            );
        }
    }
}
