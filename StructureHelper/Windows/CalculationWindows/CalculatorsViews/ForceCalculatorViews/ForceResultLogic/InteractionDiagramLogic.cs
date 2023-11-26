using LoaderCalculator.Data.Ndms;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class InteractionDiagramLogic : ILongProcessLogic
    {
        const double xmax = 0.7e6d; 
        const double xmin = -0.7e6d; 
        const double ymax = 1.5e6d; 
        const double ymin = -9e6d; 

        private ArrayParameter<double> arrayParameter;
        private static GeometryNames GeometryNames => ProgramSetting.GeometryNames;

        public int StepCount { get; }

        public Action<int> SetProgress { get; set; }
        public bool Result { get; set; }
        public IEnumerable<INdmPrimitive> NdmPrimitives { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public ForceTuple ForceTuple { get; set; }

        private void Show()
        {
            var options = new TriangulationOptions() { LimiteState = LimitState, CalcTerm = CalcTerm };
            var ndmCollection = NdmPrimitives.SelectMany(x => x.GetNdms(options));
            //var ndmCollection = NdmPrimitivesService.GetNdms(NdmPrimitives, LimitState, CalcTerm);

            var predicateFactory = new PredicateFactory()
            {
                My = ForceTuple.My,
                Ndms = ndmCollection
            };
            Predicate<IPoint2D> predicate = predicateFactory.GetResult;
            //var logic = new StabLimitCurveLogic();
            var logic = new LimitCurveLogic(predicate);
            var calculator = new LimitCurveCalculator(logic);
            calculator.SurroundData.XMax = xmax;
            calculator.SurroundData.XMin = xmin;
            calculator.SurroundData.YMax = ymax;
            calculator.SurroundData.YMin = ymin;
            calculator.SurroundData.PointCount = 40;
            calculator.Run();
            var result = calculator.Result;
            if (result.IsValid = false) { return; }
            var interactionResult = result as LimitCurveResult;
            var unitForce = CommonOperation.GetUnit(UnitTypes.Force, "kN");
            var unitMoment = CommonOperation.GetUnit(UnitTypes.Moment, "kNm");
            string[] labels = GetLabels(unitForce, unitMoment);
            var items = interactionResult.Points;
            arrayParameter = new ArrayParameter<double>(items.Count(), labels.Count(), labels);
            var data = arrayParameter.Data;
            for (int i = 0; i < items.Count(); i++)
            {
                var valueList = new List<double>
                {
                    items[i].X * unitForce.Multiplyer,
                    items[i].Y * unitMoment.Multiplyer
                };
                for (int j = 0; j < valueList.Count; j++)
                {
                    data[i, j] = valueList[j];
                }
                SetProgress?.Invoke(i);
            }
        }

        private static string[] GetLabels(IUnit unitForce, IUnit unitMoment)
        {
            return new string[]
            {
                $"{GeometryNames.MomFstName}, {unitMoment.Name}",
                $"{GeometryNames.LongForceName}, {unitForce.Name}"
            };
        }

        public void ShowWindow()
        {
            Show();
            Result = true;
            SafetyProcessor.RunSafeProcess(() =>
            {
                var wnd = new GraphView(arrayParameter);
                wnd.ShowDialog();
            },
            "Errors appeared during showing a graph, see detailed information");
        }

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
    }
}
