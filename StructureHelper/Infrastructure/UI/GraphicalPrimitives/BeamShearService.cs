using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public static class BeamShearService
    {
        public static double GetAbsoluteLevel(IBeamSpanLoad load, IInclinedSection inclinedSection)
        {
            double height;
            IShape shape = inclinedSection.BeamShearSection.Shape;
            if (shape is IRectangleShape rectangle) { height = rectangle.Height; }
            else if (shape is ICircleShape circle) { height = circle.Diameter; }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(shape) + $": load {load.Name} shape");
            }
            double level = (load.RelativeLoadLevel + 0.5) * height;
            return level;
        }
    }
}
