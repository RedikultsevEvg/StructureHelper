using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace StructureHelperCommon.Services.ColorServices
{
    public static class ColorProcessor
    {
        public static Color GetRandomColor()
        {
            Thread.Sleep(100);
            var randomR = new Random(new Random((int)DateTime.Now.Ticks % 1000).Next(50)).Next(0, 255);
            var randomG = new Random(new Random((int)DateTime.Now.Ticks % 200).Next(100, 200)).Next(0, 255);
            var randomB = new Random(new Random((int)DateTime.Now.Ticks % 50).Next(500, 1000)).Next(0, 255);
            return Color.FromRgb((byte)randomR, (byte)randomG, (byte)randomB);
        }
        public static void EditColor(ref Color wpfColor)
        {
            var winformsColor = System.Drawing.Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
            ColorDialog dialog = new ColorDialog();
            dialog.FullOpen = true;
            dialog.Color = winformsColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                wpfColor = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
            }
        }
    }
}
