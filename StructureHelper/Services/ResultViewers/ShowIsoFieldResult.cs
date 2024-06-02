using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.WindowsOperation;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.ResultData;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static List<IPrimitiveSet> GetPrimitiveSets(IStrainMatrix strainMatrix, IEnumerable<INdm> ndms, IEnumerable<ForceResultFunc> resultFuncs)
        {
            List<IPrimitiveSet> primitiveSets = new List<IPrimitiveSet>();
            foreach (var valDelegate in resultFuncs)
            {
                PrimitiveSet primitiveSet = new PrimitiveSet() { Name = valDelegate.Name };
                List<IValuePrimitive> primitives = new List<IValuePrimitive>();
                foreach (INdm ndm in ndms)
                {
                    primitives.Add(ProcessNdm(strainMatrix, valDelegate, ndm));
                }
                primitiveSet.ValuePrimitives = primitives;
                primitiveSets.Add(primitiveSet);
            }
            return primitiveSets;
        }

        public static List<IPrimitiveSet> GetPrimitiveSets(IEnumerable<RebarCrackResult> rebarResults, IEnumerable<CrackResultFunc> resultFuncs)
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

        private static IValuePrimitive ProcessNdm(CrackResultFunc valDelegate, RebarCrackResult rebarResult)
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

        private static IValuePrimitive ProcessNdm(IStrainMatrix strainMatrix, ForceResultFunc valDelegate, INdm ndm)
        {
            double delegateResult = valDelegate.ResultFunction.Invoke(strainMatrix, ndm);
            double val = delegateResult * valDelegate.UnitFactor;
            //val = roundLogic.RoundValue(val);
            IValuePrimitive valuePrimitive;
            if (ndm is IRectangleNdm shapeNdm)
            {
                valuePrimitive = ProcessRectangle(shapeNdm, val);
            }
            else
            {
                valuePrimitive = ProcessCircle(ndm, val);
            }
            return valuePrimitive;
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
