using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface ILongProcessLogic : ILogic
    {
        int StepCount { get; }
        Action<int> SetProgress { get; set; }
        bool Result { get; set; }

        void WorkerDoWork(object sender, DoWorkEventArgs e);
        void WorkerProgressChanged(object sender, ProgressChangedEventArgs e);
        void WorkerRunWorkCompleted(object sender, RunWorkerCompletedEventArgs e);
    }
}
