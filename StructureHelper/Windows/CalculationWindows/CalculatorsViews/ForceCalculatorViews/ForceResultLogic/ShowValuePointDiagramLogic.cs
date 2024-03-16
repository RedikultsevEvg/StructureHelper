using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
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
        private List<(PrimitiveBase PrimitiveBase, List<INamedAreaPoint> namedPoints)> valuePoints;
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
            valuePoints = new List<(PrimitiveBase PrimitiveBase, List<INamedAreaPoint>)>();
        }

        public void SetParameters()
        {
            var factory = new DiagramFactory()
            {
                TupleList = validTupleList,
                SetProgress = SetProgress,
            };
            arrayParameter = factory.GetCommonArray();

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

            var selectedDelegates = ValueDelegatesLogic.ResultFuncs.SelectedItems;
            if (selectedDelegates.Count() == 0) { return; }
            var labels = GetLabels(valuePoints, selectedDelegates);
            var pointCount = valuePoints.Sum(x => x.namedPoints.Count());
            List<double> values = new();
            var tuplesList = tupleList
                .Where(x => x.IsValid == true)
                .ToList();
            var newArray = new ArrayParameter<double>(tuplesList.Count(), labels);
            for (int i = 0; i < tuplesList.Count(); i++)
            {
                var strainMatrix = tuplesList[i].LoaderResults.ForceStrainPair.StrainMatrix;
                values.Clear();
                foreach (var valuePoint in valuePoints)
                {
                    foreach (var point in valuePoint.namedPoints)
                    {
                        var limitState = tuplesList[i].DesignForceTuple.LimitState;
                        var calcTerm = tuplesList[i].DesignForceTuple.CalcTerm;
                        var ndm = GetNdm(valuePoint, point, limitState, calcTerm);

                        foreach (var valDelegate in selectedDelegates)
                        {

                            double val = valDelegate.ResultFunction.Invoke(strainMatrix, ndm) * valDelegate.UnitFactor;
                            values.Add(val);
                        }
                    }
                }
                newArray.AddRow(i, values);
            }
            arrayParameter.AddArray(newArray);
        }

        private List<string> GetLabels(List<(PrimitiveBase PrimitiveBase, List<INamedAreaPoint> namedPoints)> valuePoints, IEnumerable<IResultFunc> selectedDelegates)
        {
            List<string> strings = new();
            foreach (var valuePoint in valuePoints)
            {
                foreach (var item in valuePoint.namedPoints)
                {
                    foreach (var deleg in selectedDelegates)
                    {
                        string s = valuePoint.PrimitiveBase.Name;
                        s += "_" + item.Name;
                        s += "_" + deleg.Name + ", " + deleg.UnitName;
                        strings.Add(s);
                    }
                }
            }
            return strings;
        }

        private static RebarNdm GetNdm((PrimitiveBase PrimitiveBase, List<INamedAreaPoint> namedPoints) valuePoint, INamedAreaPoint point, LimitStates limitState, CalcTerms calcTerm)
        {
            var ndmPrimitive = valuePoint.PrimitiveBase.GetNdmPrimitive();
            var material = ndmPrimitive.HeadMaterial.GetLoaderMaterial(limitState, calcTerm);
            var userPrestrain = ndmPrimitive.UsersPrestrain;
            var autoPrestrain = ndmPrimitive.AutoPrestrain;
            var ndm = new RebarNdm()
            {
                Area = point.Area,
                CenterX = point.Point.X,
                CenterY = point.Point.Y,
                Material = material,
            };
            ndm.Prestrain = (userPrestrain.Mx + autoPrestrain.Mx) * point.Point.Y
                + (userPrestrain.My + autoPrestrain.My) * point.Point.X
                + userPrestrain.Nz + autoPrestrain.Nz;
            return ndm;
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
            validTupleList = tupleList.Where(x => x.IsValid == true).ToList();

            var factory = new DiagramFactory()
            {
                TupleList = validTupleList,
                SetProgress = SetProgress,
            };
            arrayParameter = factory.GetCommonArray();
        }

        private List<string> GetColumnNames()
        {
            var columnNames = LabelsFactory.GetCommonLabels();
            
            return columnNames;
        }
    }
}
