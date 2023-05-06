using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure;
using StructureHelper.Services.Exports;
using StructureHelper.Services.Reports;
using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.GeometryCalculatorViews;
using StructureHelper.Windows.Errors;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.PrimitivePropertiesWindow;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelper.Windows.ViewModels.Graphs;
using StructureHelper.Windows.ViewModels.PrimitiveProperties;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.Geometry;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmCalculations;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    public class ForcesResultsViewModel : ViewModelBase
    {
        private IForceCalculator forceCalculator;
        private IForcesResults forcesResults;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private IEnumerable<INdmPrimitive> selectedNdmPrimitives;
        private IEnumerable<INdm> ndms;
        private IReport isoFieldReport;

        static string firstAxisName => ProgramSetting.CrossSectionAxisNames.FirstAxis;
        static string secondAxisName => ProgramSetting.CrossSectionAxisNames.SecondAxis;
        static string thirdAxisName => ProgramSetting.CrossSectionAxisNames.ThirdAxis;

        public ForcesTupleResult SelectedResult { get; set; }
        private RelayCommand showIsoFieldCommand;
        private RelayCommand exportToCSVCommand;
        private RelayCommand interpolateCommand;
        private RelayCommand setPrestrainCommand;
        private ICommand showAnchorageCommand;
        private ICommand showGeometryResultCommand;

        public IForcesResults ForcesResults
        {
            get => forcesResults;
        }

        public RelayCommand ShowIsoFieldCommand
        {
            get
            {
                return showIsoFieldCommand ??
                (showIsoFieldCommand = new RelayCommand(o =>
                {
                    if (SelectPrimitives() == true)
                    {
                        ShowIsoField();
                    }
                }, o => (SelectedResult != null) && SelectedResult.IsValid));
            }
        }

        public ICommand ExportToCSVCommand
        {
            get
            {
                return exportToCSVCommand ??
                    (exportToCSVCommand = new RelayCommand(o =>
                    {
                        ExportToCSV();
                    }
                    ));
            }
        }
        private void ExportToCSV()
        {
            var inputData = new ExportToFileInputData();
            inputData.FileName = "New File";
            inputData.Filter = "csv |*.csv";
            inputData.Title = "Save in csv File";
            var logic = new ExportForceResultToCSVLogic(forcesResults);
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }

        private ICommand showGraphsCommand;

        public ICommand ShowGraphsCommand
        {
            get => showGraphsCommand ??= new RelayCommand(o => showGraphs());
        }

        private void showGraphs()
        {
            var unitForce = CommonOperation.GetUnit(UnitTypes.Force, "kN");
            var unitMoment = CommonOperation.GetUnit(UnitTypes.Moment, "kNm");
            var unitCurvature = CommonOperation.GetUnit(UnitTypes.Curvature, "1/m");

            var labels = new string[]
            {
                $"M{firstAxisName}, {unitMoment.Name}",
                $"M{secondAxisName}, {unitMoment.Name}",
                $"N{thirdAxisName}, {unitForce.Name}",
                $"K{firstAxisName}, {unitCurvature.Name}",
                $"K{secondAxisName}, {unitCurvature.Name}",
                $"Eps_{thirdAxisName}"
            };
            var resultList = forcesResults.ForcesResultList.Where(x => x.IsValid == true).ToList();
            var array = new ArrayParameter<double>(resultList.Count(), labels.Count(), labels);
            var data = array.Data;
            for (int i = 0; i < resultList.Count(); i++)
            {
                var valueList = new List<double>
                {
                    resultList[i].DesignForceTuple.ForceTuple.Mx * unitMoment.Multiplyer,
                    resultList[i].DesignForceTuple.ForceTuple.My * unitMoment.Multiplyer,
                    resultList[i].DesignForceTuple.ForceTuple.Nz * unitForce.Multiplyer,
                    resultList[i].LoaderResults.ForceStrainPair.StrainMatrix.Kx * unitCurvature.Multiplyer,
                    resultList[i].LoaderResults.ForceStrainPair.StrainMatrix.Ky * unitCurvature.Multiplyer,
                    resultList[i].LoaderResults.ForceStrainPair.StrainMatrix.EpsZ
                };
                for (int j = 0; j < valueList.Count(); j++)
                {
                    data[i, j] = valueList[j];
                }
            }
            var graphViewModel = new GraphViewModel(array);
            var wnd = new GraphView(graphViewModel);
            try
            {
                wnd.ShowDialog();
            }
            catch(Exception ex)
            {
                var vm = new ErrorProcessor()
                {
                    ShortText = "Errors apearred during showing isofield, see detailed information",
                    DetailText = $"{ex}"
                };
                new ErrorMessage(vm).ShowDialog();
            }
        }

        public RelayCommand InterpolateCommand
        {
            get
            {
                return interpolateCommand ??
                    (interpolateCommand = new RelayCommand(o =>
                    {
                        Interpolate();
                    }, o => SelectedResult != null));
            }
        }
        private void Interpolate()
        {
            IDesignForceTuple startDesignTuple, finishDesignTuple;
            finishDesignTuple = SelectedResult.DesignForceTuple.Clone() as IDesignForceTuple;
            var viewModel = new InterpolateTuplesViewModel(finishDesignTuple, null, 100);
            var wndTuples = new InterpolateTuplesView(viewModel);
            wndTuples.ShowDialog();
            if (wndTuples.DialogResult != true) return;
            startDesignTuple = viewModel.StartDesignForce;
            finishDesignTuple = viewModel.FinishDesignForce;
            int stepCount = viewModel.StepCount;
            var calculator = InterpolateService.InterpolateForceCalculator(forceCalculator, finishDesignTuple, startDesignTuple, stepCount);
            calculator.Run();
            var result = calculator.Result;
            if (result is null || result.IsValid == false)
            {
                MessageBox.Show(result.Description, "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var vm = new ForcesResultsViewModel(calculator);
                var wnd = new ForceResultsView(vm);
                wnd.ShowDialog();
            }
        }

        public RelayCommand SetPrestrainCommand
        {
            get
            {
                return setPrestrainCommand ??
                    (setPrestrainCommand = new RelayCommand(o=>
                    {
                        SetPrestrain();
                    }, o => SelectedResult != null
                    ));
            }
        }

        private void SetPrestrain()
        {
            var source = StrainTupleService.ConvertToStrainTuple(SelectedResult.LoaderResults.StrainMatrix);
            var vm = new SetPrestrainViewModel(source);
            var wnd = new SetPrestrainView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                foreach (var item in ndmPrimitives)
                {
                    StrainTupleService.CopyProperties(wnd.StrainTuple, item.AutoPrestrain);
                }
            }
        }

        public ICommand ShowAnchorageCommand
        {
            get
            {
                return showAnchorageCommand??
                    (showAnchorageCommand = new RelayCommand(o =>
                    {
                        showAnchorage();
                    }, o => SelectedResult != null && SelectedResult.IsValid
                    ));
            }
        }
        private void showAnchorage()
        {
            try
            {
                var strainMatrix = SelectedResult.LoaderResults.ForceStrainPair.StrainMatrix;
                var limitState = SelectedResult.DesignForceTuple.LimitState;
                var calcTerm = SelectedResult.DesignForceTuple.CalcTerm;

                var primitiveSets = ShowAnchorageResult.GetPrimitiveSets(strainMatrix, limitState, calcTerm, ndmPrimitives);
                isoFieldReport = new IsoFieldReport(primitiveSets);
                isoFieldReport.Show();
            }
            catch(Exception ex)
            {
                var vm = new ErrorProcessor()
                    { ShortText = "Errors apearred during showing isofield, see detailed information",
                    DetailText = $"{ex}"};
                new ErrorMessage(vm).ShowDialog();
            }
        }

        public ICommand ShowGeometryResultCommand =>
            showGeometryResultCommand ??= new RelayCommand(o =>
            showGeometryResult(), o => SelectedResult != null && SelectedResult.IsValid);
        private void showGeometryResult()
        {
            if (SelectPrimitives() == true)
            {
                try
                {
                    var strainMatrix = SelectedResult.LoaderResults.ForceStrainPair.StrainMatrix;
                    var textParametrsLogic = new TextParametersLogic(ndms, strainMatrix);
                    var calculator = new GeometryCalculator(textParametrsLogic);
                    calculator.Run();
                    var result = calculator.Result as IGeometryResult;         
                    var wnd = new GeometryCalculatorResultView(result);
                    wnd.ShowDialog();
                }
                catch (Exception ex)
                {
                    var vm = new ErrorProcessor()
                    {
                        ShortText = "Errors apearred during showing isofield, see detailed information",
                        DetailText = $"{ex}"
                    };
                    new ErrorMessage(vm).ShowDialog();
                }
            }
        }

        public ForcesResultsViewModel(IForceCalculator forceCalculator)
        {
            this.forceCalculator = forceCalculator;
            this.forcesResults = this.forceCalculator.Result as IForcesResults;
            ndmPrimitives = this.forceCalculator.Primitives;
        }

        private void ShowIsoField()
        {
            try
            {
                IStrainMatrix strainMatrix = SelectedResult.LoaderResults.ForceStrainPair.StrainMatrix;
                var primitiveSets = ShowIsoFieldResult.GetPrimitiveSets(strainMatrix, ndms, ResultFuncFactory.GetResultFuncs());
                isoFieldReport = new IsoFieldReport(primitiveSets);
                isoFieldReport.Show();
            }
            catch (Exception ex)
            {
                var vm = new ErrorProcessor()
                {
                    ShortText = "Errors apearred during showing isofield, see detailed information",
                    DetailText = $"{ex}"
                };
                new ErrorMessage(vm).ShowDialog();
            }

        }
        private void GetNdms()
        {
            var limitState = SelectedResult.DesignForceTuple.LimitState;
            var calcTerm = SelectedResult.DesignForceTuple.CalcTerm;
            var orderedNdmPrimitives = ndmPrimitives.OrderBy(x => x.VisualProperty.ZIndex);
            var ndmRange = new List<INdm>();
            foreach (var item in orderedNdmPrimitives)
            {
                if (item is IHasDivisionSize)
                {
                    var hasDivision = item as IHasDivisionSize;
                    if (hasDivision.ClearUnderlying == true)
                    {
                        ndmRange.RemoveAll(x => hasDivision.IsPointInside(new Point2D() { X = x.CenterX, Y = x.CenterY }) == true);
                    }
                }
                if (selectedNdmPrimitives.Contains(item) & item.Triangulate == true)
                {
                    ndmRange.AddRange(NdmPrimitivesService.GetNdms(item, limitState, calcTerm));
                }
            }
            ndms = ndmRange;
        }

        private bool SelectPrimitives()
        {
            var vm = new SelectPrimitivesViewModel(ndmPrimitives);
            var wnd = new SelectPrimitivesView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                selectedNdmPrimitives = vm.Items.CollectionItems.Where(x => x.IsSelected == true).Select(x => x.Item.GetNdmPrimitive());
                GetNdms();
                return true;
            }
            return false;
        }
    }
}
