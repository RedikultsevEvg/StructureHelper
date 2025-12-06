using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.WindowsOperation;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;

namespace StructureHelper.Services.ResultViewers
{
    public static class ShowIsoFieldResult
    {
        static IMathRoundLogic roundLogic = new SmartRoundLogic() { DigitQuant = 3 };
        public static void ShowResult(IStrainMatrix strainMatrix, IEnumerable<INdm> ndms, IEnumerable<ForceResultFunc> resultFuncs)
        {
            var primitiveSets = GetPrimitiveSets(strainMatrix, ndms, resultFuncs);
            FieldViewerOperation.ShowViewer(primitiveSets);
        }

        public static List<IPrimitiveSet> GetPrimitiveSets(IStrainMatrix strainMatrix, IEnumerable<INdm> ndms, IEnumerable<ForceResultFunc> resultFuncs, bool convertRectangles = false)
        {
            List<IPrimitiveSet> primitiveSets = new List<IPrimitiveSet>();
            foreach (var valDelegate in resultFuncs)
            {
                PrimitiveSet primitiveSet = new PrimitiveSet() { Name = valDelegate.Name };
                List<IValuePrimitive> primitives = new List<IValuePrimitive>();
                foreach (INdm ndm in ndms)
                {
                    primitives.AddRange(ProcessNdm(strainMatrix, valDelegate, ndm, convertRectangles));
                }
                primitiveSet.ValuePrimitives = primitives;
                primitiveSets.Add(primitiveSet);
            }
            return primitiveSets;
        }

        public static List<IPrimitiveSet> GetPrimitiveSets(IEnumerable<IRebarCrackResult> rebarResults, IEnumerable<CrackResultFunc> resultFuncs)
        {
            List<IPrimitiveSet> primitiveSets = new List<IPrimitiveSet>();
            foreach (var valDelegate in resultFuncs)
            {
                PrimitiveSet primitiveSet = new PrimitiveSet() { Name = valDelegate.Name };
                List<IValuePrimitive> primitives = new List<IValuePrimitive>();
                foreach (var rebarResult in rebarResults)
                {
                    primitives.Add(ProcessNdm(valDelegate, rebarResult));
                }
                primitiveSet.ValuePrimitives = primitives;
                primitiveSets.Add(primitiveSet);
            }
            return primitiveSets;
        }

        private static IValuePrimitive ProcessNdm(CrackResultFunc valDelegate, IRebarCrackResult rebarResult)
        {
            double delegateResult = valDelegate.ResultFunction.Invoke(rebarResult);
            var val = delegateResult * valDelegate.UnitFactor;
            //val = roundLogic.RoundValue(val);
            IValuePrimitive valuePrimitive;
            var rebarNdm = rebarResult.RebarPrimitive.GetRebarNdm(new TriangulationOptions()
            {
                LimiteState = LimitStates.SLS,
                CalcTerm = CalcTerms.ShortTerm
            }
            );
            valuePrimitive = ProcessCircle(rebarNdm, val);
            return valuePrimitive;
        }

        private static List<IValuePrimitive> ProcessNdm(IStrainMatrix strainMatrix, ForceResultFunc valDelegate, INdm ndm, bool convertRectangles)
        {
            List<IValuePrimitive> valuePrimitives = [];
            double delegateResult = valDelegate.ResultFunction.Invoke(strainMatrix, ndm);
            double val = delegateResult * valDelegate.UnitFactor;
            //val = roundLogic.RoundValue(val);
            IValuePrimitive valuePrimitive;
            if (ndm is IRectangleNdm shapeNdm)
            {
                if (convertRectangles)
                {
                    valuePrimitives.AddRange(ProcessRectangleToTriangles(shapeNdm, strainMatrix, valDelegate));
                }
                else
                {
                    valuePrimitive = ProcessRectangle(shapeNdm, val);
                    valuePrimitives.Add(valuePrimitive);
                }
            }
            else if (ndm is ITriangleNdm triangle)
            {
                //valuePrimitive = ProcessTriangle(triangle, val);
                valuePrimitive = ProcessTriangle(strainMatrix, valDelegate, triangle);
                valuePrimitives.Add(valuePrimitive);
            }
            else
            {
                valuePrimitive = ProcessCircle(ndm, val);
                valuePrimitives.Add(valuePrimitive);
            }
            return valuePrimitives;
        }

