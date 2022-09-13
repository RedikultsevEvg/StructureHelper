using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.WindowsOperation;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.ResultData;
using LoaderCalculator.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.ResultViewers
{
    public static class ShowIsoFieldResult
    {
        public static void ShowResult(IStrainMatrix strainMatrix, IEnumerable<INdm> ndms, IEnumerable<IResultFunc> resultFuncs)
        {
            var primitiveSets = GetPrimitiveSets(strainMatrix, ndms, resultFuncs);
            FieldViewerOperation.ShowViewer(primitiveSets);
        }

        public static List<IPrimitiveSet> GetPrimitiveSets(IStrainMatrix strainMatrix, IEnumerable<INdm> ndms, IEnumerable<IResultFunc> resultFuncs)
        {
            List<IPrimitiveSet> primitiveSets = new List<IPrimitiveSet>();
            foreach (var valDelegate in resultFuncs)
            {
                PrimitiveSet primitiveSet = new PrimitiveSet() { Name = valDelegate.Name };
                List<IValuePrimitive> primitives = new List<IValuePrimitive>();
                foreach (INdm ndm in ndms)
                {
                    double val = valDelegate.ResultFunction.Invoke(strainMatrix, ndm);
                    IValuePrimitive valuePrimitive;
                    if (ndm is IRectangleNdm)
                    {
                        var shapeNdm = ndm as IRectangleNdm;
                        valuePrimitive = new RectanglePrimitive() { CenterX = ndm.CenterX, CenterY = ndm.CenterY, Height = shapeNdm.Height, Width = shapeNdm.Width, Value = val };
                    }
                    else
                    {
                        valuePrimitive = new CirclePrimitive() { CenterX = ndm.CenterX, CenterY = ndm.CenterY, Diameter = Math.Sqrt(ndm.Area / Math.PI) * 2, Value = val };
                    }
                    primitives.Add(valuePrimitive);
                }
                primitiveSet.ValuePrimitives = primitives;
                primitiveSets.Add(primitiveSet);
            }
            return primitiveSets;
        }
    }
}
