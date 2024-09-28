using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Projects;
using StructureHelperLogic.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    internal class VisualAnalysisToDTOConvertStrategy : IConvertStrategy<VisualAnalysisDTO, IVisualAnalysis>
    {
        private IConvertStrategy<IAnalysis, IAnalysis> convertStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public VisualAnalysisToDTOConvertStrategy(IConvertStrategy<IAnalysis, IAnalysis> convertStrategy)
        {
            this.convertStrategy = convertStrategy;
        }

        public VisualAnalysisToDTOConvertStrategy() : this(new AnalysisToDTOConvertStrategy())
        {
            
        }

        public VisualAnalysisDTO Convert(IVisualAnalysis source)
        {
            Check();
            VisualAnalysisDTO visualAnalysisDTO = new()
            {
                Id = source.Id
            };
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<IAnalysis, IAnalysis>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = convertStrategy,
                TraceLogger = TraceLogger
            };
            visualAnalysisDTO.Analysis = convertLogic.Convert(source.Analysis);
            return visualAnalysisDTO;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<VisualAnalysisDTO, IVisualAnalysis>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
