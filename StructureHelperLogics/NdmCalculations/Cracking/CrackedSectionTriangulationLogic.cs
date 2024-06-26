﻿using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackedSectionTriangulationLogic : ICrackedSectionTriangulationLogic
    {
        const LimitStates limitState = LimitStates.SLS;
        const CalcTerms shortTerm = CalcTerms.ShortTerm;

        private ITriangulatePrimitiveLogic triangulateLogic;
        private string ndmPrimitiveCountMessage;

        public IEnumerable<INdmPrimitive> NdmPrimitives { get; private set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public CrackedSectionTriangulationLogic(IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            NdmPrimitives = ndmPrimitives;
            ndmPrimitiveCountMessage = $"Source collection containes {NdmPrimitives.Count()} primitives";
            triangulateLogic = new TriangulatePrimitiveLogic
            {
                Primitives = NdmPrimitives,
                LimitState = limitState,
                CalcTerm = shortTerm,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
        }
        public List<INdm> GetNdmCollection()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(ndmPrimitiveCountMessage, TraceLogStatuses.Debug);
            triangulateLogic = new TriangulatePrimitiveLogic()
            {
                LimitState = limitState,
                CalcTerm = shortTerm,
                Primitives = NdmPrimitives,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            return triangulateLogic.GetNdms();
        }
        public List<INdm> GetCrackedNdmCollection()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(ndmPrimitiveCountMessage, TraceLogStatuses.Debug);
            triangulateLogic = new TriangulatePrimitiveLogic(new MeshCrackedConcreteLogic())
            {
                LimitState = limitState,
                CalcTerm = shortTerm,
                Primitives = NdmPrimitives,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            return triangulateLogic.GetNdms();
        }

        public List<RebarPrimitive> GetRebarPrimitives()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(ndmPrimitiveCountMessage, TraceLogStatuses.Debug);
            List<RebarPrimitive> rebarPrimitives = new();
            foreach (var item in NdmPrimitives)
            {
                if (item is RebarPrimitive rebar)
                {
                    TraceLogger?.AddMessage($"Primitive {rebar.Name} is rebar primitive", TraceLogStatuses.Service);
                    rebarPrimitives.Add(rebar);
                }
            }
            TraceLogger?.AddMessage($"Obtained {rebarPrimitives.Count} rebar primitives");
            return rebarPrimitives;
        }

        public List<INdm> GetElasticNdmCollection()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(ndmPrimitiveCountMessage, TraceLogStatuses.Debug);
            triangulateLogic = new TriangulatePrimitiveLogic(new MeshElasticLogic())
            {
                LimitState = limitState,
                CalcTerm = shortTerm,
                Primitives = NdmPrimitives,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            return triangulateLogic.GetNdms();
        }
    }
}
