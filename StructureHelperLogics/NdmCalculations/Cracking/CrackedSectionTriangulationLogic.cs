using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <inheritdoc/>
    public class CrackedSectionTriangulationLogic : ICrackedSectionTriangulationLogic
    {
        const LimitStates limitState = LimitStates.SLS;

        private ITriangulatePrimitiveLogic triangulateLogic;
        private string ndmPrimitiveCountMessage;
        public CalcTerms CalcTerm { get; set; }
        public IEnumerable<INdmPrimitive> NdmPrimitives { get; private set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public CrackedSectionTriangulationLogic(IEnumerable<INdmPrimitive> ndmPrimitives, CalcTerms calcTerm)
        {
            NdmPrimitives = ndmPrimitives;
            CalcTerm = calcTerm;
            ndmPrimitiveCountMessage = $"Source collection containes {NdmPrimitives.Count()} primitives";
            triangulateLogic = new TriangulatePrimitiveLogic
            {
                Primitives = NdmPrimitives,
                LimitState = limitState,
                CalcTerm = CalcTerm,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
        }

        /// <inheritdoc/>
        public List<INdm> GetNdmCollection()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(ndmPrimitiveCountMessage, TraceLogStatuses.Debug);
            triangulateLogic = new TriangulatePrimitiveLogic()
            {
                LimitState = limitState,
                CalcTerm = CalcTerm,
                Primitives = NdmPrimitives,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            return triangulateLogic.GetNdms();
        }
        /// <inheritdoc/>
        public List<INdm> GetCrackedNdmCollection()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(ndmPrimitiveCountMessage, TraceLogStatuses.Debug);
            triangulateLogic = new TriangulatePrimitiveLogic(new MeshCrackedConcreteLogic())
            {
                LimitState = limitState,
                CalcTerm = CalcTerm,
                Primitives = NdmPrimitives,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            return triangulateLogic.GetNdms();
        }

        /// <inheritdoc/>
        public List<IRebarNdmPrimitive> GetRebarPrimitives()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage(ndmPrimitiveCountMessage, TraceLogStatuses.Debug);
            List<IRebarNdmPrimitive> rebarPrimitives = new();
            foreach (var item in NdmPrimitives)
            {
                if (item is IRebarNdmPrimitive rebar)
                {
                    TraceLogger?.AddMessage($"Primitive {rebar.Name} is rebar primitive", TraceLogStatuses.Service);
                    rebarPrimitives.Add(rebar);
                }
            }
            TraceLogger?.AddMessage($"Obtained {rebarPrimitives.Count} rebar primitives");
            return rebarPrimitives;
        }

        /// <inheritdoc/>
        public List<INdm> GetElasticNdmCollection()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage(ndmPrimitiveCountMessage, TraceLogStatuses.Debug);
            triangulateLogic = new TriangulatePrimitiveLogic(new MeshElasticLogic())
            {
                LimitState = limitState,
                CalcTerm = CalcTerm,
                Primitives = NdmPrimitives,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            return triangulateLogic.GetNdms();
        }
    }
}
