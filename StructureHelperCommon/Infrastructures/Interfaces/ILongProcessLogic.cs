using System;
using System.ComponentModel;

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
