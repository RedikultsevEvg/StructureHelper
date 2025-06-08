using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    internal class VisualAnalysisToDTOConvertStrategy : ConvertStrategy<VisualAnalysisDTO, IVisualAnalysis>
    {
        private IConvertStrategy<IAnalysis, IAnalysis> convertLogic;

        public override VisualAnalysisDTO GetNewItem(IVisualAnalysis source)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage($"Visual analysis Id = {source.Id} converting has been started");
            VisualAnalysisDTO visualAnalysisDTO = GetNewAnalysis(source);
            TraceLogger?.AddMessage($"Visual analysis Id = {visualAnalysisDTO.Id} converting has been finished successfully");
            return visualAnalysisDTO;
        }

        public VisualAnalysisToDTOConvertStrategy
            (Dictionary<(Guid id, Type type), ISaveable> referenceDictionary,
            IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public VisualAnalysisToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private VisualAnalysisDTO GetNewAnalysis(IVisualAnalysis source)
        {
            InitializeStrategies();
            VisualAnalysisDTO visualAnalysisDTO = new(source.Id)
            {
                Analysis = convertLogic.Convert(source.Analysis)
            };
            return visualAnalysisDTO;
        }

        private void InitializeStrategies()
        {
            convertLogic ??= new DictionaryConvertStrategy<IAnalysis, IAnalysis>
                (this,
                new AnalysisToDTOConvertStrategy(ReferenceDictionary, TraceLogger)
                );
        }
    }
}
