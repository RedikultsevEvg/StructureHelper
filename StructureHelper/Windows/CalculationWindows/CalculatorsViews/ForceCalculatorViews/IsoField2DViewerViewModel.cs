using FieldVisualizer.Entities.Values.Primitives;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.UserControls.WorkPlanes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class IsoField2DViewerViewModel : ViewModelBase
    {
        private SelectedPrimitiveSet ndmPrimitiveSet;
        private RelayCommand zoomInCommand;
        private RelayCommand zoomOutCommand;
        private IPrimitiveSet selectedPrimitiveSet;
        private RelayCommand rebuildCommand;
        const int zoomFactor = 1000;

        public Canvas WorkPlaneCanvas => Window?.WorkPlaneRoot.WorkPlaneCanvas;
        public SaveCopyFWElementViewModel SaveCopyViewModel { get; } = new();
        public WorkPlaneRootViewModel WorkPlaneRoot { get; set; }

        public IsoField2DViewerView Window { get; internal set; }
        public IsoFieldTitleViewModel Title { get; private set; }
        public IsoFieldSummaryViewModel Summary { get; private set; }
        public List<IPrimitiveSet> PrimitiveSets { get; set; }
        public IPrimitiveSet SelectedPrimitiveSet
        {
            get => selectedPrimitiveSet;
            set
            {
                selectedPrimitiveSet = value;
                Refresh();
            }
        }
        public ColorMapViewModel ColorMapViewModel { get; set; }
        public ContoursRangeViewModel ContourRange { get; set; } = new();

        public ICommand RebuildCommand => rebuildCommand ??= new RelayCommand(o => Rebuild());
        public ICommand ZoomInCommand => zoomInCommand ??= new RelayCommand(o => Zoom(1.2));
        public ICommand ZoomOutCommand => zoomOutCommand ??= new RelayCommand(o => Zoom(0.8));


        public IsoField2DViewerViewModel(SelectedPrimitiveSet ndmPrimitiveSet)
        {
            WorkPlaneRoot = new()
            {
                IsToolBarVisible = false,
                IsStatusBarVisible = false
            };
            WorkPlaneRoot.WorkPlaneConfig.ScaleValue = 1;
            SaveCopyViewModel.FrameWorkElementServiseLogic.Dpi = 768;
            this.ndmPrimitiveSet = ndmPrimitiveSet;
            SetValues(ndmPrimitiveSet);
            ColorMapViewModel = new ColorMapViewModel(this);
            if (PrimitiveSets.Any())
            {
                SelectedPrimitiveSet = PrimitiveSets[0];
            }
            Refresh();
        }

        private void SetValues(SelectedPrimitiveSet ndmPrimitiveSet)
        {
            PrimitiveSets = ShowIsoFieldResult.GetPrimitiveSets(ndmPrimitiveSet.StrainMatrix, ndmPrimitiveSet.Ndms, ForceResultFuncFactory.GetResultFuncs());
        }

        public void Refresh()
        {
            if (SelectedPrimitiveSet is null) { return; }
            if (ColorMapViewModel is null) { return; }
            if (ContourRange is null) { return; }
            SetInfo();
            ContourRange.ColorMap = ColorMapViewModel.SelectedColorMap;
            double minValue = SelectedPrimitiveSet.ValuePrimitives.Min(x => x.Value);
            ContourRange.ValueRange.BottomValue = ContourRange.UserMinValue = minValue;
            double maxValue = SelectedPrimitiveSet.ValuePrimitives.Max(x => x.Value);
            ContourRange.ValueRange.TopValue = ContourRange.UserMaxValue = maxValue;
            ContourRange.Refresh();
            OnPropertyChanged(nameof(ContourRange));
            Rebuild();
        }

        private void SetInfo()
        {
            Title = new(SelectedPrimitiveSet);
            Title.Refresh();
            OnPropertyChanged(nameof(Title));
            Summary = new(SelectedPrimitiveSet);
            Summary.Refresh();
            OnPropertyChanged(nameof(Summary));
        }

        private void Rebuild()
        {
            if (WorkPlaneCanvas is not null)
            {
                DrawPrimitives();
            }
        }

        private void DrawPrimitives()
        {
            WorkPlaneCanvas.Children.Clear();
            double sizeX = FieldVisualizer.Services.PrimitiveServices.PrimitiveOperations.GetSizeX(SelectedPrimitiveSet.ValuePrimitives) * zoomFactor;
            double sizeY = FieldVisualizer.Services.PrimitiveServices.PrimitiveOperations.GetSizeY(SelectedPrimitiveSet.ValuePrimitives) * zoomFactor;
            var logic = new AddPrimitivesToCanvasLogic()
            {
                WorkPlaneCanvas = WorkPlaneCanvas,
                ValueRange = ContourRange.ValueRange,
                ColorMap = ColorMapViewModel.SelectedColorMap,
                ValueColorRanges = ContourRange.ContourLegend.ValueColorRanges,
                ValueLabelZoomFactor = Math.Max(sizeX, sizeY) * 0.0007
            };
            logic.ProcessPrimitives(SelectedPrimitiveSet.ValuePrimitives);
        }

        private void Zoom(double coefficient)
        {
            WorkPlaneRoot.WorkPlaneConfig.ScaleValue *= coefficient;
        }
    }
}
