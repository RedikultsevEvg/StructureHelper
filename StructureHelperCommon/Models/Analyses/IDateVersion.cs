using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Analyses
{
    public interface IDateVersion
    {
        DateTime DateTime { get; set; }
        ISaveable Item { get; set; }
    }
}
