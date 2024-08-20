using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.MainWindow
{
    public class FileLogic : ViewModelBase
    {
        private ICommand fileOpen;
        private ICommand fileSave;

        public ICommand FileOpen => fileOpen;
        public ICommand FileSave => fileSave;
    }
}
