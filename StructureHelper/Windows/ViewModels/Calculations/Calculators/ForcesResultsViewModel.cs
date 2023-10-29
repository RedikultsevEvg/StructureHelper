using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure;
using StructureHelper.Services.Exports;
using StructureHelper.Services.Reports;
using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.GeometryCalculatorViews;
using StructureHelper.Windows.Errors;
using StructureHelper.Windows.PrimitivePropertiesWindow;
using StructureHelper.Windows.ViewModels.Calculations.Calculators.ForceResultLogic;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelper.Windows.ViewModels.PrimitiveProperties;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.Geometry;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    public class ForcesResultsViewModel : ViewModelBase
    {
        private static readonly ShowDiagramLogic showDiagramLogic = new();
        private static readonly InterpolateLogic interpolateLogic = new();
        private static readonly ShowCrackResultLogic showCrackResultLogic = new();
        private static readonly ShowCrackWidthLogic showCrackWidthLogic = new();
        private IForceCalculator forceCalculator;
        private IForcesResults forcesResults;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private IEnumerable<INdmPrimitive> selectedNdmPrimitives;
        private IEnumerable<INdm> ndms;
        private IReport isoFieldReport;

        public static GeometryNames GeometryNames => ProgramSetting.GeometryNames;

        public ForcesTupleResult SelectedResult { get; set; }
        private ICommand showIsoFieldCommand;
        private ICommand exportToCSVCommand;
        private ICommand interpolateCommand;
        private ICommand setPrestrainCommand;
        private ICommand showAnchorageCommand;
        private ICommand showGeometryResultCommand;
        private ICommand showGraphsCommand;
        private ICommand showCrackResult;
        private ICommand showCrackGraphsCommand;
        private RelayCommand showCrackWidthResult;

        public IForcesResults ForcesResults
        {
            get => forcesResults;
        }
        public ICommand ShowIsoFieldCommand
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
        public ICommand ShowGraphsCommand
        {
            get => showGraphsCommand ??= new RelayCommand(o =>
            {
                showDiagramLogic.Show(forcesResults.ForcesResultList);
            }
            );
        }
        public ICommand ShowCrackGraphsCommand
        {
            get => showCrackGraphsCommand ??= new RelayCommand(o =>
            {
                showDiagramLogic.ShowCracks(forcesResults.ForcesResultList, ndmPrimitives);
            }
            );
        }
        public ICommand ShowCrackResultCommand
        {
            get => showCrackResult ??= new RelayCommand(o =>
            {
                SafetyProcessor.RunSafeProcess(ShowCrackResult);
            }, o => (SelectedResult != null) && SelectedResult.IsValid);
        }
        private void ShowCrackResult()
        {
            showCrackResultLogic.LimitState = SelectedResult.DesignForceTuple.LimitState;
            showCrackResultLogic.CalcTerm = CalcTerms.ShortTerm; //= SelectedResult.DesignForceTuple.CalcTerm;
            showCrackResultLogic.ForceTuple = SelectedResult.DesignForceTuple.ForceTuple;
            showCrackResultLogic.ndmPrimitives = ndmPrimitives;
            showCrackResultLogic.Show(SelectedResult.DesignForceTuple.Clone() as IDesignForceTuple);
        }

        public ICommand ShowCrackWidthResultCommand
        {
            get => showCrackWidthResult ??= new RelayCommand(o =>
            {
                SafetyProcessor.RunSafeProcess(ShowCrackWidthResult);
            }, o => (SelectedResult != null) && SelectedResult.IsValid);
        }

        private void ShowCrackWidthResult()
        {
            showCrackWidthLogic.LimitState = SelectedResult.DesignForceTuple.LimitState;
            showCrackWidthLogic.CalcTerm = SelectedResult.DesignForceTuple.CalcTerm;
            showCrackWidthLogic.ForceTuple = SelectedResult.DesignForceTuple.ForceTuple;
            showCrackWidthLogic.ndmPrimitives = ndmPrimitives.ToList();
            showCrackWidthLogic.Show();
        }
        public ICommand InterpolateCommand
        {
            get
            {
                return interpolateCommand ??
                    (interpolateCommand = new RelayCommand(o =>
                    {
                        IDesignForceTuple finishDesignTuple = SelectedResult.DesignForceTuple.Clone() as IDesignForceTuple;
                        interpolateLogic.Show(finishDesignTuple, forceCalculator);
                    }, o => SelectedResult != null));
            }
        }
        public ICommand SetPrestrainCommand
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
            var source = TupleConverter.ConvertToStrainTuple(SelectedResult.LoaderResults.StrainMatrix);
            var vm = new SetPrestrainViewModel(source);
            var wnd = new SetPrestrainView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                foreach (var item in ndmPrimitives)
                {
                    ForceTupleService.CopyProperties(wnd.StrainTuple, item.AutoPrestrain);
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
            var triangulationOptions = new TriangulationOptions() { LimiteState = limitState, CalcTerm = calcTerm };
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
                    
                    ndmRange.AddRange(item.GetNdms(triangulationOptions));
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
