using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using HelixToolkit.Maths;
using HelixToolkit.Wpf.SharpDX;
using System.Collections.Generic;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public interface IGetModels3dLogic
    {
        IColorMap ColorMap { get; set; }
        bool InvertNormal { get; set; }
        bool ShowZeroPlane { get; set; }
        IValueRange ValueRange { get; set; }
        double ZoomValue { get; set; }

        static abstract Color4 ToColor4(System.Windows.Media.Color c);
        void GetModels3d(IEnumerable<IValuePrimitive> valuePrimitives, Viewport3DX viewport);
    }
}