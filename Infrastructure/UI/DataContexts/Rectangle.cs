using System;
using System.Windows.Input;
using System.Windows.Media;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.MainWindow;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class Rectangle : PrimitiveBase
    {
        public Rectangle(double primitiveWidth, double primitiveHeight, double rectX, double rectY, MainViewModel mainViewModel) : base(PrimitiveType.Rectangle, rectX, rectY, mainViewModel)
        {
            PrimitiveWidth = primitiveWidth;
            PrimitiveHeight = primitiveHeight;
            ShowedX = 0;
            ShowedY = 0;
        }
    }
}
