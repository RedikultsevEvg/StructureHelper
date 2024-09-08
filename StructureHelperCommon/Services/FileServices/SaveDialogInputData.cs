using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.FileServices
{
    public class SaveDialogInputData : IInputData
    {
        public string InitialDirectory { get; set; }
        public int FilterIndex { get; set; } = 1;
        public string FilterString { get; set; } = string.Empty;
        public bool CheckFileExist { get; set; } = false;
    }
}
