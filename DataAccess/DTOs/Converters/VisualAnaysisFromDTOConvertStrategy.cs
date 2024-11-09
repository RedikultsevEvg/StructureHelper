using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogic.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class VisualAnaysisFromDTOConvertStrategy : ConvertStrategy<IVisualAnalysis, IVisualAnalysis>
    {
        private IConvertStrategy<IAnalysis, IAnalysis> analysisConvertStrategy;

        public VisualAnaysisFromDTOConvertStrategy(IConvertStrategy<IAnalysis, IAnalysis> analysisConvertStrategy,
            Dictionary<(Guid id, Type type), ISaveable> refDictinary,
            IShiftTraceLogger? traceLogger)
        {
            this.analysisConvertStrategy = analysisConvertStrategy;
            ReferenceDictionary = refDictinary;
            TraceLogger = traceLogger;
        }

        public VisualAnaysisFromDTOConvertStrategy(
            Dictionary<(Guid id, Type type), ISaveable> refDictinary,
            IShiftTraceLogger? traceLogger)
             : this(
                   new AnalysisFromDTOConvertStrategy(),
                   refDictinary,
                   traceLogger)
        {    }

        public override VisualAnalysis GetNewItem(IVisualAnalysis source)
        {
            TraceLogger?.AddMessage($"Visual Analysis Name = {source.Analysis.Name} converting is started");
            analysisConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            analysisConvertStrategy.TraceLogger = TraceLogger;
            IAnalysis analysis = analysisConvertStrategy.Convert(source.Analysis);
            VisualAnalysis newItem = new(source.Id, analysis);
            TraceLogger?.AddMessage($"Visual Analysis Name = {newItem.Analysis.Name} was obtained successfully");
            return newItem;
        }
    }
}
