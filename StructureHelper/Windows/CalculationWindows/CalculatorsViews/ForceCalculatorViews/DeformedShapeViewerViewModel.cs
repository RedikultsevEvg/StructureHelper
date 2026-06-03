using StructureHelper.Infrastructure;
using StructureHelper.Windows.Graphs;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;
using System.Windows.Input;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class DeformedShapeViewerViewModel : ViewModelBase
    {
        private RelayCommand rebuildCommand;
        private double scaleFactor = 100.0;
        private int divisionNumber = 20;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private bool isCapsShown = false;
        private double length = 3.0;

        public IForceTuple Curvature { get; set; }

        public double Length
        {
            get => length;
            set
            {
                try
                {
                    length = value;
                    OnPropertyChanged(nameof(Length));
                    Rebuild();
                }
                catch { }
            }
        }

        public double ScaleFactor
        {
            get => scaleFactor;
            set
            {
                try
                {
                    scaleFactor = value;
                    OnPropertyChanged(nameof(ScaleFactor));
                    Rebuild();
                }
                catch { }
            }
        }

        public int DivisionNumber
        {
            get => divisionNumber;
            set
            {
                try
                {
                    divisionNumber = value;
                    OnPropertyChanged(nameof(DivisionNumber));
                    Rebuild();
                }
                catch { }
            }
        }

        public bool IsCapsShown
        {
            get => isCapsShown;
            set
            {
                isCapsShown = value;
                OnPropertyChanged(nameof(IsCapsShown));
                Rebuild();
            }
        }

        public ContourViewportViewModel ViewportViewModel { get; } = new ContourViewportViewModel();
        public SaveCopyFWElementViewModel SaveCopyViewModel { get; private set; } = new();

        public ICommand RebuildCommand => rebuildCommand ??= new RelayCommand(Rebuild);
        public void Rebuild(object? commandParameter = null)
        {
            var logic = new DeformedShapeLogic()
            {
                NdmPrimitives = ndmPrimitives,
                Viewport = ViewportViewModel.Viewport3D,
                Curvature = Curvature,
                ScaleFactor = (float)ScaleFactor,
                DivisionNumber = DivisionNumber,
                IsCapsShown = IsCapsShown,
                Length = (float)Length
            };
            logic.GetModels3d();
        }

        public DeformedShapeViewerViewModel(IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            this.ndmPrimitives = ndmPrimitives;
        }

    }
}
