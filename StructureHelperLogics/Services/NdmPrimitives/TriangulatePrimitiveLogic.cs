using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public class TriangulatePrimitiveLogic : ITriangulatePrimitiveLogic
    {
        private List<INdm> ndmCollection;

        public IEnumerable<INdmPrimitive> Primitives { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public bool ConsiderCracking { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public TriangulatePrimitiveLogic()
        {
            ConsiderCracking = false;
        }
        public List<INdm> GetNdms()
        {
            TraceLogger?.AddMessage($"Total count of primitives n = {Primitives.Count()}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Limit state is {LimitState}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Calc term  is {CalcTerm}", TraceLogStatuses.Service);
            var orderedNdmPrimitives = Primitives.OrderBy(x => x.VisualProperty.ZIndex);
            ndmCollection = new List<INdm>();
            foreach (var item in orderedNdmPrimitives)
            {
                TraceLogger?.AddMessage($"Triangulation of primitive {item.Name} has started", TraceLogStatuses.Service);
                ProcessHasDivision(item);
                TriangulatePrimitive(item);
            }
            if (TraceLogger is not null)
            {
                TraceService.TraceNdmCollection(TraceLogger, ndmCollection);
            }
            return ndmCollection;
        }

        private void TriangulatePrimitive(INdmPrimitive item)
        {
            if (item.Triangulate == true)
            {
                var triangulationOptions = new TriangulationOptions()
                {
                    LimiteState = LimitState,
                    CalcTerm = CalcTerm
                };
                var itemNdms = item.GetNdms(triangulationOptions);
                ndmCollection.AddRange(itemNdms);
                TraceLogger?.AddMessage($"Triangulation of primitive {item.Name} was finished, {itemNdms.Count()} part(s) were obtained", TraceLogStatuses.Service);
            }
            else
            {
                TraceLogger?.AddMessage($"Triangulation of primitive was skiped cause its settings", TraceLogStatuses.Service);
            }
        }

        private void ProcessHasDivision(INdmPrimitive? item)
        {
            if (item is IHasDivisionSize hasDivision)
            {
                if (hasDivision.ClearUnderlying == true)
                {
                    TraceLogger?.AddMessage("Removing of background part has started", TraceLogStatuses.Service);
                    ndmCollection
                        .RemoveAll(x =>
                        hasDivision
                            .IsPointInside(new Point2D() { X = x.CenterX, Y = x.CenterY }) == true);
                }
            }
        }

        public bool CheckPrimitives(IEnumerable<INdmPrimitive> primitives)
        {
            if (!primitives.Any())
            {
                string errorMessage = string.Intern(ErrorStrings.DataIsInCorrect + $": Count of primitive must be greater than zero");
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
            if (!primitives.Any(x => x.Triangulate == true))
            {
                string errorMessage = string.Intern(ErrorStrings.DataIsInCorrect + $": There are not primitives to triangulate");
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
            foreach (var item in primitives)
            {
                if (item.Triangulate == true &
                    item.HeadMaterial is null)
                {
                    string errorMessage = string.Intern(ErrorStrings.DataIsInCorrect + $": Primitive: {item.Name} can't be triangulated since material is null");
                    TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                    throw new StructureHelperException(errorMessage);
                }
            }
            return true;
        }
    }
}
