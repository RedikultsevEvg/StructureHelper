using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace StructureHelper.Models.Materials
{
    public class HeadMaterial : IHeadMaterial
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public MaterialDefinitionBase Material { get; set; }

        public HeadMaterial()
        {
            Thread.Sleep(100);
            var randomR = new Random(new Random((int)DateTime.Now.Ticks % 1000).Next(50)).Next(0, 255);
            var randomG = new Random(new Random((int)DateTime.Now.Ticks % 200).Next(100, 200)).Next(0, 255);
            var randomB = new Random(new Random((int)DateTime.Now.Ticks % 50).Next(500, 1000)).Next(0, 255);
            Color = Color.FromRgb((byte)randomR, (byte)randomG, (byte)randomB);
        }
    }
}
