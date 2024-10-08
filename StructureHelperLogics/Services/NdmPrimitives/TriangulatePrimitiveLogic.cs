using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public class TriangulatePrimitiveLogic : ITriangulatePrimitiveLogic
    {
        private IMeshHasDivisionLogic divisionLogic;
        private IMeshPrimitiveLogic meshLogic;
        private List<INdm> ndmCollection;
        private ICheckPrimitivesForMeshingLogic checkLogic;

        public IEnumerable<INdmPrimitive> Primitives { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public TriangulatePrimitiveLogic(IMeshPrimitiveLogic meshPrimitiveLogic)
        {
            meshLogic = meshPrimitiveLogic;
        }

        public TriangulatePrimitiveLogic() : this (new MeshPrimitiveLogic()) { }

        public List<INdm> GetNdms()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            CheckPrimitives();
            ndmCollection = new List<INdm>();
            SetLogics();
            TraceInitialSettings();
            TriangulatePrimitives();
            return ndmCollection;
        }

        private void TriangulatePrimitives()
        {
            var orderedNdmPrimitives = Primitives.OrderBy(x => x.VisualProperty.ZIndex);
            foreach (var primitive in orderedNdmPrimitives)
            {
                TriangulatePrimitive(primitive);
            }
            if (TraceLogger is not null)
            {
                TraceService.TraceNdmCollection(TraceLogger, ndmCollection);
            }
            TraceLogger?.AddMessage($"Triangulation of primitives has finished, {ndmCollection.Count} part(s) were obtained", TraceLogStatuses.Service);
        }

        private void TraceInitialSettings()
        {
            TraceLogger?.AddMessage($"Total count of primitives n = {Primitives.Count()}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Limit state is {LimitState}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Calc term  is {CalcTerm}", TraceLogStatuses.Service);
        }

        private void SetLogics()
        {
            divisionLogic = new MeshHasDivisionLogic()
            {
                NdmCollection = ndmCollection,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            var triangulationOptions = new TriangulationOptions()
            {
                LimiteState = LimitState,
                CalcTerm = CalcTerm
            };
            meshLogic.TriangulationOptions = triangulationOptions;
            meshLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
        }

        private void TriangulatePrimitive(INdmPrimitive primitive)
        {
            TraceLogger?.AddMessage($"Triangulation of primitive {primitive.Name} has started", TraceLogStatuses.Service);
            if (primitive is IHasDivisionSize hasDivision)
            {
                divisionLogic.Primitive = hasDivision;
                divisionLogic.MeshHasDivision();
            }
            if (primitive.NdmElement.Triangulate == true)
            {
                meshLogic.Primitive = primitive;
                var ndms = meshLogic.MeshPrimitive();
                ndmCollection.AddRange(ndms);
            }
            else
            {
                TraceLogger?.AddMessage($"Triangulation of primitive was skiped cause its settings", TraceLogStatuses.Service);
            }
        }

        private bool CheckPrimitives()
        {
            checkLogic = new CheckPrimitivesForMeshingLogic()
            {
                Primitives = Primitives,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            checkLogic.Check();
            return true;
        }
    }
}
