using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.MainWindow
{
    public class AnalysesManagerViewModel : ViewModelBase
    {
        public FileLogic FileLogic { get; }
        public DiagramLogic DiagramLogic { get; }
        public AnalysesLogic AnalysesLogic { get; }

        public AnalysesManagerViewModel()
        {
            FileLogic = new();
            DiagramLogic = new();
            AnalysesLogic = new();
        }
    }
}
