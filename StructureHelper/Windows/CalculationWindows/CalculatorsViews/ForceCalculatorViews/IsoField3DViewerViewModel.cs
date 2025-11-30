using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Services.ColorServices;
using FieldVisualizer.Services.PrimitiveServices;
using FieldVisualizer.Services.ValueRanges;
using HelixToolkit;
using HelixToolkit.Geometry;
using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Color = HelixToolkit.Maths.Color;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class IsoField3DViewerViewModel : ViewModelBase
    {
        const int RangeNumber = 16;
        private int userZoomFactor = 100;
        private IEnumerable<IPrimitiveSet> primitiveSets;
        private Element3D item0;
        private Element3D item1;
        private Element3D item2;
        private IPrimitiveSet selectedPrimitiveSet;
        private double zoomValue = 1.0;
        private IValueRange valueRange;
        private IEnumerable<IValueRange> valueRanges;
        private IEnumerable<IValueColorRange> valueColorRanges;
        private IColorMap _ColorMap;
        private ColorMapsTypes _ColorMapType;

        public ContourViewportViewModel ViewportViewModel { get; } = new ContourViewportViewModel();
        public IEnumerable<IPrimitiveSet> PrimitiveSets { get => primitiveSets;}

        public IsoField3DViewerViewModel(IEnumerable<IPrimitiveSet> primitiveSets)
        {
            this.primitiveSets = primitiveSets;
            _ColorMapType = ColorMapsTypes.LiraSpectrum;
            _ColorMap = ColorMapFactory.GetColorMap(_ColorMapType);
        }
        public IPrimitiveSet SelectedPrimitiveSet
        {
            get => selectedPrimitiveSet;
            set
            {
                selectedPrimitiveSet = value;
                OnPropertyChanged(nameof(SelectedPrimitiveSet));
                RebuildPrimitives();
            }
        }

        public int UserZoomFactor
        {
            get => userZoomFactor;
            set
            {
                userZoomFactor = value;
                OnPropertyChanged(nameof(UserZoomFactor));
                RebuildPrimitives ();
            }
        }

        private void RebuildPrimitives()
        {
            SetColor();
            item0 = ViewportViewModel.Viewport3D.Items[0];
            item1 = ViewportViewModel.Viewport3D.Items[1];
            item2 = ViewportViewModel.Viewport3D.Items[2];
            ViewportViewModel.Viewport3D.Items.Clear();
            ViewportViewModel.Viewport3D.Items.Add(item0);
            ViewportViewModel.Viewport3D.Items.Add(item1);
            ViewportViewModel.Viewport3D.Items.Add(item2);
            double maxValue = SelectedPrimitiveSet.ValuePrimitives.Max(x => x.Value) - SelectedPrimitiveSet.ValuePrimitives.Min(x => x.Value);
            zoomValue = UserZoomFactor / 100.0 / maxValue;
            foreach (var primitive in SelectedPrimitiveSet.ValuePrimitives)
            {
                if (primitive is ITrianglePrimitive triangle)
                {
                    var model = CreateTriangle(triangle);
                    ViewportViewModel.Viewport3D.Items.Add(model);
                }
            }
        }


        private MeshGeometryModel3D CreateTriangle(ITrianglePrimitive triangle)
        {
            // Triangle vertices
            Vector3 p0 = new Vector3((float)triangle.Point1.X, (float)triangle.Point1.Y, (float)(triangle.ValuePoint1 * zoomValue));
            Vector3 p1 = new Vector3((float)triangle.Point2.X, (float)triangle.Point2.Y, (float)(triangle.ValuePoint2 * zoomValue));
            Vector3 p2 = new Vector3((float)triangle.Point3.X, (float)triangle.Point3.Y, (float)(triangle.ValuePoint3 * zoomValue));

            var builder = new MeshBuilder();
            builder.AddTriangle(p0, p1, p2);

            var mesh = builder.ToMeshGeometry3D();

            var material = new PhongMaterial
            {
                DiffuseColor = ToColor4(ColorOperations.GetColorByValue(valueRange, _ColorMap, triangle.Value)),
                SpecularShininess = 50f
            };


            var model = new MeshGeometryModel3D
            {
                Geometry = mesh,
                Material = material,
                CullMode = SharpDX.Direct3D11.CullMode.None,
                ToolTip = triangle.Value
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

        internal void Refresh()
        {
            
            
        }

        private void SetColor()
        {
            valueRange = PrimitiveOperations.GetValueRange(SelectedPrimitiveSet.ValuePrimitives);
            valueRanges = ValueRangeOperations.DivideValueRange(valueRange, RangeNumber);
            valueColorRanges = ColorOperations.GetValueColorRanges(valueRange, valueRanges, _ColorMap);
        }
    }
}
