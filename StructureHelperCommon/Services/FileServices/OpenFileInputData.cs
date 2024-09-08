using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.FileServices
{
    public class OpenFileInputData : IInputData
    {
        public IShiftTraceLogger? TraceLogger { get; set; }
        public string FilterString { get; set; } = string.Empty;
        public int FilterIndex { get; set; } = 1;
        public bool MultiSelect { get; set; } = false;
        public string Title { get; set; } = "Select a file";
    }
}
