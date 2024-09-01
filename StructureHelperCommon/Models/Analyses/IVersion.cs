using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Analyses
{
    public interface IVersion
    {
        DateTime DateTime { get; }
        ISaveable Item { get; set; }
    }
}
