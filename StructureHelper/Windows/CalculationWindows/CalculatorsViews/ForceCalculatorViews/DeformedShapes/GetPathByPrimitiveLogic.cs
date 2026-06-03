using Newtonsoft.Json;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public class GetPathByPrimitiveLogic : IGetPathByPrimitiveLogic
    {
        public INdmPrimitive Primitive { get; set; }
        public float Length { get; set; } = 3f;
        public int DivisionNumber { get; set; } = 20;

        public IDeformedPath GetPath()
        {
            CheckObject.ThrowIfNull(Primitive);

            DeformedPath path = new DeformedPath();

            float dz = Length / DivisionNumber;

            for (int i = 0; i <= DivisionNumber; i++)
            {
                path.Vectors.Add(new Vector3((float)Primitive.Center.X, (float)Primitive.Center.Y, - Length / 2 + i * dz));
            }
            return path;
        }
    }
}
