using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Services.ShapeServices
{
    public static class ShapeService
    {
        public static void CopyLineProperties(ILineShape source, ILineShape target)
        {
            target.StartPoint.X = source.StartPoint.X;
            target.StartPoint.Y = source.StartPoint.Y;
            target.EndPoint.X = source.EndPoint.X;
            target.EndPoint.Y = source.EndPoint.Y;
        }

        public static void CopyRectangleProperties(IRectangleShape source, IRectangleShape target)
        {
            target.Width = source.Width;
            target.Height = source.Height;
            target.Angle = source.Angle;
        }

        public static void CopyCircleProperties(ICircleShape source, ICircleShape target)
        {
            target.Diameter = source.Diameter;
        }
    }
}
