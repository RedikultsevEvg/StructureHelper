using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.Exports
{
    internal class ExportToFileInputData : IExportToFileInputData
    {
        public string FileName { get; set; }
        public string Filter { get; set; }
        public string Title { get; set; }
    }
}
