using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Analyses
{
    public interface IAnalysis : ISaveable, ICloneable
    {
        string Name { get; set; }
        string Tags { get; set; }
        IVersionProcessor VersionProcessor { get;}
    }
}
