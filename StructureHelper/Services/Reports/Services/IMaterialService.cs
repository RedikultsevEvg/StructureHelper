using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Services.Reports.Services
{
    public interface IMaterialService
    {
        PhongMaterial CreateValueMaterial(double value, IValueRange range, IColorMap colorMap);
        PBRMaterial CreateTransparentPlaneMaterial(float opacity);
    }

}