        private static IValuePrimitive ProcessTriangle(IStrainMatrix strainMatrix, ForceResultFunc valDelegate, ITriangleNdm triangle)
        {
            double delegateResult = valDelegate.ResultFunction.Invoke(strainMatrix, triangle);
            double val = delegateResult * valDelegate.UnitFactor;
            var moqLogic = new GetMoqNdmLogic();
            var moqNdm1 = moqLogic.GetMockNdm(triangle, new Point2D(triangle.Point1.X, triangle.Point1.Y));
            var moqNdm2 = moqLogic.GetMockNdm(triangle, new Point2D(triangle.Point2.X, triangle.Point2.Y));
            var moqNdm3 = moqLogic.GetMockNdm(triangle, new Point2D(triangle.Point3.X, triangle.Point3.Y));
            var primitive = new TrianglePrimitive()
            {
                Point1 = new Point2D() { X = triangle.Point1.X, Y = triangle.Point1.Y },
                Point2 = new Point2D() { X = triangle.Point2.X, Y = triangle.Point2.Y },
                Point3 = new Point2D() { X = triangle.Point3.X, Y = triangle.Point3.Y },
                Value = val,
                ValuePoint1 = valDelegate.ResultFunction.Invoke(strainMatrix, moqNdm1) * valDelegate.UnitFactor,
                ValuePoint2 = valDelegate.ResultFunction.Invoke(strainMatrix, moqNdm2) * valDelegate.UnitFactor,
                ValuePoint3 = valDelegate.ResultFunction.Invoke(strainMatrix, moqNdm3) * valDelegate.UnitFactor,
            };
            return primitive;
        }


        //private static IValuePrimitive ProcessTriangle(ITriangleNdm triangle, double val)
        //{
        //    var primitive = new TrianglePrimitive()
        //    {
        //        Point1 = new Point2D() { X = triangle.Point1.X, Y = triangle.Point1.Y },
        //        Point2 = new Point2D() { X = triangle.Point2.X, Y = triangle.Point2.Y },
        //        Point3 = new Point2D() { X = triangle.Point3.X, Y = triangle.Point3.Y },
        //        Value = val
        //    };
        //    return primitive;
        //}

        private static List<IValuePrimitive> ProcessRectangleToTriangles(IRectangleNdm shapeNdm, IStrainMatrix strainMatrix, ForceResultFunc valDelegate)
        {
            List<INdm> triangles = NdmTransform.ConvertRectangleToTriangleNdm(shapeNdm);
            List<IValuePrimitive> valuePrimitives = [];
            foreach (var item in triangles)
            {
                double delegateResult = valDelegate.ResultFunction.Invoke(strainMatrix, item);
                double val = delegateResult * valDelegate.UnitFactor;
                valuePrimitives.Add(ProcessTriangle(strainMatrix, valDelegate, (ITriangleNdm)item));
            }
            return valuePrimitives;
        }

        private static IValuePrimitive ProcessRectangle(IRectangleNdm shapeNdm, double val)
        {
            return new RectanglePrimitive()
            {
                CenterX = shapeNdm.CenterX,
                CenterY = shapeNdm.CenterY,
                Height = shapeNdm.Height,
                Width = shapeNdm.Width,
                Value = val
            };
        }

        private static IValuePrimitive ProcessCircle(INdm ndm, double val)
        {
            return new CirclePrimitive()
            {
                CenterX = ndm.CenterX,
                CenterY = ndm.CenterY,
                Diameter = Math.Sqrt(ndm.Area / Math.PI) * 2,
                Value = val
            };
        }
    }
}
