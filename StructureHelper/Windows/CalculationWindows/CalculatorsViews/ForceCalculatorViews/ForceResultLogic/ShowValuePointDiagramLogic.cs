using LoaderCalculator.Data.Materials.MaterialBuilders;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
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

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class ShowValuePointDiagramLogic //: ILongProcessLogic
    {
        private ArrayParameter<double> arrayParameter;
        private IValuePointDiagramLogic pointDiagramLogic;

        public IEnumerable<IForcesTupleResult> TupleList { get; set; }
        public ForceCalculator Calculator { get; set; }
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
            pointDiagramLogic.TupleList = TupleList;
            pointDiagramLogic.PrimitiveLogic = PrimitiveLogic;
            pointDiagramLogic.Calculator = Calculator;
            pointDiagramLogic.ValueDelegatesLogic = ValueDelegatesLogic;
            var results = pointDiagramLogic.GetArrayParameter();
            return results;
        }
    }
}
