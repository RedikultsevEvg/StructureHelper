using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.InfraStructures.Exceptions;
using FieldVisualizer.InfraStructures.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FieldVisualizer.Services.PrimitiveServices
{
    public static class PrimitiveOperations
    {
        public static IValueRange GetValueRange(IEnumerable<IValuePrimitive> valuePrimitives)
        {
            double minVal =0d, maxVal = 0d;
            foreach (var primitive in valuePrimitives)
            {
                minVal = Math.Min(minVal, primitive.Value);
                maxVal = Math.Max(maxVal, primitive.Value);
            }
            return new ValueRange() { BottomValue = minVal, TopValue = maxVal };
        }

        public static double[] GetMinMaxX(IEnumerable<IValuePrimitive> valuePrimitives)
        {
            List<double> coords = GetXs(valuePrimitives);
            return new double[] { coords.Min(), coords.Max() };
        }

        public static double GetSizeX(IEnumerable<IValuePrimitive> valuePrimitives)
        {
            double[] coords = GetMinMaxX(valuePrimitives);
            return coords[1] - coords[0];
        }

        public static double[] GetMinMaxY(IEnumerable<IValuePrimitive> valuePrimitives)
        {
            List<double> coords = GetYs(valuePrimitives);
            return new double[] { coords.Min(), coords.Max() };
        }

        public static double GetSizeY(IEnumerable<IValuePrimitive> valuePrimitives)
        {
            double[] coords = GetMinMaxY(valuePrimitives);
            return coords[1] - coords[0];
        }

        public static List<double> GetXs(IEnumerable<IValuePrimitive> valuePrimitives)
        {
            List<double> coords = new List<double>();
            foreach (var primitive in valuePrimitives)
            {
                if (primitive is IRectanglePrimitive)
                {
                    IRectanglePrimitive rectanglePrimitive = primitive as IRectanglePrimitive;
                    coords.Add(rectanglePrimitive.CenterX + rectanglePrimitive.Width / 2);
                    coords.Add(rectanglePrimitive.CenterX - rectanglePrimitive.Width / 2);
                }
                else if (primitive is ICirclePrimitive)
                {
                    ICirclePrimitive circlePrimitive = primitive as ICirclePrimitive;
                    coords.Add(circlePrimitive.CenterX + circlePrimitive.Diameter / 2);
                    coords.Add(circlePrimitive.CenterX - circlePrimitive.Diameter / 2);
                }
                else { throw new FieldVisulizerException(ErrorStrings.PrimitiveTypeIsUnknown);}
            }
            return coords;
        }

        public static List<double> GetYs(IEnumerable<IValuePrimitive> valuePrimitives)
        {
            List<double> coords = new List<double>();
            foreach (var primitive in valuePrimitives)
            {
                if (primitive is IRectanglePrimitive)
                {
                    IRectanglePrimitive rectanglePrimitive = primitive as IRectanglePrimitive;
                    coords.Add(rectanglePrimitive.CenterY + rectanglePrimitive.Height / 2);
                    coords.Add(rectanglePrimitive.CenterY - rectanglePrimitive.Height / 2);
                }
                else if (primitive is ICirclePrimitive)
                {
                    ICirclePrimitive circlePrimitive = primitive as ICirclePrimitive;
                    coords.Add(circlePrimitive.CenterY + circlePrimitive.Diameter / 2);
                    coords.Add(circlePrimitive.CenterY - circlePrimitive.Diameter / 2);
                }
                else { throw new FieldVisulizerException(ErrorStrings.PrimitiveTypeIsUnknown); }
            }
            return coords;
        }
    }
}
