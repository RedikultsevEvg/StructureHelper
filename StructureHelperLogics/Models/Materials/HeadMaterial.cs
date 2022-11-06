using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Materials;

namespace StructureHelper.Models.Materials
{
    public class HeadMaterial : IHeadMaterial
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public IHelperMaterial HelperMaterial {get; set;}

        //public MaterialDefinitionBase Material { get; set; }

        public HeadMaterial()
        {
            Color = ColorProcessor.GetRandomColor();
        }

        public object Clone()
        {
            IHeadMaterial material = new HeadMaterial
            {
                Name = Name,
                Color = Color,
                HelperMaterial = HelperMaterial.Clone() as IHelperMaterial
            };
            return material;
        }
    }
}
