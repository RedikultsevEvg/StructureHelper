using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.FileServices
{
    public class OpenFileResult : IResult
    {
        public bool IsValid { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; } = string.Empty;
    }
}
