using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System.Collections.Generic;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class ShowValuePointDiagramLogic //: ILongProcessLogic
    {
        private ArrayParameter<double> arrayParameter;
        private IValuePointDiagramLogic pointDiagramLogic;

        public IEnumerable<IExtendedForceTupleCalculatorResult> TupleResultList { get; set; }
        public IForceCalculator Calculator { get; set; }
        public PointPrimitiveLogic PrimitiveLogic { get; set; }
        public ValueDelegatesLogic ValueDelegatesLogic { get; set; }

        //public int StepCount => throw new NotImplementedException();

        //public Action<int> SetProgress { get; set; }
        //public bool Result { get; set; }
        //public IShiftTraceLogger? TraceLogger { get; set; }
        public ShowValuePointDiagramLogic(IValuePointDiagramLogic pointDiagramLogic)
        {
            this.pointDiagramLogic = pointDiagramLogic;
        }
        public ShowValuePointDiagramLogic() : this(new ValuePointDiagramLogic())
        {
            
        }
        public void ShowWindow()
        {
            var result = GetResult();
            if (result.IsValid != true)
            {
                SafetyProcessor.ShowMessage(ErrorStrings.DataIsInCorrect, result.Description);
                return;
            }
            arrayParameter = result.Value;
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

        private GenericResult<ArrayParameter<double>> GetResult()
        {
            pointDiagramLogic.TupleList = TupleResultList;
            pointDiagramLogic.PrimitiveLogic = PrimitiveLogic;
            pointDiagramLogic.Calculator = Calculator;
            pointDiagramLogic.ValueDelegatesLogic = ValueDelegatesLogic;
            var results = pointDiagramLogic.GetArrayParameter();
            return results;
        }
    }
}
