using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Exports
{
    public interface IImportFromFileLogic : IImportLogic
    {
        string? FileName { get; set; }
    }
}
