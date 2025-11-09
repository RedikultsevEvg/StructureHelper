using netDxf;
using netDxf.Entities;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Shapes.Logics
{
    public class CircleToDxfCircleConvertStrategy : IShapeConvertStrategy<Circle, ICircleShape>
    {
        public double Dx { get; set; } = 0;
        public double Dy { get; set; } = 0;
        public double Scale { get; set; } = 1;

        public Circle Convert(ICircleShape source)
        {
            Vector3 center = new Vector3() { X = Dx * Scale, Y = Dy * Scale};
            Circle circle = new Circle()
            {
                Radius = source.Diameter / 2 * Scale,
                Center = center,
            };
            return circle;
        }
    }
}
