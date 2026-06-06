using HelixToolkit.Geometry;
using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;
using System.Numerics;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class DeformedShapeLogic
    {
        private IGetPathByPrimitiveLogic getPathByPrimitiveLogic = new GetPathByPrimitiveLogic();
        private IGetSectionByPrimitiveLogic getSectionByPrimitiveLogic = new GetSectionByPrimitiveLogic();
        private IModifyPathByCurvatureLogic modifyPathByCurvatureLogic = new ModifyPathByCurvatureLogic();
        private ForceTupleServiceLogic forceTupleServiceLogic = new ForceTupleServiceLogic();

        private Element3D item0;
        private Element3D item1;
        private Element3D item2;

        public IEnumerable<INdmPrimitive> NdmPrimitives { get; set; }
        public Viewport3DX Viewport {  get; set; }

        public IForceTuple ResultCurvature { get; set; }

        public float Length { get; set; } = 3f;

        public int DivisionNumber { get; set; } = 20;
        public float ScaleFactor { get; set; }
        public bool IsCapsShown { get; internal set; }
        public bool ConsiderResultCurvature { get; internal set; }
        public bool ConsiderPrestrainCurvature { get; internal set; }
        public bool CreateMesh { get; internal set; }
        public DeformedShapeSymmetrySetViewModel SymmetrySet { get; internal set; }

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

            if (CreateMesh == true)
            {
                getSectionByPrimitiveLogic = new GetSectionByMeshLogic();
            }
            else
            {
                getSectionByPrimitiveLogic = new GetSectionByPrimitiveLogic();
            }
            List<IDeformedSection> sections = getSectionByPrimitiveLogic.GetSection(ndmPrimitive);
            getPathByPrimitiveLogic.Primitive = ndmPrimitive;
            getPathByPrimitiveLogic.DivisionNumber = DivisionNumber;
            getPathByPrimitiveLogic.Length = Length;
            IDeformedPath path = getPathByPrimitiveLogic.GetPath();

            IForceTuple fullCurvature = GetFullCurvature(ndmPrimitive);

            path = modifyPathByCurvatureLogic.ProcessPath(path, fullCurvature, ScaleFactor);

            Vector3 sectionXAxis = new(1, 0, 0);
            foreach (var section in sections)
            {
                List<IDeformedPath> paths = [];
                foreach (var symmetrySet in SymmetrySet.SymmetrySets)
                {
                    for (int i = 0; i < symmetrySet.ElementCount; i++)
                    {
                        var extraPath = modifyPathByCurvatureLogic.ProcessPath(path,
                            (float)(i * symmetrySet.DX),
                            (float)(i * symmetrySet.DY),
                            (float)(i * symmetrySet.DZ));
                        paths.Add(extraPath);
                    }
                }
                foreach (var extraPath in paths)
                {
                    builder.AddTube(extraPath.Vectors, null, null, section.Vertices, sectionXAxis, false, true, IsCapsShown, IsCapsShown);
                }
                //builder.AddArrow(path.Vectors[^1], path.Vectors[1], 0.2f);
            }

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

        private IForceTuple GetFullCurvature(INdmPrimitive ndmPrimitive)
        {
            IForceTuple fullCurvature = new ForceTuple();
            if (ResultCurvature is not null && ConsiderResultCurvature == true)
            {
                fullCurvature = forceTupleServiceLogic.SumTuples(fullCurvature, ResultCurvature);
            }
            if (ConsiderPrestrainCurvature == true)
            {
                ForceTuple prestrainCurvature = GetCurvatureFromPrestrain(ndmPrimitive.NdmElement.UsersPrestrain);
                fullCurvature = forceTupleServiceLogic.SumTuples(fullCurvature, prestrainCurvature);
                prestrainCurvature = GetCurvatureFromPrestrain(ndmPrimitive.NdmElement.AutoPrestrain);
                fullCurvature = forceTupleServiceLogic.SumTuples(fullCurvature, prestrainCurvature);
            }

            return fullCurvature;
        }

        private static ForceTuple GetCurvatureFromPrestrain(IForceTuple? prestrain = null)
        {
            if (prestrain == null)
            {
                return new ForceTuple();
            }
            ForceTuple prestrainCurvature = new()
            {
                Mx = prestrain.Mx,
                My = prestrain.My,
                Nz = prestrain.Nz,
            };
            return prestrainCurvature;
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
