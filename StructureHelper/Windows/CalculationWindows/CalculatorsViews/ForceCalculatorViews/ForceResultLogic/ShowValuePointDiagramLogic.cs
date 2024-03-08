using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class ShowValuePointDiagramLogic : ILongProcessLogic
    {
        private ArrayParameter<double> arrayParameter;
        private IEnumerable<IForcesTupleResult> tupleList;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private List<IForcesTupleResult> validTupleList;
        private List<(PrimitiveBase PrimitiveBase, List<NamedValue<IPoint2D>>)> valuePoints;
        private List<IResultFunc> resultFuncList;

        public ForceCalculator Calculator { get; set; }
        public PointPrimitiveLogic PrimitiveLogic { get; set; }
        public ValueDelegatesLogic ValueDelegatesLogic { get; set; }

        public int StepCount => throw new NotImplementedException();

        public Action<int> SetProgress { get; set; }
        public bool Result { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public ShowValuePointDiagramLogic(IEnumerable<IForcesTupleResult> tupleList, IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            this.tupleList = tupleList;
            this.ndmPrimitives = ndmPrimitives;
            validTupleList = this.tupleList.Where(x => x.IsValid == true).ToList();
            valuePoints = new List<(PrimitiveBase PrimitiveBase, List<NamedValue<IPoint2D>>)>();
            foreach (var item in PrimitiveLogic.Collection.CollectionItems) 
            {
                var pointsCount = item.Item.ValuePoints.SelectedCount;
                if (pointsCount > 0)
                {
                    var points = item.Item.ValuePoints.SelectedItems.ToList();
                    var primitive = item.Item.PrimitiveBase;
                    valuePoints.Add((primitive, points));
                }
            }
        }
        public void ShowWindow()
        {
            SafetyProcessor.RunSafeProcess(() =>
            {
                var series = new Series(arrayParameter)
                {
                    Name = "Forces and curvatures"
                };
                var vm = new GraphViewModel(new List<Series>()
                {
                    series
                });
                var wnd = new GraphView(vm);
                wnd.ShowDialog();
            }, ErrorStrings.ErrorDuring("building chart"));
        }

        public void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            Show();
            Result = true;
        }

        public void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Nothing to do
        }

        public void WorkerRunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Nothing to do
        }

        private void Show()
        {

        }

        private List<string> GetColumnNames()
        {
            var columnNames = LabelsFactory.GetCommonLabels();
            
            return columnNames;
        }
    }
}
