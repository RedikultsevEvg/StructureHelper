using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public static class NdmPrimitivesService
    {
        public static List<INdm> GetNdms(IEnumerable<INdmPrimitive> primitives, LimitStates limitState, CalcTerms calcTerm)
        {
            var orderedNdmPrimitives = primitives.OrderBy(x => x.VisualProperty.ZIndex);
            var ndms = new List<INdm>();
            var triangulationOptions = new TriangulationOptions() { LimiteState = limitState, CalcTerm = calcTerm };
            foreach (var item in orderedNdmPrimitives)
            {
                if (item is IHasDivisionSize)
                {
                    var hasDivision = item as IHasDivisionSize;
                    if (hasDivision.ClearUnderlying == true)
                    {
                        ndms.RemoveAll(x => hasDivision.IsPointInside(new Point2D() { X = x.CenterX, Y = x.CenterY }) == true);
                    }
                }
                if (item.Triangulate == true)
                {
                    ndms.AddRange(item.GetNdms(triangulationOptions));
                }
            }
            return ndms;
        }

        public static bool CheckPrimitives(IEnumerable<INdmPrimitive> primitives)
        {
            if (!primitives.Any())
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Count of primitive must be greater than zero");
            }
            if (!primitives.Any(x => x.Triangulate == true))
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": There are not primitives to triangulate");
            }
            foreach (var item in primitives)
            {
                if (item.Triangulate == true &
                    item.HeadMaterial is null)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Primitive: {item.Name} can't be triangulated since material is null");
                }
            } 
            return true;
        }
    }
}
