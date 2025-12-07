using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Services.ColorServices;
using HelixToolkit.Maths;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Services.Reports.Services
{
    public class MaterialService : IMaterialService
    {
        public PhongMaterial CreateValueMaterial(double value, IValueRange range, IColorMap colorMap)
        {
            var c = ColorOperations.GetColorByValue(range, colorMap, value);
            return new PhongMaterial
            {
                DiffuseColor = new Color4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f),
                SpecularShininess = 50f
            };
        }

        public PBRMaterial CreateTransparentPlaneMaterial(float opacity)
        {
            return new PBRMaterial
            {
                AlbedoColor = new Color4(0.5f, 0.5f, 0.5f, opacity),
                RoughnessFactor = 0.8f,
                MetallicFactor = 0.0f
            };
        }
    }

}
