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
    public class VisualAnaysisFromDTOConvertStrategy : IConvertStrategy<IVisualAnalysis, IVisualAnalysis>
    {
        private IConvertStrategy<IAnalysis, IAnalysis> analysisConvertStrategy = new AnalysisFromDTOConvertStrategy();
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public IVisualAnalysis Convert(IVisualAnalysis source)
        {
            Check();
            try
            {
                VisualAnalysis newItem = GetAnalysis(source);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private VisualAnalysis GetAnalysis(IVisualAnalysis source)
        {
            analysisConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            analysisConvertStrategy.TraceLogger = TraceLogger;
            IAnalysis analysis = analysisConvertStrategy.Convert(source.Analysis);
            VisualAnalysis newItem = new(source.Id, analysis);
            TraceLogger?.AddMessage($"Visual Analysis was obtained succesfully", TraceLogStatuses.Debug);
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<IVisualAnalysis, IVisualAnalysis>(this);
            checkLogic.Check();
        }
    }
}
