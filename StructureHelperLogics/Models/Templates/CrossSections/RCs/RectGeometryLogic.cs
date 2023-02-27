using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections.RCs
{
    public class RectGeometryLogic : IRCGeometryLogic
    {
        IRectangleBeamTemplate template;
        IHeadMaterial concrete => HeadMaterials.ToList()[0];
        IHeadMaterial reinforcement => HeadMaterials.ToList()[1];

        RectangleShape rect => template.Shape as RectangleShape;
        double width => rect.Width;
        double height => rect.Height;
        double area1 => Math.PI * template.BottomDiameter * template.BottomDiameter / 4d;
        double area2 => Math.PI * template.TopDiameter * template.TopDiameter / 4d;
        double gap => template.CoverGap;

        public IEnumerable<IHeadMaterial> HeadMaterials { get; set; }

        public RectGeometryLogic(IRectangleBeamTemplate template)
        {
            this.template = template;
        }

        public IEnumerable<INdmPrimitive> GetNdmPrimitives()
        {
            List<INdmPrimitive> primitives = new List<INdmPrimitive>();
            primitives.AddRange(GetConcretePrimitives());
            primitives.AddRange(GetCornerReinfrocementPrimitives());
            if (template.WidthCount > 2 || template.HeightCount > 2)
            {
                primitives.AddRange(GetMiddleReinfrocementPrimitives());
            }          
            return primitives;
        }

        private IEnumerable<INdmPrimitive> GetConcretePrimitives()
        {
            var primitives = new List<INdmPrimitive>();
            var rectangle = new RectanglePrimitive(concrete) { Width = width, Height = height, Name = "Concrete block" };
            primitives.Add(rectangle);
            return primitives;
        }

        private IEnumerable<INdmPrimitive> GetCornerReinfrocementPrimitives()
        {
            double[] xs = new double[] { -width / 2 + gap, width / 2 - gap };
            double[] ys = new double[] { -height / 2 + gap, height / 2 - gap };

            List<INdmPrimitive> primitives = new List<INdmPrimitive>();
            var point = new PointPrimitive(reinforcement) { CenterX = xs[0], CenterY = ys[0], Area = area1, Name = "Left bottom point" };
            primitives.Add(point);
            point = new PointPrimitive(reinforcement) { CenterX = xs[1], CenterY = ys[0], Area = area1, Name = "Right bottom point" };
            primitives.Add(point);
            point = new PointPrimitive(reinforcement) { CenterX = xs[0], CenterY = ys[1], Area = area2, Name = "Left top point" };
            primitives.Add(point);
            point = new PointPrimitive(reinforcement) { CenterX = xs[1], CenterY = ys[1], Area = area2, Name = "Right top point" };
            primitives.Add(point);
            return primitives;
        }

        private IEnumerable<INdmPrimitive> GetMiddleReinfrocementPrimitives()
        {
            double[] xs = new double[] { -width / 2 + gap, width / 2 - gap };
            double[] ys = new double[] { -height / 2 + gap, height / 2 - gap };

            List<INdmPrimitive> primitives = new List<INdmPrimitive>();
            IPointPrimitive point;
            if (template.WidthCount > 2)
            {
                int count = template.WidthCount - 1;
                double dist = (xs[1] - xs[0]) / count;
                for (int i = 1; i < count; i++)
                {
                    point = new PointPrimitive(reinforcement) { CenterX = xs[0] + dist * i, CenterY = ys[0], Area = area1, Name = $"Bottom point {i}" };
                    primitives.Add(point);
                    point = new PointPrimitive(reinforcement) { CenterX = xs[0] + dist * i, CenterY = ys[1], Area = area2, Name = $"Top point {i}" };
                    primitives.Add(point);
                }
            }
            if (template.HeightCount > 2)
            {
                int count = template.HeightCount - 1;
                double dist = (ys[1] - ys[0]) / count;
                for (int i = 1; i < count; i++)
                {
                    point = new PointPrimitive(reinforcement) { CenterX = xs[0], CenterY = ys[0] + dist * i, Area = area1, Name = $"Left point {i}" };
                    primitives.Add(point);
                    point = new PointPrimitive(reinforcement) { CenterX = xs[1], CenterY = ys[0] + dist * i, Area = area1, Name = $"Right point {i}" };
                    primitives.Add(point);
                }
            }
            return primitives;
        }
    }
}
