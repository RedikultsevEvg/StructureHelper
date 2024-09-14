using DataAccess.DTOs;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.JsonConverters
{
    internal class VisualAnalysisDTOJsonConverter : BaseJsonConverter<VisualAnalysisDTO>
    {
        public VisualAnalysisDTOJsonConverter(IShiftTraceLogger logger) : base(logger)
        {
        }
    }
}
