using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Models.Primitives.Factories
{
    internal static class PrimitiveFactory
    {
        public static IEnumerable<PrimitiveBase> GetRectangleRCElement(RectangleBeamTemplate template, IHeadMaterial concrete, IHeadMaterial reinforcement)
        {
            List<PrimitiveBase> primitives = new List<PrimitiveBase>();
            var rect = template.Shape as StructureHelperCommon.Models.Shapes.RectangleShape;
            var width = rect.Width;
            var height = rect.Height;
            var area1 = Math.PI * template.BottomDiameter * template.BottomDiameter / 4d;
            var area2 = Math.PI * template.TopDiameter * template.TopDiameter / 4d;
            var gap = template.CoverGap;

            double[] xs = new double[] { -width / 2 + gap, width / 2 - gap };
            double[] ys = new double[] { -height / 2 + gap, height / 2 - gap };

            var rectangle = new RectanglePrimitive() { Width = width, Height = height, Name = "Concrete block" };
            primitives.Add(new RectangleViewPrimitive(rectangle) { HeadMaterial = concrete});
            var point = new PointPrimitive() { CenterX = xs[0], CenterY = ys[0], Area = area1};
            var viewPoint = new PointViewPrimitive(point) { HeadMaterial = reinforcement, Name = "Left bottom point" };
            viewPoint.RegisterDeltas(xs[0], ys[0]);
            primitives.Add(viewPoint);
            point = new PointPrimitive() {CenterX = xs[1], CenterY = ys[0], Area = area1 };
            viewPoint = new PointViewPrimitive(point) { HeadMaterial = reinforcement, Name = "Right bottom point" };
            primitives.Add(viewPoint);
            point = new PointPrimitive() { CenterX = xs[0], CenterY = ys[1], Area = area2 };
            viewPoint = new PointViewPrimitive(point) { HeadMaterial = reinforcement, Name = "Left top point" };
            primitives.Add(viewPoint);
            point = new PointPrimitive() { CenterX = xs[1], CenterY = ys[1], Area = area2 };
            viewPoint = new PointViewPrimitive(point) { HeadMaterial = reinforcement, Name = "Right top point" };
            viewPoint.RegisterDeltas(xs[1], ys[1]);
            primitives.Add(viewPoint);

            if (template.WidthCount > 2)
            {
                int count = template.WidthCount - 1;
                double dist = (xs[1] - xs[0]) / count;
                for (int i = 1; i < count; i++)
                {
                    point = new PointPrimitive() {CenterX = xs[0] + dist * i, CenterY = ys[0], Area = area1 };
                    viewPoint = new PointViewPrimitive(point) { HeadMaterial = reinforcement, Name = $"Bottom point {i}" };
                    primitives.Add(viewPoint);

                    point = new PointPrimitive() { CenterX = xs[0] + dist * i, CenterY = ys[1], Area = area2 };
                    viewPoint = new PointViewPrimitive(point) { HeadMaterial = reinforcement, Name = $"Top point {i}" };
                    primitives.Add(viewPoint);
                }
            }
            if (template.HeightCount > 2)
            {
                int count = template.HeightCount - 1;
                double dist = (ys[1] - ys[0]) / count;
                for (int i = 1; i < count; i++)
                {
                    point = new PointPrimitive() {CenterX = xs[0], CenterY = ys[0] + dist * i, Area = area1 };
                    viewPoint = new PointViewPrimitive(point) { HeadMaterial = reinforcement, Name = $"Left point {i}" };
                    primitives.Add(viewPoint);

                    point = new PointPrimitive() { CenterX = xs[1], CenterY = ys[0] + dist * i, Area = area1 };
                    viewPoint = new PointViewPrimitive(point) { HeadMaterial = reinforcement, Name = $"Right point {i}" };
                    primitives.Add(viewPoint);
                }
            }
            return primitives;
        }
    }
}
