using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Codes
{
    public interface ICodeEntity : ISaveable
    {
        NatSystems NatSystem { get; }
        string Name { get; set; }
        string FullName { get; set; }

    }
}
