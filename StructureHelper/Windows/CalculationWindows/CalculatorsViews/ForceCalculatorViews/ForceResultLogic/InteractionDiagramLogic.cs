using LoaderCalculator.Data.Ndms;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Calculators;
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
using System.Windows;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class InteractionDiagramLogic : ILongProcessLogic
    {
        private ArrayParameter<double> arrayParameter;
        private IResult result;

        private static GeometryNames GeometryNames => ProgramSetting.GeometryNames;

        public int StepCount => SurroundData.PointCount;

        public Action<int> SetProgress { get; set; }
        public bool Result { get; set; }
        public IEnumerable<INdmPrimitive> NdmPrimitives { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public ForceTuple ForceTuple { get; set; }

        public SurroundData SurroundData { get; set; }

        public InteractionDiagramLogic(SurroundData surroundData)
        {
            SurroundData = surroundData;
        }

        private void DoCalculations()
        {
            var ndmCollection = NdmPrimitivesService.GetNdms(NdmPrimitives, LimitState, CalcTerm);

            var predicateFactory = new PredicateFactory()
            {
                My = SurroundData.ConstZ,
                Ndms = ndmCollection
            };
            Predicate<IPoint2D> predicate = predicateFactory.IsSectionFailure;
            //Predicate<IPoint2D> predicate = predicateFactory.IsSectionCracked;
            //var logic = new StabLimitCurveLogic();
            var logic = new LimitCurveLogic(predicate);
            var calculator = new LimitCurveCalculator(logic);
            calculator.SurroundData = SurroundData;
            calculator.ActionToOutputResults = SetProgressByResult;
            SafetyProcessor.RunSafeProcess(() =>
            {
                CalcResult(calculator);
            }, "Errors appeared during showing a graph, see detailed information");
        }

        private void CalcResult(LimitCurveCalculator calculator)
        {
            calculator.Run();
            result = calculator.Result;
            if (result.IsValid == false) { return; }
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
            }
        }

        private void SetProgressByResult(IResult calcResult)
        {
            if (calcResult is not LimitCurveResult)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(LimitCurveResult), calcResult));
            }
            var parameterResult = calcResult as LimitCurveResult;
            SetProgress?.Invoke(parameterResult.IterationNumber);
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
            Result = true;
            SafetyProcessor.RunSafeProcess(() =>
            {
                if (result.IsValid == true)
                {
                    var wnd = new GraphView(arrayParameter);
                    wnd.ShowDialog();
                }
                else
                {
                    MessageBox.Show(result.Description);
                }
            },
            "Errors appeared during showing a graph, see detailed information");
        }

        public void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            DoCalculations();
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
