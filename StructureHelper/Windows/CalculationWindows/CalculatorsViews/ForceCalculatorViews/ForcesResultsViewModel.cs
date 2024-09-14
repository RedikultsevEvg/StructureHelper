using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.Exports;
using StructureHelper.Services.Reports;
using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.GeometryCalculatorViews;
using StructureHelper.Windows.Errors;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.PrimitivePropertiesWindow;
using StructureHelper.Windows.ViewModels.Calculations.Calculators;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.Geometry;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class ForcesResultsViewModel : ViewModelBase
    {
        private ShowDiagramLogic showDiagramLogic;
        private ForceCalculator forceCalculator;
        private ILongProcessLogic progressLogic;
        private ShowProgressLogic showProgressLogic;
        private InteractionDiagramLogic interactionDiagramLogic;
        private static readonly ShowCrackResultLogic showCrackResultLogic = new();
        //private static readonly ShowCrackWidthLogic showCrackWidthLogic = new();
        private IForcesResults forcesResults;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private IEnumerable<INdmPrimitive> selectedNdmPrimitives;
        private IEnumerable<INdm> ndms;
        private IReport isoFieldReport;

        public static GeometryNames GeometryNames => ProgramSetting.GeometryNames;

        public ForcesTupleResult SelectedResult { get; set; }
        private ICommand? showIsoFieldCommand;
        private ICommand? exportToCSVCommand;
        private ICommand? interpolateCommand;
        private ICommand? setPrestrainCommand;
        private ICommand? showAnchorageCommand;
        private ICommand? showGeometryResultCommand;
        private ICommand? showGraphsCommand;
        private ICommand? showCrackResult;
        private ICommand? showCrackGraphsCommand;
        private ICommand? showCrackWidthResult;
        private ICommand? showInteractionDiagramCommand;
        private ICommand? graphValuepointsCommand;

        public IForcesResults ForcesResults
        {
            get => forcesResults;
        }
        public ICommand ShowInteractionDiagramCommand
        {
            get
            {
                return showInteractionDiagramCommand ??
                    (showInteractionDiagramCommand = new RelayCommand(o =>
                    {
                        ShowInteractionDiagram();
                    }));
            }
        }

        private void ShowInteractionDiagram()
        {
            var inputData = new LimitCurveInputData(ndmPrimitives);
            var vm = new LimitCurveDataViewModel(inputData, ndmPrimitives);
            //vm.LimitStateItems.SetIsSelected();
            //vm.CalcTermITems.SetIsSelected();
            //vm.PredicateItems.SetIsSelected();
            var wnd = new LimitCurveDataView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult != true) return;
            if (vm.Check() == false)
            {
                MessageBox.Show(ErrorStrings.DataIsInCorrect + ": nothing selected"); ;
                return;
            }
            vm.RefreshInputData();
            ShowInteractionDiagramByInputData(inputData);
        }

        private void ShowInteractionDiagramByInputData(LimitCurveInputData inputData)
        {
            interactionDiagramLogic = new(inputData);
            showProgressLogic = new(interactionDiagramLogic)
            {
                WindowTitle = "Diagram creating...",
                ShowResult = interactionDiagramLogic.ShowWindow
            };
            showProgressLogic.Show();
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
                }, o => SelectedResult != null && SelectedResult.IsValid));
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
                InterpolateTuplesViewModel interpolateTuplesViewModel;
                InterpolateTuplesView wndTuples;
                ShowInterpolationWindow(out interpolateTuplesViewModel, out wndTuples);
                if (wndTuples.DialogResult != true) return;

                var interpolationLogic = new InterpolationProgressLogic(forceCalculator, interpolateTuplesViewModel.ForceInterpolationViewModel.Result);
                showProgressLogic = new(interpolationLogic)
                {
                    WindowTitle = "Interpolate forces"
                };
                showProgressLogic.Show();

                var result = interpolationLogic.InterpolateCalculator.Result;
                if (result is IForcesResults)
                {
                    var tupleResult = result as IForcesResults;
                    var diagramLogic = new ShowDiagramLogic(tupleResult.ForcesResultList, ndmPrimitives);
                    showProgressLogic = new(diagramLogic)
                    {
                        ShowResult = diagramLogic.ShowWindow,
                        WindowTitle = "Calculate crack diagram"
                    };
                    showProgressLogic.Show();
                }
            }, o => SelectedResult != null);
        }
        public ICommand ShowCrackGraphsCommand
        {
            get => showCrackGraphsCommand ??= new RelayCommand(o =>
            {
                InterpolateTuplesViewModel interploateTuplesViewModel;
                InterpolateTuplesView wndTuples;
                ShowInterpolationWindow(out interploateTuplesViewModel, out wndTuples);
                if (wndTuples.DialogResult != true) return;

                var interpolationLogic = new InterpolationProgressLogic(forceCalculator, interploateTuplesViewModel.ForceInterpolationViewModel.Result);
                showProgressLogic = new(interpolationLogic)
                {
                    WindowTitle = "Interpolate forces"
                };
                showProgressLogic.Show();

                var result = interpolationLogic.InterpolateCalculator.Result;
                if (result is IForcesResults)
                {
                    var tupleResult = result as IForcesResults;
                    var diagramLogic = new CrackDiagramLogic(tupleResult.ForcesResultList, ndmPrimitives);
                    showProgressLogic = new(diagramLogic)
                    {
                        ShowResult = diagramLogic.ShowWindow,
                        WindowTitle = "Calculate crack diagram"
                    };
                    showProgressLogic.Show();
                }
            }, o => SelectedResult != null && SelectedResult.IsValid
            );
        }
        public ICommand ShowCrackResultCommand
        {
            get => showCrackResult ??= new RelayCommand(o =>
            {
                SafetyProcessor.RunSafeProcess(ShowCrackResult);
            }, o => SelectedResult != null && SelectedResult.IsValid);
        }
        private void ShowCrackResult()
        {
            showCrackResultLogic.LimitState = SelectedResult.DesignForceTuple.LimitState;
            showCrackResultLogic.CalcTerm = CalcTerms.ShortTerm; //= SelectedResult.DesignForceTuple.CalcTerm;
            showCrackResultLogic.ForceTuple = SelectedResult.DesignForceTuple.ForceTuple;
            showCrackResultLogic.ndmPrimitives = ndmPrimitives;
            showCrackResultLogic.Show(SelectedResult.DesignForceTuple.Clone() as IDesignForceTuple);
        }

        //public ICommand ShowCrackWidthResultCommand
        //{
        //    get => showCrackWidthResult ??= new RelayCommand(o =>
        //    {
        //        SafetyProcessor.RunSafeProcess(ShowCrackWidthResult);
        //    }, o => SelectedResult != null && SelectedResult.IsValid);
        //}

        //private void ShowCrackWidthResult()
        //{
        //    showCrackWidthLogic.LimitState = SelectedResult.DesignForceTuple.LimitState;
        //    showCrackWidthLogic.CalcTerm = SelectedResult.DesignForceTuple.CalcTerm;
        //    showCrackWidthLogic.ForceTuple = SelectedResult.DesignForceTuple.ForceTuple;
        //    showCrackWidthLogic.ndmPrimitives = ndmPrimitives.ToList();
        //    showCrackWidthLogic.Show();
        //}
        public ICommand InterpolateCommand
        {
            get
            {
                return interpolateCommand ??
                    (interpolateCommand = new RelayCommand(o =>
                    {
                        InterpolateCurrentTuple();
                    }, o => SelectedResult != null));
            }
        }

        private void InterpolateCurrentTuple()
        {
            InterpolateTuplesViewModel interploateTuplesViewModel;
            InterpolateTuplesView wndTuples;
            ShowInterpolationWindow(out interploateTuplesViewModel, out wndTuples);
            if (wndTuples.DialogResult != true) return;

            var interpolationLogic = new InterpolationProgressLogic(forceCalculator, interploateTuplesViewModel.ForceInterpolationViewModel.Result);
            progressLogic = interpolationLogic;
            showProgressLogic = new(interpolationLogic);
            showProgressLogic.ShowResult = ShowInterpolationProgressDialog;
            showProgressLogic.Show();
        }

        public ICommand GraphValuePointsCommand
        {
            get
            {
                return graphValuepointsCommand ??
                    (graphValuepointsCommand = new RelayCommand(o =>
                    {
                        InterpolateValuePoints();
                    }, o => SelectedResult != null));
            }
        }

        private void InterpolateValuePoints()
        {
            if (SelectedResult is null)
            {
                throw new StructureHelperException(ErrorStrings.NullReference + ": Nothing is selected");
            }
            var logic = new InterpolateValuePointsLogic()
            {
                SelectedResult = SelectedResult,
                ForceCalculator = forceCalculator,
                NdmPrimitives = ndmPrimitives,
                ProgressLogic = progressLogic,
                ShowProgressLogic = showProgressLogic
            };
            logic.InterpolateValuePoints();
        }

        private void ShowInterpolationWindow(out InterpolateTuplesViewModel interploateTuplesViewModel, out InterpolateTuplesView wndTuples)
        {
            IDesignForceTuple finishDesignTuple = SelectedResult.DesignForceTuple.Clone() as IDesignForceTuple;
            interploateTuplesViewModel = new InterpolateTuplesViewModel(finishDesignTuple, null);
            wndTuples = new InterpolateTuplesView(interploateTuplesViewModel);
            wndTuples.ShowDialog();
        }

        private void ShowInterpolationProgressDialog()
        {
            if (progressLogic is InterpolationProgressLogic)
            {
                var interpolationLogic = progressLogic as InterpolationProgressLogic;
                var calculator = interpolationLogic.InterpolateCalculator;
                var vm = new ForcesResultsViewModel(calculator);
                var wnd = new ForceResultsView(vm);
                wnd.ShowDialog();
            }
        }

        public ICommand SetPrestrainCommand
        {
            get
            {
                return setPrestrainCommand ??
                    (setPrestrainCommand = new RelayCommand(o =>
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
                    ForceTupleService.CopyProperties(wnd.StrainTuple, item.NdmElement.AutoPrestrain);
                }
            }
        }
        public ICommand ShowAnchorageCommand
        {
            get
            {
                return showAnchorageCommand ??
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
        public ForcesResultsViewModel(ForceCalculator forceCalculator)
        {
            this.forceCalculator = forceCalculator;
            forcesResults = forceCalculator.Result as IForcesResults;
            ndmPrimitives = forceCalculator.InputData.Primitives;
        }

        private void ShowIsoField()
        {
            try
            {
                IStrainMatrix strainMatrix = SelectedResult.LoaderResults.ForceStrainPair.StrainMatrix;
                var primitiveSets = ShowIsoFieldResult.GetPrimitiveSets(strainMatrix, ndms, ForceResultFuncFactory.GetResultFuncs());
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
                if (item is IHasDivisionSize hasDivision)
                {

                    if (hasDivision.DivisionSize.ClearUnderlying == true)
                    {
                        ndmRange.RemoveAll(x =>
                        hasDivision
                        .IsPointInside(new Point2D()
                        {
                            X = x.CenterX, Y = x.CenterY
                        }
                        ) == true);
                    }
                }
                if (selectedNdmPrimitives.Contains(item) & item.NdmElement.Triangulate == true)
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
