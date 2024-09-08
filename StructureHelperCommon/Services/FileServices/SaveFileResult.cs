using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.FileServices
{
    public class SaveFileResult : IResult
    {
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
