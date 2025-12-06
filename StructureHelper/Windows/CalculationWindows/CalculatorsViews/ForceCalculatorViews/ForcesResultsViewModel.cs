using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure;
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
using StructureHelperCommon.Services.Exports.Factories;
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

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class ForcesResultsViewModel : ViewModelBase
    {
        private ShowDiagramLogic showDiagramLogic;
        private IForceCalculator forceCalculator;
        private ILongProcessLogic progressLogic;
        private ShowProgressLogic showProgressLogic;
        private InteractionDiagramLogic interactionDiagramLogic;
        private static readonly ShowCrackResultLogic showCrackResultLogic = new();
        private IForceCalculatorResult resultModel;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private IEnumerable<INdmPrimitive> selectedNdmPrimitives;
        private IEnumerable<INdm> ndms;
        private IReport isoFieldReport;
        private IForceTupleServiceLogic forceTupleServiceLogic;
        private IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();

        public static GeometryNames GeometryNames => ProgramSetting.GeometryNames;

        public IExtendedForceTupleCalculatorResult? SelectedResult { get; set; }
        private ICommand? showIsoFieldCommand;
        private ICommand? exportToCSVCommand;
        private ICommand? interpolateCommand;
        private ICommand? setPrestrainCommand;
        private ICommand? showAnchorageCommand;
        private ICommand? showGeometryResultCommand;
        private ICommand? showGraphsCommand;
        private ICommand? showCrackResult;
        private ICommand? showCrackGraphsCommand;
        //private ICommand? showCrackWidthResult;
        private ICommand? showInteractionDiagramCommand;
        private ICommand? graphValuepointsCommand;
        private ICommand showForceResultCommand;
        private RelayCommand showIsoField3DCommand;

        public ValidResultCounterVM ValidResultCounter { get; }

        public IForceCalculatorResult ForcesResults
        {
            get => resultModel;
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
            var inputData = new LimitCurvesCalculatorInputData(ndmPrimitives);
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

        private void ShowInteractionDiagramByInputData(LimitCurvesCalculatorInputData inputData)
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

        public ICommand ShowIsoField3DCommand
        {
            get
            {
                return showIsoField3DCommand ??= new RelayCommand(o =>
                {
                    if (SelectPrimitives() == true)
                    {
                        ShowIsoField3D();
                    }
                }, o => SelectedResult != null && SelectedResult.IsValid);
            }
        }

        private void ShowIsoField3D()
        {
            try
            {
                IStrainMatrix strainMatrix = SelectedResult.ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix;
                var primitiveSets = ShowIsoFieldResult.GetPrimitiveSets(strainMatrix, ndms, ForceResultFuncFactory.GetResultFuncs(), true);
                var report = new IsoField3DReport(primitiveSets);
                report.Show();
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

        public ICommand ExportToCSVCommand => exportToCSVCommand ??= new RelayCommand(o => { ExportToCSV();});
        private void ExportToCSV()
        {
            var logic = new ExportForcesResultToCSVLogic(resultModel);
            var exportService = new ExportToFileService(FileInputDataFactory.GetFileIOInputData(FileInputDataType.Csv), logic);
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

                var interpolationLogic = new InterpolationProgressLogic(forceCalculator, SelectedResult.StateCalcTermPair, interpolateTuplesViewModel.ForceInterpolationViewModel.Result);
                showProgressLogic = new(interpolationLogic)
                {
                    WindowTitle = "Interpolate forces"
                };
                showProgressLogic.Show();

                var result = interpolationLogic.InterpolateCalculator.Result;
                if (result is IForceCalculatorResult)
                {
                    var tupleResult = result as IForceCalculatorResult;
                    var diagramLogic = new ShowDiagramLogic(tupleResult.ForcesResultList, ndmPrimitives);
                    showProgressLogic = new(diagramLogic)
                    {
                        ShowResult = diagramLogic.ShowWindow,
                        WindowTitle = "Calculate crack diagram"
                    };
                    showProgressLogic.Show();
                }
            }, o => SelectedResult is not null);
        }
        public ICommand ShowCrackGraphsCommand
        {
            get => showCrackGraphsCommand ??= new RelayCommand(o =>
            {
                InterpolateTuplesViewModel interploateTuplesViewModel;
                InterpolateTuplesView wndTuples;
                ShowInterpolationWindow(out interploateTuplesViewModel, out wndTuples);
                if (wndTuples.DialogResult != true) return;

                var interpolationLogic = new InterpolationProgressLogic(forceCalculator, SelectedResult.StateCalcTermPair, interploateTuplesViewModel.ForceInterpolationViewModel.Result);
                showProgressLogic = new(interpolationLogic)
                {
                    WindowTitle = "Interpolate forces"
                };
                showProgressLogic.Show();

                var result = interpolationLogic.InterpolateCalculator.Result;
                if (result is IForceCalculatorResult)
                {
                    var tupleResult = result as IForceCalculatorResult;
                    var diagramLogic = new CrackDiagramLogic(tupleResult.ForcesResultList, ndmPrimitives);
                    showProgressLogic = new(diagramLogic)
                    {
                        ShowResult = diagramLogic.ShowWindow,
                        WindowTitle = "Calculate crack diagram"
                    };
                    showProgressLogic.Show();
                }
            }, o => SelectedResult != null && SelectedResult.IsValid);
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
            showCrackResultLogic.LimitState = SelectedResult.StateCalcTermPair.LimitState;
            showCrackResultLogic.CalcTerm = CalcTerms.ShortTerm;
            showCrackResultLogic.ForceTuple = SelectedResult.ForcesTupleResult.InputData.ForceTuple;
            showCrackResultLogic.ndmPrimitives = ndmPrimitives;
            showCrackResultLogic.Show(SelectedResult.ForcesTupleResult.InputData.ForceTuple.Clone() as IForceTuple);
        }

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

            var interpolationLogic = new InterpolationProgressLogic(forceCalculator, SelectedResult.StateCalcTermPair, interploateTuplesViewModel.ForceInterpolationViewModel.Result);
            progressLogic = interpolationLogic;
            showProgressLogic = new(interpolationLogic)
            {
                ShowResult = ShowInterpolationProgressDialog
            };
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
                    },
                    o => SelectedResult != null));
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
            ForceTuple startTuple = new();
            IForceTuple endTuple = SelectedResult.ForcesTupleResult.InputData.ForceTuple.Clone() as IForceTuple;
            interploateTuplesViewModel = new InterpolateTuplesViewModel(startTuple, endTuple, 100);
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
                    }, o => SelectedResult != null && SelectedResult.IsValid
                    ));
            }
        }
        private void SetPrestrain()
        {
            var source = ForceTupleConverter.ConvertToStrainTuple(SelectedResult.ForcesTupleResult.LoaderResults.StrainMatrix);
            var vm = new SetPrestrainViewModel(source);
            var wnd = new SetPrestrainView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                foreach (var item in ndmPrimitives)
                {
                    ForceTupleServiceLogic.CopyProperties(wnd.StrainTuple, item.NdmElement.AutoPrestrain);
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
                var strainMatrix = SelectedResult.ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix;
                var limitState = SelectedResult.StateCalcTermPair.LimitState;
                var calcTerm = SelectedResult.StateCalcTermPair.CalcTerm;

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
                    var strainMatrix = SelectedResult.ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix;
                    var textParametrsLogic = new GeometryParametersLogic(ndms, strainMatrix);
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
        public ICommand ShowForceResultCommand =>
            showForceResultCommand ??= new RelayCommand(o =>
            showForceResult(), o => SelectedResult != null && SelectedResult.IsValid);

        private void showForceResult()
        {
            if (SelectPrimitives() == true)
            {
                try
                {
                    var strainMatrix = SelectedResult.ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix;
                    var textParametrsLogic = new ForcesParametersLogic(ndms, strainMatrix);
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
            resultModel = forceCalculator.Result as IForceCalculatorResult;
            ValidResultCounter = new(resultModel.ForcesResultList);
            ndmPrimitives = forceCalculator.InputData.Primitives;
        }

        private void ShowIsoField()
        {
            try
            {
                IStrainMatrix strainMatrix = SelectedResult.ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix;
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
            var limitState = SelectedResult.StateCalcTermPair.LimitState;
            var calcTerm = SelectedResult.StateCalcTermPair.CalcTerm;
            var triangulationOptions = new TriangulationOptions()
            {
                LimiteState = limitState,
                CalcTerm = calcTerm };
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
