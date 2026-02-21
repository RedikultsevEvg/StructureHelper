using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Services.ColorServices;
using FieldVisualizer.Services.PrimitiveServices;
using FieldVisualizer.Services.ValueRanges;
using HelixToolkit.Maths;
using HelixToolkit.Wpf.SharpDX;
using StructureHelper.Infrastructure;
using StructureHelper.Windows.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class IsoField3DViewerViewModel : ViewModelBase
    {
        const int RangeNumber = 16;
        private int userZoomFactor = 30;
        private IEnumerable<IPrimitiveSet> primitiveSets;
        private Element3D item0;
        private Element3D item1;
        private Element3D item2;
        private IPrimitiveSet selectedPrimitiveSet;
        private double zoomValue = 1.0;
        private IValueRange valueRange;
        private IEnumerable<IValueRange> valueRanges;
        private IEnumerable<IValueColorRange> valueColorRanges;
        private IColorMap _ColorMap;
        private ColorMapsTypes _ColorMapType;


        public ContourViewportViewModel ViewportViewModel { get; } = new ContourViewportViewModel();
        public IEnumerable<IPrimitiveSet> PrimitiveSets { get => primitiveSets;}
        public SaveCopyFWElementViewModel SaveCopyViewModel { get; private set; } = new();

        public IsoField3DViewerViewModel(IEnumerable<IPrimitiveSet> primitiveSets)
        {
            this.primitiveSets = primitiveSets;
            _ColorMapType = ColorMapsTypes.LiraSpectrum;
            _ColorMap = ColorMapFactory.GetColorMap(_ColorMapType);
        }
        public IPrimitiveSet SelectedPrimitiveSet
        {
            get => selectedPrimitiveSet;
            set
            {
                selectedPrimitiveSet = value;
                ViewportViewModel.Title = selectedPrimitiveSet.Name;
                ViewportViewModel.SubTitle = selectedPrimitiveSet.SubTitle;
                OnPropertyChanged(nameof(SelectedPrimitiveSet));
                RebuildPrimitives();
            }
        }

        public int UserZoomFactor
        {
            get => userZoomFactor;
            set
            {
                userZoomFactor = value;
                OnPropertyChanged(nameof(UserZoomFactor));
            }
        }

        public bool ShowZeroPlane
        {
            get => showZeroPlane;
            set
            {
                showZeroPlane = value;
                OnPropertyChanged(nameof(ShowZeroPlane));
            }
        }

        public bool InvertNormal
        {
            get => invertNormal;
            set
            {
                invertNormal = value;
                OnPropertyChanged(nameof(InvertNormal));
            }
        }

        private void RebuildPrimitives()
        {
            if (SelectedPrimitiveSet is null) { return; }
            SetColor();
            item0 = ViewportViewModel.Viewport3D.Items[0];
            item1 = ViewportViewModel.Viewport3D.Items[1];
            item2 = ViewportViewModel.Viewport3D.Items[2];
            ViewportViewModel.Viewport3D.Items.Clear();
            ViewportViewModel.Viewport3D.Items.Add(item0);
            ViewportViewModel.Viewport3D.Items.Add(item1);
            ViewportViewModel.Viewport3D.Items.Add(item2);
            double maxValue = SelectedPrimitiveSet.ValuePrimitives.Max(x => x.Value) - SelectedPrimitiveSet.ValuePrimitives.Min(x => x.Value);
            zoomValue = (Math.Pow(1.1, UserZoomFactor) - 1) / 100.0 / maxValue;
            var logic = new GetModels3dByValuePrimivesLogic()
            {
                ZoomValue = zoomValue,
                ShowZeroPlane = ShowZeroPlane,
                InvertNormal = InvertNormal,
                ColorMap = _ColorMap,
                ValueRange = valueRange,
            };
            logic.GetModels3d(SelectedPrimitiveSet.ValuePrimitives, ViewportViewModel.Viewport3D);
        }

        private void SetColor()
        {
            valueRange = PrimitiveOperations.GetValueRange(SelectedPrimitiveSet.ValuePrimitives);
            valueRanges = ValueRangeOperations.DivideValueRange(valueRange, RangeNumber);
            valueColorRanges = ColorOperations.GetValueColorRanges(valueRange, valueRanges, _ColorMap);
        }

        private RelayCommand rebuildCommand;
        private bool showZeroPlane = true;
        private bool invertNormal = false;

        public ICommand RebuildCommand => rebuildCommand ??= new RelayCommand(Rebuild);

        private void Rebuild(object commandParameter)
        {
            RebuildPrimitives();
        }
    }
}
