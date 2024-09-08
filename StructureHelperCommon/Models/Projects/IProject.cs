using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Projects
{
    public interface IProject : ISaveable
    {
        string FullFileName { get; set; }
        string FileName { get; }
        bool IsNewFile { get; set; }
        bool IsActual { get; set; }
        List<IVisualAnalysis> VisualAnalyses { get;}
    }
}
