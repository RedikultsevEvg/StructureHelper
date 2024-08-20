using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.MainWindow
{
    public class AnalisesManagerViewModel : ViewModelBase
    {
        public FileLogic FileLogic { get; }
        public DiagramLogic DiagramLogic { get; }
        public AnalisesLogic AnalisesLogic { get; }

        public AnalisesManagerViewModel()
        {
            FileLogic = new();
            DiagramLogic = new();
            AnalisesLogic = new();
        }
    }
}
