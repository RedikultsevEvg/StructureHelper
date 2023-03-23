using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.Reports;
using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.PrimitivePropertiesWindow;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelper.Windows.ViewModels.PrimitiveProperties;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmCalculations;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ForcesTupleResult SelectedResult { get; set; }
        private RelayCommand showIsoFieldCommand;
        private RelayCommand exportToCSVCommand;
        private RelayCommand interpolateCommand;
        private RelayCommand setPrestrainCommand;
        private ICommand showAnchorageCommand;

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
                    var vm = new SelectPrimitivesViewModel(ndmPrimitives);
                    var wnd = new SelectPrimitivesView(vm);
                    wnd.ShowDialog();
                    if (wnd.DialogResult == true)
                    {
                        selectedNdmPrimitives = vm.Items.CollectionItems.Where(x => x.IsSelected == true).Select(x => x.Item.GetNdmPrimitive());
                        GetNdms();
                        ShowIsoField();
                    }
                }, o => (SelectedResult != null) && SelectedResult.IsValid));
            }
        }

        public RelayCommand ExportToCSVCommand
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "csv |*.csv";
            saveFileDialog.Title = "Save an csv File";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filename = saveFileDialog.FileName;
                // If the file name is not an empty string open it for saving.
                if (filename != "")
                {
                    if (File.Exists(filename))
                    {
                        try
                        {
                            File.Delete(filename);
                        }
                        catch (Exception ex)
                        {
                            throw new StructureHelperException(ErrorStrings.FileCantBeDeleted + ex + filename);
                        }
                    }

                    try
                    {
                        var logic = new ExportToCSVLogic(saveFileDialog.FileName);
                        logic.Export(forcesResults);
                        try
                        {
                            Process filopener = new Process();
                            filopener.StartInfo.FileName = saveFileDialog.FileName;
                            filopener.Start();
                        }
                        catch (Exception) { }
                    }
                    catch (Exception ex)
                    {
                        throw new StructureHelperException(ErrorStrings.FileCantBeSaved + ex + filename);
                    }
                }
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
                MessageBox.Show(result.Desctription, "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    }, o => SelectedResult != null
                    ));
            }
        }

        private void showAnchorage()
        {
            throw new NotImplementedException();
        }

        public ForcesResultsViewModel(IForceCalculator forceCalculator)
        {
            this.forceCalculator = forceCalculator;
            this.forcesResults = this.forceCalculator.Result as IForcesResults;
            ndmPrimitives = this.forceCalculator.Primitives;
        }

        private void ShowIsoField()
        {
            IStrainMatrix strainMatrix = SelectedResult.LoaderResults.ForceStrainPair.StrainMatrix;
            var primitiveSets = ShowIsoFieldResult.GetPrimitiveSets(strainMatrix, ndms, ResultFuncFactory.GetResultFuncs());
            isoFieldReport = new IsoFieldReport(primitiveSets);
            isoFieldReport.Show();
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
    }
}
