using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class ShowDiagramLogic : ILongProcessLogic
    {
        private ArrayParameter<double> arrayParameter;
        private IEnumerable<IExtendedForceTupleCalculatorResult> tupleList;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private List<IExtendedForceTupleCalculatorResult> validTupleList;

        public int StepCount => validTupleList.Count();

        public Action<int> SetProgress { get; set; }
        public bool Result { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            Show();
            Result = true;
        }

        public void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //nothing to do
        }

        public void WorkerRunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //nothing to do
        }
        public void ShowWindow()
        {
            SafetyProcessor.RunSafeProcess(() =>
            {
                var series = new Series(arrayParameter) { Name = "Forces and curvatures" };
                var vm = new GraphViewModel(new List<Series>() { series });
                var wnd = new GraphView(vm);
                wnd.ShowDialog();
            }, ErrorStrings.ErrorDuring("building chart"));
        }
        private void Show()
        {
            validTupleList = tupleList.Where(x => x.IsValid == true).ToList();

            var factory = new DiagramFactory()
            {
                TupleResultList = validTupleList,
                //SetProgress = SetProgress,
            };
            arrayParameter = factory.GetCommonArray();
        }

        public ShowDiagramLogic(IEnumerable<IExtendedForceTupleCalculatorResult> tupleList, IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            this.tupleList = tupleList;
            this.ndmPrimitives = ndmPrimitives;
            validTupleList = tupleList.Where(x => x.IsValid == true).ToList();
        }
    }
}
