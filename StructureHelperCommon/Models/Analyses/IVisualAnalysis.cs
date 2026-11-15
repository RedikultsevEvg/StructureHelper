using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Analyses
{
    public interface IVisualAnalysis : ISaveable, ICloneStrategy<IVisualAnalysis>
    {
        IProject Project { get; }
        IAnalysis Analysis { get; set; }
        Action ActionToRun { get; set; }
        void Run();
    }
}
