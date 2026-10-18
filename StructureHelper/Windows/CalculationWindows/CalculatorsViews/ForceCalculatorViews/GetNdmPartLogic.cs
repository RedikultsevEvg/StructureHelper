using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class GetNdmPartLogic
    {
        public List<INdmPrimitive> Primitives { get; set; }
        public List<INdmPrimitive> SelectedPrimitives { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }

        public List<INdm> GetNdms()
        {
            var triangulationOptions = new TriangulationOptions()
            {
                LimiteState = LimitState,
                CalcTerm = CalcTerm
            };
            var orderedNdmPrimitives = Primitives
                .OrderBy(x => x.VisualProperty.ZIndex);
            var ndmRange = new List<INdm>();
            foreach (var item in orderedNdmPrimitives)
            {
                if (item is IHasDivisionSize hasDivision)
                {
                    if (hasDivision.DivisionSize.ClearUnderlying == true)
                    {
                        ndmRange.RemoveAll(x =>
                        hasDivision
                        .IsPointInside(new Point2D()
                        {
                            X = x.CenterX,
                            Y = x.CenterY
                        }
                        ) == true);
                    }
                }
                if (SelectedPrimitives.Contains(item) & item.NdmElement.Triangulate == true)
                {
                    ndmRange.AddRange(item.GetNdms(triangulationOptions));
                }
            }
            return ndmRange;
        }
    }
}
