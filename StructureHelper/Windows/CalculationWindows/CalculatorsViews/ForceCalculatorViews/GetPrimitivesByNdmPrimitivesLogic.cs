using FieldVisualizer.Entities.Values.Primitives;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class GetPrimitivesByNdmPrimitivesLogic
    {
        public static List<IShadedPrimitive> GetShadedPrimitives(IEnumerable<INdmPrimitive> primitives)
        {
            List<IShadedPrimitive> shadePrimitives = [];
            foreach (var item in primitives)
            {
                try
                {
                    ShadedPrimitive shadePrimitive = new ShadedPrimitive(item.Shape)
                    {
                        Center = item.Center
                    };
                    shadePrimitives.Add(shadePrimitive);
                }
                catch { }
            }
            return shadePrimitives;
        }
    }
}
