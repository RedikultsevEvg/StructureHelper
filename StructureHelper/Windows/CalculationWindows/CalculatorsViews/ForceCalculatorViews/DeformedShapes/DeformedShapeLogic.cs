using HelixToolkit.Geometry;
using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class DeformedShapeLogic
    {
        private IGetPathByPrimitiveLogic getPathByPrimitiveLogic = new GetPathByPrimitiveLogic();
        private IGetSectionByPrimitiveLogic getSectionByPrimitiveLogic = new GetSectionByPrimitiveLogic();
        private IModifyPathByCurvatureLogic modifyPathByCurvatureLogic = new ModifyPathByCurvatureLogic();
        private Element3D item0;
        private Element3D item1;
        private Element3D item2;

        public IEnumerable<INdmPrimitive> NdmPrimitives { get; set; }
        public Viewport3DX Viewport {  get; set; }

        public IForceTuple Curvature { get; set; }

        public float Length { get; set; } = 3f;

        public int DivisionNumber { get; set; } = 20;
        public float ScaleFactor { get; set; }
        public bool IsCapsShown { get; internal set; }

        public void GetModels3d()
        {
            item0 = Viewport.Items[0];
            item1 = Viewport.Items[1];
            item2 = Viewport.Items[2];
            Viewport.Items.Clear();
            Viewport.Items.Add(item0);
            Viewport.Items.Add(item1);
            Viewport.Items.Add(item2);
            foreach (var primitive in NdmPrimitives)
            {
                var model = CreatePrismByPolygon(primitive);
                Viewport.Items.Add(model);
            }
        }

        private MeshGeometryModel3D CreatePrismByPolygon(INdmPrimitive ndmPrimitive)
        {
            // Create mesh along Z-axis
            var builder = new MeshBuilder();

            List<Vector2> section = getSectionByPrimitiveLogic.GetSection(ndmPrimitive);
            getPathByPrimitiveLogic.Primitive = ndmPrimitive;
            getPathByPrimitiveLogic.DivisionNumber = DivisionNumber;
            getPathByPrimitiveLogic.Length = Length;
            IDeformedPath path = getPathByPrimitiveLogic.GetPath();
            if (Curvature is not null)
            {
                path = modifyPathByCurvatureLogic.ProcessPath(path, Curvature, ScaleFactor);
            }

            builder.AddTube(path.Vectors, null, null, section, (new Vector3(1, 0, 0)), false, true, IsCapsShown, IsCapsShown);


            var mesh = builder.ToMeshGeometry3D();
            System.Windows.Media.Color c = GetColor(ndmPrimitive);

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
            };


            return prismModel;
        }

        private static System.Windows.Media.Color GetColor(INdmPrimitive ndmPrimitive)
        {
            if (ndmPrimitive.VisualProperty.SetMaterialColor == true && ndmPrimitive.NdmElement.HeadMaterial is not null)
            {
                return ndmPrimitive.NdmElement.HeadMaterial.Color;
            }
            return ndmPrimitive.VisualProperty.Color;
        }
    }
}
