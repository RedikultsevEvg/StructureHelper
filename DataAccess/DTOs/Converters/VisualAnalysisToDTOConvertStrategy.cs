using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    internal class VisualAnalysisToDTOConvertStrategy : IConvertStrategy<VisualAnalysisDTO, IVisualAnalysis>
    {
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public VisualAnalysisDTO Convert(IVisualAnalysis source)
        {
            VisualAnalysisDTO visualAnalysisDTO = new()
            {
                Id = source.Id
            };
            return visualAnalysisDTO;
        }
    }
}
