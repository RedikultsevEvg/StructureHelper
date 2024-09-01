using StructureHelperCommon.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.MainWindow.Analyses
{
    public interface IVisualAnalysis
    {
        IAnalysis Analysis {get;set;}
        void Run();
    }
}
