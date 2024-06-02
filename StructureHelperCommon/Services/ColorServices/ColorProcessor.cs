using System;
using System.Threading;
using System.Windows.Forms;
using Color = System.Windows.Media.Color;

namespace StructureHelperCommon.Services.ColorServices
{
    public static class ColorProcessor
    {
        private static readonly Random random = new Random();
        public static Color GetRandomColor()
        {
            int r = random.Next(0, 255);
            int g = random.Next(0, 255);
            int b = random.Next(0, 255);

            // check, that colors are different
            while (Math.Abs(r - g) < 30 || Math.Abs(g - b) < 30 || Math.Abs(b - r) < 30)
            {
                r = random.Next(0, 255);
                g = random.Next(0, 255);
                b = random.Next(0, 255);
            }

            return Color.FromRgb((byte)r, (byte)g, (byte)b);
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
